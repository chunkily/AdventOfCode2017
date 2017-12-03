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
            PartOne(Int32.Parse(args[0]));
            Console.ReadLine();
        }

        static void PartOne(int number)
        {
            int ring = 0;
            int current = 1;

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

    }

    enum Direction
    {
        Right,
        Up,
        Left,
        Down
    }
}
