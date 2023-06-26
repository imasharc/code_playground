using System;
using System.Collections.Generic;

namespace Snake
{
    public class Direction
    {
        public readonly static Direction Left = new Direction(0, -1);
        public readonly static Direction Right = new Direction(0, 1);
        public readonly static Direction Up = new Direction(-1, 0);
        public readonly static Direction Down = new Direction(1, 0);

        // We represent a direction in the grid by row and column offset from the starting point (0,0) - upper-left side of the grid
        public int RowOffset { get; }
        public int ColOffset { get; }

        // No other class can create an instance of the direction class - we only need 4 directions (up, down, left, right)
        private Direction(int rowOffset, int colOffset)
        {
            RowOffset = rowOffset;
            ColOffset = colOffset;
        }

        public Direction Opposite()
        {
            return new Direction(-RowOffset, -ColOffset);
        }

        // Override equals and getHashCode so the direction class can be used as KEY in the dictionary
        public override bool Equals(object obj)
        {
            return obj is Direction direction &&
                   RowOffset == direction.RowOffset &&
                   ColOffset == direction.ColOffset;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(RowOffset, ColOffset);
        }

        // We also have overloads of equality and inequality operators so we can use them to compare directions
        public static bool operator ==(Direction left, Direction right)
        {
            return EqualityComparer<Direction>.Default.Equals(left, right);
        }

        public static bool operator !=(Direction left, Direction right)
        {
            return !(left == right);
        }
    }
}
