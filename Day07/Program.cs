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
                    root.Attach(oldRoot);
                }
                else
                {
                    // Try to plant it on another branch
                    var tip = root.FindTip(next.Name);
                    if (tip != null)
                    {
                        tip.Attach(next);
                    }
                    else
                    {
                        // Tip not found, put back into queue at the end
                        branches.Enqueue(next);
                    }
                }
            }

            // Attach the leaves
            foreach (var leaf in leaves)
            {
                var tip = root.FindTip(leaf.Name);
                tip.Attach(leaf);
            }

            Console.WriteLine(root.Name + " is the root node");
            root.ValidateWeights();
        }
    }

    class Prog
    {
        private string desc;
        public string Name { get; set; }
        public int Weight { get; set; }
        public int TotalWeight { get; set; }

        public string[] Disc { get; set; }
        public List<Prog> DiscProgs { get; set; }
        public Prog Parent { get; set; }

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
            TotalWeight = Weight;
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

        public void Attach(Prog prog)
        {
            DiscProgs.Add(prog);
            prog.Parent = this;
            AddToTotalWeight(prog.TotalWeight);
        }

        public void AddToTotalWeight(int weight)
        {
            TotalWeight += weight;
            if (Parent != null)
            {
                Parent.AddToTotalWeight(weight);
            }

        }

        public bool HasBalancedDisc()
        {
            return this.HasDisc && this.DiscProgs.Select(p => p.TotalWeight).Distinct().Count() == 1;
        }

        public void ValidateWeights()
        {
            // Root will always have unbalanced weights.
            // We need to find the single odd one out and drill down into it
            // Until we get to a program that has completely balanced weights.
            // That program will be the one that requires adjusting

            if(this.HasBalancedDisc())
            {
                Console.WriteLine(Name + " has a balanced disk but it's parent has an unbalanced disk");

                var otherTotal = Parent.DiscProgs.First(p => p.Name != Name).TotalWeight;

                Console.WriteLine("You have to alter the weight of "+ Name + " to " + (otherTotal - TotalWeight + Weight )+ " to attain balance.");
            }
            else
            {
                // Drill down into the odd one out!
                // ASSUME THAT WE WILL NEVER GET AN UNBALANCED DISC WITH LESS THAN 3 PROGRAMS
                var first = DiscProgs[0].TotalWeight;
                var second = DiscProgs[1].TotalWeight;
                var third = DiscProgs[2].TotalWeight;

                if (second == third && first != second)
                {
                    // First is odd one out
                    DiscProgs[0].ValidateWeights();
                }
                else if(first == third && second != third)
                {
                    // Second is odd one
                    DiscProgs[1].ValidateWeights();
                }
                else if(first == second && second != third)
                {
                    // third is odd one
                    DiscProgs[2].ValidateWeights();
                }
                else
                {
                    // First 3 are the same
                    for (int i = 3; i < DiscProgs.Count; i++)
                    {
                        if(DiscProgs[i].TotalWeight != first)
                        {
                            DiscProgs[i].ValidateWeights();
                        }
                    }
                }
            }
        }

        public override string ToString()
        {
            return desc;
        }

    }
}
