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
        private readonly int rows = 15, columns = 15;
        // This array will make it easy to access the image for a given position in the grid
        private readonly Image[,] gridImages;

        public MainWindow()
        {
            InitializeComponent();
            gridImages = SetupGrid();
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
    }
}
