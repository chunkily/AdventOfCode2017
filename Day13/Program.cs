using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace Day13
{
    class Program
    {
        static void Main(string[] args)
        {
            //PartOne(new string[] {
            //    "0: 3",
            //    "1: 2",
            //    "4: 4",
            //    "6: 4",
            //});
            //PartOne(File.ReadAllLines(args[0]));

            //PartTwo(new string[] {
            //    "0: 3",
            //    "1: 2",
            //    "4: 4",
            //    "6: 4",
            //});
            PartTwo(File.ReadAllLines(args[0]));

            Console.ReadLine();
        }

        static void PartOne(string[] input)
        {
            int severity = 0;
            for (int i = 0; i < input.Length; i++)
            {
                var scanner = new Scanner(input[i]);
                if (scanner.DetectsPlayerAt(scanner.Depth))
                {
                    // BEEP INTRUDER DETECTED
                    severity += scanner.Depth * scanner.Range;
                }
            }

            Console.WriteLine("Leaving now has a severity of " + severity);
        }

        static void PartTwo(string[] input)
        {
            List<Scanner> firewall = new List<Scanner>();
            foreach (var line in input)
            {
                firewall.Add(new Scanner(line));
            }

            int delay = 0;
            bool caught = true;
            while (caught)
            {
                delay++;
                caught = false;
                int i = 0;
                while (!caught && i < firewall.Count)
                {
                    var scanner = firewall[i++];
                    caught = scanner.DetectsPlayerAt(scanner.Depth + delay);
                    //if (caught)
                    //{
                    //    Console.WriteLine("Leaving with a delay of " + delay + " would get you caught by scanner " + scanner.ToString());
                    //}
                }
            }
            Console.WriteLine("Leaving with a delay of " + delay + " would let you slip through.");
        }
    }

    class Scanner
    {
        private string desc;
        public int Depth { get; set; }
        public int Range { get; set; }
        public int CycleLength { get; set; }

        private readonly static Regex regex = new Regex(@"^([0-9]+): ([0-9]+)$");

        public Scanner(string desc)
        {
            this.desc = desc;
            var match = regex.Match(desc);
            Depth = Int32.Parse(match.Groups[1].Value);
            Range = Int32.Parse(match.Groups[2].Value);

            CycleLength = (Range - 1) * 2;
        }

        public override string ToString()
        {
            return desc;
        }

        // Deprecate naive solution.
        public int PositionAt(int time)
        {
            int position = 0;

            bool travellingDown = true;
            while (time-- > 0)
            {
                if (travellingDown)
                {
                    if (position + 1 == Range)
                    {
                        travellingDown = false;
                        position--;
                    }
                    else
                    {
                        position++;
                    }
                }
                else
                {
                    if (position == 0)
                    {
                        travellingDown = true;
                        position++;
                    }
                    else
                    {
                        position--;
                    }
                }
            }

            return position;
        }

        public bool DetectsPlayerAt(int time)
        {
            // One cycle length = (Range - 1) * 2
            return time % CycleLength == 0;
        }
    }
}
