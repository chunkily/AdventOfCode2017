using System;
using System.Text;

namespace Day14
{
    class Program
    {
        static void Main(string[] args)
        {
            int[,] grid = new int[128, 128];

            Console.ReadLine();
        }
    }

    class KnotHash
    {
        public static string Hash(string input)
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

            // var circularArray = new CircularArray(256);
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

            StringBuilder sb = new StringBuilder();

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

            foreach (var i in denseHash)
            {
                sb.AppendFormat("{0:x2}", i);
            }

            return sb.ToString();
        }
    }
}
