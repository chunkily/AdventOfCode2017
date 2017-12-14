using System;
using System.Collections;
using System.Linq;
using System.Text;

namespace Day14
{
    class Program
    {
        static void Main(string[] args)
        {
            // PartOne("flqrgnkx");
            // Usage dotnet run -- flqrgnkx
            PartOne(args[0]);
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

            //var hash = Hash(input);
            //bool[] boolArr = new bool[128];
            //var i = 0;

            //foreach (char c in hash)
            //{
            //    var j = Int32.Parse(c.ToString(), System.Globalization.NumberStyles.HexNumber);
            //    // high bit first
            //    boolArr[i++] = (j & 8) != 0;
            //    boolArr[i++] = (j & 4) != 0;
            //    boolArr[i++] = (j & 2) != 0;
            //    boolArr[i++] = (j & 1) != 0;
            //}

            //return boolArr;
        }
    }
}
