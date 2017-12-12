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
            // PartOne(File.ReadAllLines(args[0]));

            //PartTwo(new string[] {
            //    "0 <-> 2",
            //    "1 <-> 1",
            //    "2 <-> 0, 3, 4",
            //    "3 <-> 2, 4",
            //    "4 <-> 2, 3, 6",
            //    "5 <-> 6",
            //    "6 <-> 4, 5"
            //});
            PartTwo(File.ReadAllLines(args[0]));

            Console.ReadLine();
        }

        static void PartOne(string[] lines)
        {
            List<Prog> fullList = new List<Prog>();

            foreach(var line in lines)
            {
                fullList.Add(new Prog(line));
            }

            fullList[0].Visit(fullList, 0);

            var visitedCount = fullList.Count(p => p.Visited);
            Console.WriteLine("There are " + visitedCount + " programs in the group.");
        }

        static void PartTwo(string[] lines)
        {
            List<Prog> fullList = new List<Prog>();

            foreach(var line in lines)
            {
                fullList.Add(new Prog(line));
            }

            while (fullList.Any(p => !p.Visited))
            {
                var firstUnvisited = fullList.First(p => !p.Visited);
                firstUnvisited.Visit(fullList, firstUnvisited.Id);
            }

            var groupCount = fullList.Select(p => p.GroupId).Distinct().Count();
            Console.WriteLine("There are " + groupCount + " groups in the list.");
        }
    }

    class Prog
    {
        private string desc;
        public int Id { get; set; }
        public bool Visited { get; set; }
        public int GroupId { get; set; }
        public List<int> RelatedIds { get; set; }

        readonly static Regex regex = new Regex(@"^([0-9]+) <-> (.+)$");

        public Prog(string desc)
        {
            this.desc = desc;

            var match = regex.Match(desc);

            Id = Int32.Parse(match.Groups[1].Value); 
            RelatedIds = match.Groups[2].Value.Split(",").Select(c => Int32.Parse(c)).ToList();
        }

        public void Visit(List<Prog> list, int groupId)
        {
            if(Visited)
            {
                return;
            }

            this.Visited = true;
            this.GroupId = groupId;
            foreach(var relatedId in RelatedIds)
            {
                list[relatedId].Visit(list, groupId);
            }
        }

        public override string ToString()
        {
            return desc;
        }
    }
}
