using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Day21
{
    class Program
    {
        static void Main(string[] args)
        {
            //PartOne(new string[] {
            //    "../.# => ##./#../...",
            //    ".#./..#/### => #..#/..../..../#..#"
            //});
            //PartOne(File.ReadAllLines("input.txt"));
            PartTwo(File.ReadAllLines("input.txt"));
            Console.ReadLine();
        }

        static void PartOne(string[] input)
        {
            Solve(input, 5);
        }

        static void PartTwo(string[] input)
        {
            Solve(input, 18);
        }

        static void Solve(string[] inputs, int numIterations)
        {
            // Rules are indexed by the number of trues in the input
            List<List<Transformation>> rules2 = new List<List<Transformation>>();
            List<List<Transformation>> rules3 = new List<List<Transformation>>();

            for (int i = 0; i < 5; i++)
            {
                rules2.Add(new List<Transformation>());
            }
            for (int i = 0; i < 10; i++)
            {
                rules3.Add(new List<Transformation>());
            }

            foreach (var line in inputs)
            {
                var rule = new Transformation(line);

                if (rule.InputSize == 2)
                {
                    rules2[rule.NumInputTrue].Add(rule);
                }
                else
                {
                    rules3[rule.NumInputTrue].Add(rule);
                }
            }

            Grid grid = new Grid();
            grid.Update(new char[] { '.', '#', '.', '.', '.', '#', '#', '#', '#' }, 3);

            for (int i = 0; i < numIterations; i++)
            {
                var s = grid.Split();
                foreach (var row in s)
                {
                    foreach (var subGrid in row)
                    {
                        int numTrue = subGrid.GetTrueCount();
                        if (subGrid.Size == 2)
                        {
                            foreach (var rule in rules2[numTrue])
                            {
                                rule.Transform(subGrid);
                            }
                        }
                        else
                        {
                            foreach (var rule in rules3[numTrue])
                            {
                                rule.Transform(subGrid);
                            }
                        }
                    }
                }
                grid = new Grid(s);
                // grid.Print();
            }

            Console.WriteLine(grid.GetTrueCount() + " pixels are on");
        }
    }

    class Transformation
    {
        static readonly Regex regex = new Regex(@"^(.+) => (.+)$");

        public int NumInputTrue { get; set; }
        public int InputSize { get; set; }
        public string[] ValidInputs { get; set; }
        public char[] Output { get; set; }

        public Transformation(string input)
        {
            var match = regex.Match(input);

            string inputString = match.Groups[1].Value;

            NumInputTrue = inputString.Count(c => c == '#');

            var inputStrings = inputString.Split('/');
            if (inputStrings.Length == 2)
            {
                InputSize = 2;
                ValidInputs = BuildInputsSizeTwo(inputStrings);
            }
            else
            {
                InputSize = 3;
                ValidInputs = BuildInputsSizeThree(inputStrings);
            }

            string outputString = match.Groups[2].Value;

            string[] rows = outputString.Split('/');
            int size = rows.Length;
            Output = new char[size * size];
            for (int i = 0; i < rows.Length; i++)
            {
                var row = rows[i];
                for (int j = 0; j < row.Length; j++)
                {
                    Output[i * size + j] = row[j];
                }
            }
        }
        public static string[] BuildInputsSizeTwo(string[] input)
        {
            var validInputs = new string[8];
            char a = input[0][0];
            char b = input[0][1];
            char c = input[1][0];
            char d = input[1][1];

            // original
            validInputs[0] = new string(new char[] { a, b, c, d }, 0, 4);
            // rotate 90 clockwise
            validInputs[1] = new string(new char[] { c, a, d, b }, 0, 4);
            // rotate 180 clockwise
            validInputs[2] = new string(new char[] { d, c, b, a }, 0, 4);
            // rotate 270 clockwise
            validInputs[3] = new string(new char[] { b, d, a, c }, 0, 4);
            // flip
            validInputs[4] = new string(new char[] { c, d, a, b }, 0, 4);
            // rotate 90 clockwise, flip
            validInputs[5] = new string(new char[] { d, b, c, a }, 0, 4);
            // rotate 180 clockwise, flip
            validInputs[6] = new string(new char[] { b, a, d, c }, 0, 4);
            // rotate 270 clockwise, flip
            validInputs[7] = new string(new char[] { a, c, b, d }, 0, 4);

            return validInputs;
        }

        private static string[] BuildInputsSizeThree(string[] input)
        {
            var validInputs = new string[8];
            char a = input[0][0];
            char b = input[0][1];
            char c = input[0][2];
            char d = input[1][0];
            char e = input[1][1];
            char f = input[1][2];
            char g = input[2][0];
            char h = input[2][1];
            char i = input[2][2];

            validInputs = new string[8];

            // original
            validInputs[0] = new string(new char[] { a, b, c, d, e, f, g, h, i }, 0, 9);
            // rotate 90 clockwise
            validInputs[1] = new string(new char[] { g, d, a, h, e, b, i, f, c }, 0, 9);
            // rotate 180 clockwise
            validInputs[2] = new string(new char[] { i, h, g, f, e, d, c, b, a }, 0, 9);
            // rotate 270 clockwise
            validInputs[3] = new string(new char[] { c, f, i, b, e, h, a, d, g }, 0, 9);
            // flip
            validInputs[4] = new string(new char[] { g, h, i, d, e, f, a, b, c }, 0, 9);
            // rotate 90 clockwise, flip
            validInputs[5] = new string(new char[] { i, f, c, h, e, b, g, d, a }, 0, 9);
            // rotate 180 clockwise, flip
            validInputs[6] = new string(new char[] { c, b, a, f, e, d, i, h, g }, 0, 9);
            // rotate 270 clockwise, flip
            validInputs[7] = new string(new char[] { a, d, g, b, e, h, c, f, i }, 0, 9);

            return validInputs;
        }

        public void Transform(Grid grid)
        {
            if (ValidInputs.Contains(grid.ToString()))
            {
                grid.Update(Output, InputSize + 1);
            }
        }
    }

    class Grid
    {
        public int Size { get; set; }
        char[] values;

        // Default constructor
        public Grid() { }

        // Rebuild Grid
        public Grid(List<List<Grid>> grids)
        {
            // Assume all grids are of equal size.
            int gridSize = grids[0][0].Size;
            int numGridsPer = grids.Count;
            values = new char[gridSize * numGridsPer * gridSize * numGridsPer];

            for (int gi = 0; gi < numGridsPer; gi++)
            {
                for (int gj = 0; gj < numGridsPer; gj++)
                {
                    var grid = grids[gi][gj];
                    var v = grid.values;

                    for (int i = 0; i < gridSize; i++)
                    {
                        for (int j = 0; j < gridSize; j++)
                        {
                            char c = v[i + gridSize * j];

                            int offset =
                                gi * gridSize
                                + i
                                + gj * gridSize * gridSize * numGridsPer
                                + j * gridSize * numGridsPer;
                            values[offset] = c;
                        }
                    }
                }
            }

            Size = gridSize * numGridsPer;
        }

        public void Update(char[] newValues, int size)
        {
            Size = size;
            values = newValues;
        }

        public List<List<Grid>> Split()
        {
            List<List<Grid>> grids = new List<List<Grid>>();

            int gridSize = 3;
            if (Size % 2 == 0)
            {
                gridSize = 2;
            }
            int numGridsPer = Size / gridSize;
            for (int gi = 0; gi < numGridsPer; gi++)
            {
                var row = new List<Grid>();
                for (int gj = 0; gj < numGridsPer; gj++)
                {
                    var grid = new Grid();
                    var c = new char[gridSize * gridSize];
                    for (int i = 0; i < gridSize; i++)
                    {
                        for (int j = 0; j < gridSize; j++)
                        {
                            int offset = gi * gridSize
                                + i
                                + gj * gridSize * gridSize * numGridsPer
                                + j * gridSize * numGridsPer;

                            c[i + j * gridSize] = values[offset];
                        }
                    }
                    grid.Update(c, gridSize);
                    row.Add(grid);
                }

                grids.Add(row);
            }

            return grids;
        }

        public int GetTrueCount()
        {
            return values.Count(b => b == '#');
        }

        public override string ToString()
        {
            return string.Join("", values);
        }

        public void Print()
        {
            for (int i = 0; i < values.Length; i++)
            {
                if (i % Size == 0)
                {
                    Console.WriteLine();
                }
                Console.Write(values[i]);
            }
            Console.WriteLine();
        }
    }
}
