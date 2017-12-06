using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace Day06
{
    class Program
    {
        static void Main(string[] args)
        {
            var testList = new int[]
            {
                0,2,7,0
            };

            var list = new int[16];

            var text = File.ReadAllText(args[0]);
            var numStrings = text.Split('\t');
            for (int i = 0; i < list.Length; i++)
            {
                list[i] = Int32.Parse(numStrings[i]);
            }

            // PartOne(testList);
            PartOne(list);

            Console.ReadLine();
        }

        static void PartOne(int[] banks)
        {
            int banksCount = banks.Length;
            int numCycle = 0;
            var comparer = new ListComparer();
            HashSet<int[]> seen = new HashSet<int[]>();

            while (!seen.Contains(banks, comparer))
            {
                numCycle++;

                var clone = new int[banksCount];
                banks.CopyTo(clone, 0);
                seen.Add(clone);

                int indexOfMax = 0;
                int max = 0;
                for (int i = 0; i < banks.Length; i++)
                {
                    if (banks[i] > max)
                    {
                        max = banks[i];
                        indexOfMax = i;
                    }
                }

                banks[indexOfMax] = 0;
                int j = indexOfMax + 1;

                for (int k = 0; k < max; k++)
                {
                    banks[j++ % banksCount]++;
                }
            }

            Console.WriteLine(numCycle + " cycles required");
        }
    }

    public class ListComparer : IEqualityComparer<int[]>
    {
        public bool Equals(int[] x, int[] y)
        {
            // Assume x and y same length
            for (int i = 0; i < x.Length; i++)
            {
                if (x[i] != y[i])
                {
                    return false;
                }
            }
            return true;
        }

        public int GetHashCode(int[] obj)
        {
            throw new NotImplementedException();
        }
    }
}
