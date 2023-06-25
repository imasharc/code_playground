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

        private readonly int rows = 15, columns = 15;
        // This array will make it easy to access the image for a given position in the grid
        private readonly Image[,] gridImages;
        private GameState gameState;

        public MainWindow()
        {
            InitializeComponent();
            gridImages = SetupGrid();
            gameState = new GameState(rows, columns);
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Draw();
            await GameLoop();
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
                        Source = Images.Empty
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
                }
            }
        }
    }
}
