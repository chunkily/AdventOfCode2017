using System;
using System.IO;

namespace Day11
{
    class Program
    {
        static void Main(string[] args)
        {
            // string[] input = "se,sw,se,sw,sw".Split(',');
            string[] input = File.ReadAllText(args[0]).Split(',');
            PartOne(input);
            Console.ReadLine();
        }

        static void PartOne(string[] directions)
        {
            var origin = new HexCell(0, 0, 0);
            var cell = new HexCell(0, 0, 0);
            int maxDistance = 0;
            int currentDistance = 0;

            foreach (var direction in directions)
            {
                cell.MoveTo(direction);
                currentDistance = cell.DistanceFrom(origin);
                if (currentDistance > maxDistance)
                {
                    maxDistance = currentDistance;
                }
            }
            Console.WriteLine("you are " + currentDistance + " steps away from origin");
            Console.WriteLine("you have travelled the furthest distance of " + maxDistance + " steps away from origin");
        }
    }

    class HexCell
    {
        // https://www.redblobgames.com/grids/hexagons/
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }

        public HexCell(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public void MoveTo(string direction)
        {
            switch (direction)
            {
                case "nw":
                    X--;
                    Y++;
                    break;
                case "n":
                    Y++;
                    Z--;
                    break;
                case "ne":
                    X++;
                    Z--;
                    break;
                case "se":
                    X++;
                    Y--;
                    break;
                case "s":
                    Y--;
                    Z++;
                    break;
                case "sw":
                    X--;
                    Z++;
                    break;
                default:
                    throw new ArgumentException("Unknown direction", nameof(direction));
            }
        }

        public int DistanceFrom(HexCell other)
        {
            return (Math.Abs(X - other.X) + Math.Abs(Y - other.Y) + Math.Abs(Z - other.Z)) / 2;
        }
    }
}
