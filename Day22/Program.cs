using System;
using System.Collections.Generic;
using System.IO;

namespace Day22
{
    class Program
    {
        static void Main(string[] args)
        {
            //PartOne(new string[] {
            //    "..#",
            //    "#..",
            //    "..."
            //});
            //PartOne(File.ReadAllLines("input.txt"));
            //PartTwo(new string[] {
            //    "..#",
            //    "#..",
            //    "..."
            //});
            PartTwo(File.ReadAllLines("input.txt"));

            Console.ReadLine();
        }

        static void PartOne(string[] input)
        {
            var grid = new Grid(input);
            var virus = new Virus();
            for (int i = 0; i < 10000; i++)
            {
                virus.Work(grid);
            }
            Console.WriteLine(virus.InfectionCount);
        }

        static void PartTwo(string[] input)
        {
            var grid = new Grid(input);
            var virus = new Virus();
            for (int i = 0; i < 10_000_000; i++)
            {
                virus.Work2(grid);
                if (i % 100_000 == 0)
                {
                    Console.WriteLine("Working... " + (i / 100_000) + "%");
                }
            }
            Console.WriteLine(virus.InfectionCount);
        }
    }

    class Grid
    {
        static readonly NodeComparer comparer = new NodeComparer();

        SortedList<Node, Node> nodes = new SortedList<Node, Node>(comparer);

        public Grid(string[] input)
        {
            int offsetY = input.Length / 2;
            int offsetX = input[0].Length / 2;

            for (int y = 0; y < input.Length; y++)
            {
                var row = input[y];
                for (int x = 0; x < row.Length; x++)
                {
                    char c = row[x];
                    var node = new Node(x - offsetX, y - offsetY, c == '#');
                    nodes.Add(node, node);
                }
            }
        }

        public Node GetNode(int x, int y)
        {
            var i = nodes.IndexOfKey(new Node(x, y, false));

            if (i < 0)
            {
                var newNode = new Node(x, y, false);
                nodes.Add(newNode, newNode);
                return newNode;
            }

            return nodes.Values[i];
        }
    }

    class NodeComparer : IComparer<Node>
    {
        public int Compare(Node node1, Node node2)
        {
            var x = node1.X.CompareTo(node2.X);
            if (x != 0)
            {
                return x;
            }
            else
            {
                return node1.Y.CompareTo(node2.Y);
            }
        }

        public int GetHashCode(Node node)
        {
            return node.X.GetHashCode() ^ node.Y.GetHashCode();
        }
    }

    class Node
    {
        public int X { get; set; }
        public int Y { get; set; }
        public bool IsInfected
        {
            get
            {
                return Status == Status.Infected;
            }
            set
            {
                if (value)
                {
                    Status = Status.Infected;
                }
                else
                {
                    Status = Status.Clean;
                }
            }
        }
        public Status Status { get; set; }

        public Node(int x, int y, bool isInfected)
        {
            X = x;
            Y = y;
            IsInfected = isInfected;
        }

        public override string ToString()
        {
            return $"{X},{Y}:{Status}";
        }
    }

    class Virus
    {
        public int CurrentX { get; set; }
        public int CurrentY { get; set; }
        public Direction CurrentDirection { get; set; }
        public int InfectionCount { get; set; }

        public void Work(Grid grid)
        {
            var node = grid.GetNode(CurrentX, CurrentY);

            if (node.IsInfected)
            {
                SpinRight();
                node.IsInfected = false;
                WalkForward();
            }
            else
            {
                SpinLeft();
                node.IsInfected = true;
                InfectionCount++;
                WalkForward();
            }
        }

        public void Work2(Grid grid)
        {
            var node = grid.GetNode(CurrentX, CurrentY);
            switch (node.Status)
            {
                case Status.Clean:
                    SpinLeft();
                    node.Status = Status.Weakened;
                    break;
                case Status.Weakened:
                    InfectionCount++;
                    node.Status = Status.Infected;
                    break;
                case Status.Infected:
                    SpinRight();
                    node.Status = Status.Flagged;
                    break;
                case Status.Flagged:
                    ReverseDirection();
                    node.Status = Status.Clean;
                    break;
            }
            WalkForward();
        }

        public void WalkForward()
        {
            switch (CurrentDirection)
            {
                case Direction.Up:
                    CurrentY--;
                    break;
                case Direction.Left:
                    CurrentX--;
                    break;
                case Direction.Down:
                    CurrentY++;
                    break;
                case Direction.Right:
                    CurrentX++;
                    break;
            }
        }

        public void SpinLeft()
        {
            switch (CurrentDirection)
            {
                case Direction.Up:
                    CurrentDirection = Direction.Left;
                    break;
                case Direction.Left:
                    CurrentDirection = Direction.Down;
                    break;
                case Direction.Down:
                    CurrentDirection = Direction.Right;
                    break;
                case Direction.Right:
                    CurrentDirection = Direction.Up;
                    break;
            }
        }

        public void SpinRight()
        {
            switch (CurrentDirection)
            {
                case Direction.Up:
                    CurrentDirection = Direction.Right;
                    break;
                case Direction.Right:
                    CurrentDirection = Direction.Down;
                    break;
                case Direction.Down:
                    CurrentDirection = Direction.Left;
                    break;
                case Direction.Left:
                    CurrentDirection = Direction.Up;
                    break;
            }
        }

        public void ReverseDirection()
        {
            switch (CurrentDirection)
            {
                case Direction.Up:
                    CurrentDirection = Direction.Down;
                    break;
                case Direction.Right:
                    CurrentDirection = Direction.Left;
                    break;
                case Direction.Down:
                    CurrentDirection = Direction.Up;
                    break;
                case Direction.Left:
                    CurrentDirection = Direction.Right;
                    break;
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

    enum Status
    {
        Clean,
        Weakened,
        Infected,
        Flagged
    }
}

