using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Day24
{
    class Program
    {
        static void Main(string[] args)
        {
            //PartOne(new string[] {
            //    "0/2",
            //    "2/2",
            //    "2/3",
            //    "3/4",
            //    "3/5",
            //    "0/1",
            //    "10/1",
            //    "9/10",
            //});
            PartOne(File.ReadAllLines("input.txt"));
            Console.ReadLine();
        }

        static void PartOne(string[] input)
        {
            List<Component> components = new List<Component>();

            for (int i = 0; i < input.Length; i++)
            {
                components.Add(new Component(input[i], i));
            }

            List<List<Bridge>> bridges = new List<List<Bridge>>()
            {
                new List<Bridge>()
                {
                    new Bridge(new List<Component>(), components, 0)
                }
            };

            List<Bridge> nextList;
            int count = 0;
            do
            {
                nextList = new List<Bridge>();
                foreach(var bridge in bridges[count++])
                {
                    nextList.AddRange(bridge.GetBranches());
                }
                if(nextList.Count > 0)
                {
                    bridges.Add(nextList);
                }
            }
            while (nextList.Count > 0);

            var allBridges = bridges.SelectMany(bs => bs);
            int max = allBridges.Max(b => b.GetTotalStrength());

            var longestBridges = bridges.Last();
            int max2 = longestBridges.Max(b => b.GetTotalStrength());

            Console.WriteLine("Strongest bridge has strength of: " + max);
            Console.WriteLine("Longest, strongest bridge has strength of: " + max2);
        }
    }

    class Bridge
    {
        List<Component> usedComponents = new List<Component>();
        List<Component> remainingComponents = new List<Component>();
        int EndPort = 0;

        public Bridge(List<Component> usedComponents, List<Component> remainingComponents, int endPort)
        {
            this.usedComponents = usedComponents;
            this.remainingComponents = remainingComponents;
            this.EndPort = endPort;
        }

        public bool TryAttach(Component c)
        {
            if(c.A == EndPort)
            {
                usedComponents.Add(c);
                EndPort = c.B;
                remainingComponents.Remove(c);
                return true;
            }
            else if(c.B == EndPort)
            {
                usedComponents.Add(c);
                EndPort = c.A;
                remainingComponents.Remove(c);
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<Bridge> GetBranches()
        {
            List<Bridge> nextList = new List<Bridge>();
            foreach(var component in remainingComponents)
            {
                var next = new Bridge(
                    usedComponents.ToList(),
                    remainingComponents.ToList(),
                    EndPort);

                if(next.TryAttach(component))
                {
                    nextList.Add(next);
                }
            }
            return nextList;
        }

        public int GetTotalStrength()
        {
            return usedComponents.Sum(c => c.Strength);
        }

        public override string ToString()
        {
            return String.Join(',', usedComponents.Select(c => c.Id));
        }
    }

    struct Component
    {
        public int Id { get; set; }
        public int A { get; set; }
        public int B { get; set; }

        public int Strength
        {
            get
            {
                return A + B;
            }
        }

        static readonly Regex regex = new Regex(@"^([0-9]+)/([0-9]+)$");

        public Component(string s, int id)
        {
            var match = regex.Match(s);
            A = Int32.Parse(match.Groups[1].Value);
            B = Int32.Parse(match.Groups[2].Value);
            Id = id;
        }
    }
}
