using System;
using System.Collections.Generic;
using System.IO;

namespace Day19
{
    class Program
    {
        static void Main(string[] args)
        {
            //PartOne(new string[] {
            //    "     |          ",
            //    "     |  +--+    ",
            //    "     A  |  C    ",
            //    " F---|----E|--+ ",
            //    "     |  |  |  D ",
            //    "     +B-+  +--+ ",
            //    "                "
            //});
            //PartOne(File.ReadAllLines("input.txt"));

            //PartTwo(new string[] {
            //    "     |          ",
            //    "     |  +--+    ",
            //    "     A  |  C    ",
            //    " F---|----E|--+ ",
            //    "     |  |  |  D ",
            //    "     +B-+  +--+ ",
            //    "                "
            //});
            PartTwo(File.ReadAllLines("input.txt"));

            Console.ReadLine();
        }

        static void PartOne(string[] input)
        {
            var grid = new Grid(input);
            grid.FindStartPoint();

            while (grid.Walk())
            {
            }

            Console.WriteLine(grid.Output);
        }

        static void PartTwo(string[] input)
        {
            var grid = new Grid(input);
            grid.FindStartPoint();

            int count = 1;
            while (grid.Walk())
            {
                count++;
            }

            Console.WriteLine(count);
        }
    }

    class Grid
    {
        // List<List<char>> grid;
        string[] grid;
        string output = "";

        int currentX = 0;
        int currentY = 0;
        Direction currentDirection;

        public string Output { get => output; }

        public Grid(string[] input)
        {
            grid = input;
        }

        public void FindStartPoint()
        {
            // Check first row
            bool found = false;
            var firstRow = grid[0];
            for (int y = 0; y < grid[0].Length && !found; y++)
            {
                if (firstRow[y] == '|')
                {
                    found = true;
                    currentX = 0;
                    currentY = y;
                    currentDirection = Direction.Down;
                }
            }
            if (found)
            {
                return;
            }

            // TODO: Check bottom row and vertical
        }

        /// <summary>
        /// Walks in the current direction. If cannot proceed anymore returns false
        /// </summary>
        /// <returns>false if cannot continue, true otherwise</returns>
        public bool Walk()
        {
            var currentCell = grid[currentX][currentY];
            if (IsLetter(currentCell))
            {
                output += currentCell;
            }

            switch (currentDirection)
            {
                case Direction.Up:
                    return WalkUp();
                case Direction.Down:
                    return WalkDown();
                case Direction.Left:
                    return WalkLeft();
                case Direction.Right:
                    return WalkRight();
                default:
                    return false;
            }
        }

        private bool IsLetter(char c)
        {
            return (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z');
        }

        public bool WalkUp()
        {
            if (grid[currentX - 1][currentY] == ' ')
            {
                // Cannot go up, look left and right

                if (grid[currentX][currentY - 1] != ' ')
                {
                    currentDirection = Direction.Left;
                    currentY--;
                    return true;
                }

                if (grid[currentX][currentY + 1] != ' ')
                {
                    currentDirection = Direction.Right;
                    currentY++;
                    return true;
                }

                // If we got here we have reached the end of the path.
                return false;
            }
            else
            {
                currentX--;
                return true;
            }
        }

        public bool WalkDown()
        {
            if (grid[currentX + 1][currentY] == ' ')
            {
                // Cannot go down, look left and right

                if (grid[currentX][currentY - 1] != ' ')
                {
                    currentDirection = Direction.Left;
                    currentY--;
                    return true;
                }

                if (grid[currentX][currentY + 1] != ' ')
                {
                    currentDirection = Direction.Right;
                    currentY++;
                    return true;
                }

                // If we got here we have reached the end of the path.
                return false;
            }
            else
            {
                currentX++;
                return true;
            }

        }

        public bool WalkLeft()
        {
            if (grid[currentX][currentY - 1] == ' ')
            {
                // Cannot go left, look up and down

                if (grid[currentX - 1][currentY] != ' ')
                {
                    currentDirection = Direction.Up;
                    currentX--;
                    return true;
                }

                if (grid[currentX + 1][currentY] != ' ')
                {
                    currentDirection = Direction.Down;
                    currentX++;
                    return true;
                }

                // If we got here we have reached the end of the path.
                return false;
            }
            else
            {
                currentY--;
                return true;
            }
        }

        public bool WalkRight()
        {
            if (grid[currentX][currentY + 1] == ' ')
            {
                // Cannot go right, look up and down

                if (grid[currentX - 1][currentY] != ' ')
                {
                    currentDirection = Direction.Up;
                    currentX--;
                    return true;
                }

                if (grid[currentX + 1][currentY] != ' ')
                {
                    currentDirection = Direction.Down;
                    currentX++;
                    return true;
                }

                // If we got here we have reached the end of the path.
                return false;
            }
            else
            {
                currentY++;
                return true;
            }
        }
    }

    enum Direction
    {
        Up,
        Right,
        Down,
        Left
    }
}
