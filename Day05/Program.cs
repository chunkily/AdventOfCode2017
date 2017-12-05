using System;
using System.Collections.Generic;
using System.IO;

namespace Day05
{
    class Program
    {
        static void Main(string[] args)
        {
            // PartOne(new List<int>() {
            //    0, 3, 0 ,1, -3
            // });
            var input = File.ReadAllLines(args[0]);
            var list = new List<int>();
            foreach (var line in input)
            {
                list.Add(Int32.Parse(line));
            }
            PartOne(list);

            Console.ReadLine();
        }

        static void PartOne(List<int> instructions)
        {
            var steps = 0;
            int currentIndex = 0;
            while (currentIndex >= 0 && currentIndex < instructions.Count)
            {
                steps++;

                int jump = instructions[currentIndex];
                instructions[currentIndex]++;

                currentIndex += jump;
            }

            Console.WriteLine(steps + " steps taken");
        }
    }
}
