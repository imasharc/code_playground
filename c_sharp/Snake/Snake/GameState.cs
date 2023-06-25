using System;
using System.Collections.Generic;

namespace Snake
{
    // This class stores the current state of the game
    public class GameState
    {
        public int Rows { get; }
        public int Columns { get; }
        
        // 2-dimensional rectangular array of grid values
        public GridValue[,] Grid { get; }
        public Direction Direction { get; private set; }
        public int Score {  get; private set; }
        public bool GameOver { get; private set; }
        
        // List containing the positions currently occupied by the snake
        // LinkedList allows to add and delete from both ends of the list
        // We use the convention that the first element is the HEAD of the snake and the last element is the TAIL
        private readonly LinkedList<Position> snakePositions = new LinkedList<Position>();

        // To figure out where the food should spawn
        private readonly Random random = new Random();

        public GameState(int rows, int columns)
        {
            Rows = rows;
            Columns = columns;
            // At this point every in the array will contain GridValue.Empty because it's the first enum value
            Grid = new GridValue[rows, columns];
            // When the game starts, snake's direction will be Right
            Direction = Direction.Right;

            AddSnake();
            AddFood();
        }

        private void AddSnake()
        {
            // Snake will appear in the middle row, in column 1-3
            // If we use an even number of rows then this rows will be slightly closer to the top
            int row = Rows / 2;

            for (int column = 1; column <= 3; column++)
            {
                Grid[row, column] = GridValue.Snake;
                snakePositions.AddFirst(new Position(row, column));
            }
        }

        // Return all empty grid positions
        private IEnumerable<Position> EmptyPositions()
        {
            for (int row = 0; row < Rows; row++)
            {
                for (int column = 0;  column < Columns; column++)
                {
                    // Check if the grid at (row, column) is empty
                    if (Grid[row, column] == GridValue.Empty)
                    {
                        yield return new Position(row, column);
                    }
                }
            }
        }

        private void AddFood()
        {
            List<Position> empty = new List<Position>(EmptyPositions());
            
            // It is theoretically possible to beat Snake, in which case there wouldn't be any empty positions
            // To prevent game from crashing:
            if (empty.Count == 0)
            {
                return;
            }

            // In a general case, we pick the empty position at random
            Position position = empty[random.Next(empty.Count)];
            // and set the corresponding array entry to GridValue.Food
            Grid[position.Row, position.Column] = GridValue.Food;
        }

        public Position HeadPosition()
        {
            return snakePositions.First.Value;
        }

        public Position TailPosition()
        {
            return snakePositions.Last.Value;
        }

        public IEnumerable<Position> SnakePositions()
        {
            return snakePositions;
        }

        // methods for modifying the snake
        public void AddHead(Position position)
        {
            // Adds the given position to the front of the snake making it the new head
            snakePositions.AddFirst(position);
            // set the corresponding entry of the grid array to GridValue.Snake
            Grid[position.Row, position.Column] = GridValue.Snake;
        }

        public void RemoveTail(Position position)
        {
            // Get the current tail position
            Position tail = snakePositions.Last.Value;
            // make that position empty in the grid
            Grid[tail.Row, tail.Column] = GridValue.Empty;
            // and remove it from the LinkedList
            snakePositions.RemoveLast();
        }

        // methods for modifying the game state
        public void ChangeDirection(Direction direction)
        {
            // TOO SIMPLISTIC, TO BE CHANGED
            Direction = direction;
        }

        // Check if the given position is outside the grid or not
        private bool OutsideGrid(Position position)
        {
            return position.Row < 0 || position.Row >= Rows || position.Column < 0 || position.Column >= Columns;
        }

        // Return what the snake would hit if it moves there
        private GridValue WillHit(Position newHeadPosition)
        {
            // Special cases to handle:
            // If the newHeadPosition would be outside the grid
            if (OutsideGrid(newHeadPosition))
            {
                return GridValue.Outside;
            }

            // If the newHeadPosition would be where the Tail is currently at
            if (newHeadPosition == TailPosition())
            {
                return GridValue.Empty;
            }

            // In the general case, it will return whatever is stored in the grid at that position
            return Grid[newHeadPosition.Row, newHeadPosition.Column];
        }

        // Move the snake one step in the current direction
        public void Move()
        {
            Position newHeadPosition = HeadPosition().Translate(Direction);
            GridValue hit = WillHit(newHeadPosition);

            if (hit == GridValue.Outside || hit == GridValue.Snake)
            {
                GameOver = true;
            }
            else if (hit == GridValue.Empty)
            {
                RemoveTail(TailPosition());
                AddHead(newHeadPosition);
            }
            else if (hit == GridValue.Food)
            {
                AddHead(newHeadPosition);
                Score++;
                AddFood();
            }
        }
    }
}
