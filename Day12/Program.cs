using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Day12
{
    class Program
    {
        static void Main(string[] args)
        {
            //PartOne(new string[] {
            //    "0 <-> 2",
            //    "1 <-> 1",
            //    "2 <-> 0, 3, 4",
            //    "3 <-> 2, 4",
            //    "4 <-> 2, 3, 6",
            //    "5 <-> 6",
            //    "6 <-> 4, 5"
            //});
            PartOne(File.ReadAllLines(args[0]));

            Console.ReadLine();
        }

        static void PartOne(string[] lines)
        {
            List<Prog> fullList = new List<Prog>();

            foreach(var line in lines)
            {
                fullList.Add(new Prog(line));
            }

            fullList[0].Visit(fullList);

            var visitedCount = fullList.Count(p => p.Visited);
            Console.WriteLine("There are " + visitedCount + " programs in the group.");
        }
    }

    class Prog
    {
        private string desc;
        public bool Visited { get; set; }
        public List<int> RelatedIds { get; set; }

        readonly static Regex regex = new Regex(@"^([0-9]+) <-> (.+)$");

        public Prog(string desc)
        {
            this.desc = desc;

            var match = regex.Match(desc);

            // Id should be equal to the list index...
            // Id = Int32.Parse(match.Groups[1].Value); 
            RelatedIds = match.Groups[2].Value.Split(",").Select(c => Int32.Parse(c)).ToList();
        }

        public void Visit(List<Prog> list)
        {
            if(Visited)
            {
                return;
            }

            this.Visited = true;
            foreach(var relatedId in RelatedIds)
            {
                list[relatedId].Visit(list);
            }
        }

        public override string ToString()
        {
            return desc;
        }
    }
}
