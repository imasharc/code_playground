using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Snake
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Add dictionary which maps grid values to image sources
        // OMITTING THE TYPE AFTER THE 'NEW' KEYWORD IS POSSIBLE IN NEWER C# VERSIONS ONLY! (FOR OLDER VERSIONS - JUST WRITE THE TYPE Dictionary<GridValue, ImageSource> AGAIN 
        private readonly Dictionary<GridValue, ImageSource> gridValueToImage = new()
        {
            { GridValue.Empty, Images.Empty },
            { GridValue.Snake, Images.Body },
            { GridValue.Food, Images.Food },
        };

        // if the Snake changes direction we will apply a rotation to "it's eyes"
        private readonly Dictionary<Direction, int> directionToRotation = new()
        {
            { Direction.Up, 0 },
            { Direction.Right, 90 },
            { Direction.Down, 180 },
            { Direction.Left, 270 }
        };

        private readonly int rows = 15, columns = 15;
        // This array will make it easy to access the image for a given position in the grid
        private readonly Image[,] gridImages;
        private GameState gameState;
        private bool gameRunning;

        public MainWindow()
        {
            InitializeComponent();
            gridImages = SetupGrid();
            gameState = new GameState(rows, columns);
        }

        private async Task RunGame()
        {
            Draw();
            await ShowCountDown();
            Overlay.Visibility = Visibility.Hidden;
            await GameLoop();
            await ShowGameOver();
            gameState = new GameState(rows, columns);
        }

        // When a user presses a key Window_PreviewwKeyDown is called and after that Window_KeyDown is also called
        private async void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (Overlay.Visibility == Visibility.Visible)
            {
                e.Handled = true;
            }

            if (!gameRunning)
            {
                gameRunning = true;
                await RunGame();
                gameRunning = false;
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            // If the game is over, pressing a key should have no effect
            if (gameState.GameOver)
            {
                return;
            }

            // Otherwise we check which key was pressed
            switch (e.Key)
            {
                case Key.Left:
                    gameState.ChangeDirection(Direction.Left);
                    break;
                case Key.Right:
                    gameState.ChangeDirection(Direction.Right);
                    break;
                case Key.Up:
                    gameState.ChangeDirection(Direction.Up);
                    break;
                case Key.Down:
                    gameState.ChangeDirection(Direction.Down);
                    break;
            }
        }

        // We can change the snake's direction but we need to move it at regular intervals
        // for that we add an async GameLoop method
        private async Task GameLoop()
        {
            while (!gameState.GameOver)
            {
                await Task.Delay(100);
                gameState.Move();
                Draw();
            }
        }

        // It will add the required image controls to the GameGrid and return them in a 2D array for easy access 
        private Image[,] SetupGrid()
        {
            Image[,] images = new Image[rows, columns];
            GameGrid.Rows = rows;
            GameGrid.Columns = columns;

            for (int row = 0; row < rows; row++)
            {
                for (int column = 0; column < columns; column++)
                {
                    // for each of them we create a new image
                    Image image = new Image
                    {
                        // Initialy we want source to be the Empty image asset
                        Source = Images.Empty,
                        // this makes the images rootate around the center point
                        RenderTransformOrigin = new Point(0.5, 0.5)
                    };

                    // We store this image in the 2D array
                    images[row, column] = image;
                    // and add it as a child of the GameGrid
                    GameGrid.Children.Add(image);
                }
            }

            return images;
        }

        private void Draw()
        {
            DrawGrid();
            DrawSnakeHead();
            ScoreText.Text = $"SCORE {gameState.Score}";
        }

        // This method will look at the grid array in the game state and update the grid images to reflect it
        private void DrawGrid()
        {
            // It loops through every grid position
            for (int row = 0; row < rows; row++)
            {
                for (int column = 0; column < columns; column++)
                {
                    // Get the GridValue at the current position and set the source for the corresponding image using our dictionary
                    GridValue gridValue = gameState.Grid[row, column];
                    gridImages[row, column].Source = gridValueToImage[gridValue];
                    // reset the RenderTransform of the images
                    // this ensures that the only rotated image is the one with the snake's head
                    gridImages[row, column].RenderTransform = Transform.Identity;
                }
            }
        }

        private void DrawSnakeHead()
        {
            Position headPosition = gameState.HeadPosition();
            Image image = gridImages[headPosition.Row, headPosition.Column];
            image.Source = Images.Head;

            int rotation = directionToRotation[gameState.Direction];
            image.RenderTransform = new RotateTransform(rotation);
        }

        private async Task ShowCountDown()
        {
            for (int i = 3; i >= 1; i--)
            {
                OverlayText.Text = i.ToString();
                await Task.Delay(500);
            }
        }

        private async Task ShowGameOver()
        {
            await Task.Delay(1000);
            Overlay.Visibility = Visibility.Visible;
            OverlayText.Text = "PRESS ANY KEY TO START";
        }
    }
}
