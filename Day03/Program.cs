using System;
using System.Collections.Generic;

namespace Day03
{
    class Program
    {
        static void Main(string[] args)
        {
            // PartOne(1);
            // PartOne(12);
            // PartOne(23);
            // PartOne(1024);

            // Usage: dotnet run -- <your input here>
            // PartOne(Int32.Parse(args[0]));

            //var j = 1;
            //for (int i = 0; i < 23; i++)
            //{
            //   j = PartTwo(j);
            //}

            PartTwo(Int32.Parse(args[0]));

            Console.ReadLine();
        }

        static void PartOne(int number)
        {
            int ring = 0;
            int current = 1;

            // Find the largest bottom right corner
            while (Math.Pow(2 * (ring + 1) + 1, 2) < number)
            {
                ring++;
                current = (int)Math.Pow(2 * ring + 1, 2);
            }

            int xOffset = ring;
            int yOffset = -ring;

            Direction direction = Direction.Right;

            while(current < number)
            {
                current++;
                if(direction == Direction.Right)
                {
                    xOffset++;
                    if(xOffset > ring)
                    {
                        ring++;
                        direction = Direction.Up;
                    }
                }
                else if(direction == Direction.Up)
                {
                    yOffset++;
                    if(yOffset >= ring)
                    {
                        direction = Direction.Left;
                    }
                }
                else if (direction == Direction.Left)
                {
                    xOffset--;
                    if(xOffset <= -ring)
                    {
                        direction = Direction.Down;
                    }
                }
                else if (direction == Direction.Down)
                {
                    yOffset--;
                    if(yOffset <= -ring)
                    {
                        direction = Direction.Right;
                    }
                }
            }

            Console.WriteLine("number: " + number);
            Console.WriteLine("xOffset: " + xOffset);
            Console.WriteLine("yOffset: " + yOffset);
            Console.WriteLine("distance: " + (Math.Abs(xOffset) + Math.Abs(yOffset)));
        }

        static int PartTwo(int number)
        {
            int ring = 0;
            int value = 0;

            int xOffset = ring;
            int yOffset = -ring;

            List<GridCell> Grid = new List<GridCell>(number) {
                new GridCell() { Value= 1, XOffset = 0, YOffset = 0 }
            };

            Direction direction = Direction.Right;

            while (value <= number)
            {
                if (direction == Direction.Right)
                {
                    xOffset++;
                    if (xOffset > ring)
                    {
                        ring++;
                        direction = Direction.Up;
                    }
                }
                else if (direction == Direction.Up)
                {
                    yOffset++;
                    if (yOffset >= ring)
                    {
                        direction = Direction.Left;
                    }
                }
                else if (direction == Direction.Left)
                {
                    xOffset--;
                    if (xOffset <= -ring)
                    {
                        direction = Direction.Down;
                    }
                }
                else if (direction == Direction.Down)
                {
                    yOffset--;
                    if (yOffset <= -ring)
                    {
                        direction = Direction.Right;
                    }
                }

                GridCell cell = new GridCell() {
                    XOffset = xOffset,
                    YOffset = yOffset
                };

                value = 0;

                // Add the 8 adjacent values
                value += Grid.Find(c => c.XOffset == xOffset + 1 && c.YOffset == yOffset + 1)?.Value ?? 0;
                value += Grid.Find(c => c.XOffset == xOffset + 1 && c.YOffset == yOffset + 0)?.Value ?? 0;
                value += Grid.Find(c => c.XOffset == xOffset + 1 && c.YOffset == yOffset - 1)?.Value ?? 0;
                value += Grid.Find(c => c.XOffset == xOffset + 0 && c.YOffset == yOffset + 1)?.Value ?? 0;
                value += Grid.Find(c => c.XOffset == xOffset + 0 && c.YOffset == yOffset - 1)?.Value ?? 0;
                value += Grid.Find(c => c.XOffset == xOffset - 1 && c.YOffset == yOffset + 1)?.Value ?? 0;
                value += Grid.Find(c => c.XOffset == xOffset - 1 && c.YOffset == yOffset + 0)?.Value ?? 0;
                value += Grid.Find(c => c.XOffset == xOffset - 1 && c.YOffset == yOffset - 1)?.Value ?? 0;

                cell.Value = value;

                Grid.Add(cell);
            }

            Console.WriteLine("value: " + value);
            return value;
        }
    }

    public class GridCell
    {
        public int XOffset { get; set; }
        public int YOffset { get; set; }
        public int Value { get; set; }
    }

    enum Direction
    {
        Right,
        Up,
        Left,
        Down
    }
}
