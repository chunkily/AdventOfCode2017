using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Day07
{
    class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines(args[0]).ToList();
            var testLines = new List<string>()
            {
                "pbga (66)",
                "xhth (57)",
                "ebii (61)",
                "havc (66)",
                "ktlj (57)",
                "fwft (72) -> ktlj, cntj, xhth",
                "qoyq (66)",
                "padx (45) -> pbga, havc, qoyq",
                "tknk (41) -> ugml, padx, fwft",
                "jptl (61)",
                "ugml (68) -> gyxo, ebii, jptl",
                "gyxo (61)",
                "cntj (57)",
            };

            // PartOne(testLines);
            PartOne(lines);
            Console.ReadLine();

        }

        static void PartOne(List<string> lines)
        {
            var branches = new Queue<Prog>();
            var leaves = new List<Prog>();

            // Parse and construct queue
            foreach (var line in lines)
            {
                var prog = new Prog(line);
                if (prog.HasDisc)
                {
                    branches.Enqueue(prog);
                }
                else
                {
                    leaves.Add(prog);
                }
            }

            // Construct tree from the branches
            Prog root = branches.Dequeue();
            while (branches.Count > 0)
            {
                var next = branches.Dequeue();
                // Check if it is supporting the root

                if (next.Disc.Contains(root.Name))
                {
                    // This is the new root
                    var oldRoot = root;
                    root = next;
                    root.DiscProgs.Add(oldRoot);
                }
                else
                {
                    // Try to plant it on another branch
                    var tip = root.FindTip(next.Name);
                    if (tip != null)
                    {
                        tip.DiscProgs.Add(next);
                    }
                    else
                    {
                        // Tip not found, put back into queue at the end
                        branches.Enqueue(next);
                    }
                }
            }

            // Attach the leaves

            Console.WriteLine(root.Name + " is the root node");
        }
    }

    class Prog
    {
        private string desc;
        public string Name { get; set; }
        public int Weight { get; set; }

        public string[] Disc { get; set; }
        public List<Prog> DiscProgs { get; set; }

        public bool HasDisc
        {
            get
            {
                return Disc != null;
            }
        }

        public Prog(string description)
        {
            desc = description;

            Regex regex = new Regex(@"^([a-z]+) \(([0-9]+)\)( -> )?(.+)?$");
            var match = regex.Match(description);

            Name = match.Groups[1].Value;
            Weight = Int32.Parse(match.Groups[2].Value);
            if (match.Groups[4].Success)
            {
                Disc = match.Groups[4].Value.Replace(" ", "").Split(',');
            }

            DiscProgs = new List<Prog>();
        }

        public Prog FindTip(string name)
        {
            if (HasDisc)
            {
                foreach (var progName in Disc)
                {
                    if (progName == name)
                    {
                        return this;
                    }
                }

            }

            if (DiscProgs.Count > 0)
            {
                foreach (var prog in DiscProgs)
                {
                    var p = prog.FindTip(name);
                    if (p != null)
                    {
                        return p;
                    }
                }
            }

            return null;
        }

        public override string ToString()
        {
            return desc;
        }
    }
}
