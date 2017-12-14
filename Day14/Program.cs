using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Day14
{
    class Program
    {
        static void Main(string[] args)
        {
            // Usage dotnet run -- flqrgnkx

            // PartOne("flqrgnkx");
            // PartOne(args[0]);

            // PartTwo("flqrgnkx");
            PartTwo(args[0]);

            Console.ReadLine();
        }

        static void PartOne(string input)
        {
            //for (int j = 0; j < 8; j++)
            //{
            //    var ba = KnotHash.HashAsBoolArr(input + "-" + j);
            //    for (int i = 0; i < 8; i++)
            //    {
            //        Console.Write(ba[i] ? '#' : '.');
            //    }
            //    Console.WriteLine();
            //}

            int numUsed = 0;
            for (int j = 0; j < 128; j++)
            {
                var ba = KnotHash.HashAsBoolArr(input + "-" + j);
                // Count number of trues, and add to total
                numUsed += ba.Count(b => b);
            }
            Console.WriteLine(numUsed + " squares are used");
        }

        static void PartTwo(string input)
        {
            var usedSquares = new List<UsedGridCell>();

            for (int j = 0; j < 128; j++)
            {
                var ba = KnotHash.HashAsBoolArr(input + "-" + j);
                for (int i = 0; i < 128; i++)
                {
                    if (ba[i])
                    {
                        usedSquares.Add(new UsedGridCell(j, i));
                    }
                }
            }

            int groupId = 1;
            while (usedSquares.Any(s => !s.Visited))
            {
                usedSquares.First(s => !s.Visited).VisitAdjacent(usedSquares, groupId++);
            }

            int groupCount = usedSquares.Select(s => s.GroupId).Distinct().Count();
            Console.WriteLine("There are " + groupCount + " regions present.");
        }
    }

    class UsedGridCell
    {
        public UsedGridCell(int row, int col)
        {
            Row = row;
            Col = col;
        }

        public int Row { get; }
        public int Col { get; }
        public int GroupId { get; set; }
        public bool Visited
        {
            get
            {
                return GroupId != 0;
            }
        }

        public void VisitAdjacent(List<UsedGridCell> list, int groupId)
        {
            if (Visited)
            {
                // Already visited
                return;
            }
            GroupId = groupId;
            list.Find(c => c.Row == Row - 1 && c.Col == Col)?.VisitAdjacent(list, groupId);
            list.Find(c => c.Row == Row + 1 && c.Col == Col)?.VisitAdjacent(list, groupId);
            list.Find(c => c.Row == Row && c.Col == Col - 1)?.VisitAdjacent(list, groupId);
            list.Find(c => c.Row == Row && c.Col == Col + 1)?.VisitAdjacent(list, groupId);
        }
    }

    class KnotHash
    {
        private static int[] GetDenseHash(string input)
        {
            int[] lengths = new int[input.Length + 5];
            for (int i = 0; i < input.Length; i++)
            {
                lengths[i] = input[i];
            }
            int[] suffix = new int[] { 17, 31, 73, 47, 23 };
            suffix.CopyTo(lengths, input.Length);

            var arr = new int[256];
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = i;
            }

            int currentPosition = 0;
            int skipSize = 0;

            for (int z = 0; z < 64; z++)
            {
                foreach (var length in lengths)
                {
                    // Reverse section
                    var section = new int[length];
                    for (int i = 0; i < section.Length; i++)
                    {
                        var j = (currentPosition + i) % arr.Length;
                        section[i] = arr[j];
                    }
                    for (int i = 0; i < section.Length; i++)
                    {
                        var j = (currentPosition + i) % arr.Length;
                        var k = section.Length - i - 1;
                        arr[j] = section[k];
                    }

                    currentPosition += length + skipSize;
                    skipSize++;

                    currentPosition = currentPosition % arr.Length;
                }
            }

            int[] denseHash = new int[16];

            for (int i = 0; i < denseHash.Length; i++)
            {
                int first = i * 16;
                var last = first + 16;
                for (int j = first; j < last; j++)
                {
                    denseHash[i] = denseHash[i] ^ arr[j];
                }
            }

            return denseHash;
        }
        public static string Hash(string input)
        {
            StringBuilder sb = new StringBuilder();

            foreach (int i in GetDenseHash(input))
            {
                sb.AppendFormat("{0:x2}", i);
            }

            return sb.ToString();
        }


        public static bool[] HashAsBoolArr(string input)
        {
            var denseHash = GetDenseHash(input);
            bool[] boolArr = new bool[128];
            int i = 0;
            foreach (int j in denseHash)
            {
                boolArr[i++] = (j & 128) != 0;
                boolArr[i++] = (j & 64) != 0;
                boolArr[i++] = (j & 32) != 0;
                boolArr[i++] = (j & 16) != 0;
                boolArr[i++] = (j & 8) != 0;
                boolArr[i++] = (j & 4) != 0;
                boolArr[i++] = (j & 2) != 0;
                boolArr[i++] = (j & 1) != 0;
            }

            return boolArr;
        }
    }
}
