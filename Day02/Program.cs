using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace Day02
{
    class Program
    {
        static void Main(string[] args)
        {
            string inputFilePath = args[0];

            var input = File.ReadAllLines(inputFilePath);

            var testInput1 = new[] {
                "5\t1\t9\t5",
                "7\t5\t3",
                "2\t4\t6\t8"
            };

            // PartOne(testInput1);
            PartOne(input);
        }

        static void PartOne(string[] lines)
        {
            int sum = 0;
            foreach(var line in lines)
            {
                var numbersStrings = line.Split("\t");
                List<int> numbers = new List<int>();
                foreach(var numberString in numbersStrings)
                {
                    int number = Int32.Parse(numberString);
                    numbers.Add(number);
                }
                var max = numbers.Max();
                var min = numbers.Min();
                sum += max - min;
            }

            Console.WriteLine(sum);
            Console.ReadLine();
        }
    }
}
