using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Day16
{
    class Program
    {
        static void Main(string[] args)
        {
            var testStage = new Stage(5);

            //PartOne(new string[] {
            //    "s1",
            //    "x3/4",
            //    "pe/b"
            //}, testStage);

            var stage = new Stage(16);
            var input = File.ReadAllText(args[0]).Split(',');
            // PartOne(input, stage);

            //PartTwo(new string[] {
            //    "s1",
            //    "x3/4",
            //    "pe/b"
            //}, testStage, 2);
            PartTwo(input, stage, 1_000_000_000);

            Console.ReadLine();
        }

        static void PartOne(string[] instructions, Stage stage)
        {
            foreach (var instruction in instructions)
            {
                stage.ParseInstruction(instruction);
            }

            Console.WriteLine(stage);
        }

        static void PartTwo(string[] instructions, Stage stage, int numTimes)
        {
            string initial = stage.ToString();
            foreach (var instruction in instructions)
            {
                stage.ParseInstruction(instruction);
            }
            string final = stage.ToString();

            for (int i = 1; i < numTimes; i++)
            {
                stage.RepeatDance();

                if (i % 10_000_000 == 0)
                {
                    Console.WriteLine("Working... " + (i / 10_000_000) + "%");
                }
            }
            Console.WriteLine(stage);
        }
    }

    class Stage
    {
        private List<char> dancers;
        int size;

        static Regex SpinRegex = new Regex(@"^s([0-9]+)$");
        static Regex ExchangeRegex = new Regex(@"^x([0-9]+)/([0-9]+)$");
        static Regex PartnerRegex = new Regex(@"^p([a-p]+)/([a-p]+)$");

        Action a;

        public Stage(int size)
        {
            this.size = size;
            dancers = new List<char>(size);
            char dancer = 'a';
            for (int i = 0; i < size; i++)
            {
                dancers.Add(dancer++);
            }
        }

        public void RepeatDance()
        {
            a();
        }

        public void ParseInstruction(string instruction)
        {
            var sMatch = SpinRegex.Match(instruction);
            if(sMatch.Success)
            {
                int val = Int32.Parse(sMatch.Groups[1].Value);
                Spin(val);
                a += () => { Spin(val); };
                return;
            }
            var eMatch = ExchangeRegex.Match(instruction);
            if (eMatch.Success)
            {
                int pos1 = Int32.Parse(eMatch.Groups[1].Value);
                int pos2 = Int32.Parse(eMatch.Groups[2].Value);
                Exchange(pos1, pos2);
                a += () => { Exchange(pos1, pos2); };
                return;
            }
            var pMatch = PartnerRegex.Match(instruction);
            if(pMatch.Success)
            {
                char c1 = pMatch.Groups[1].Value[0];
                char c2 = pMatch.Groups[2].Value[0];
                Partner(c1, c2);
                a += () => { Partner(c1, c2); };
                return;
            }
            throw new Exception("Unhandled instruction " + instruction);
        }

        public void Spin(int num)
        {
            char[] copy = new char[size];
            dancers.CopyTo(copy, 0);
            for (int i = 0; i < dancers.Count; i++)
            {
                dancers[(i + num) % size] = copy[i];
            }
        }

        public void Exchange(int pos1, int pos2)
        {
            var temp = dancers[pos1];
            dancers[pos1] = dancers[pos2];
            dancers[pos2] = temp;
        }

        public void Partner(char dancer1, char dancer2)
        {
            var pos1 = dancers.IndexOf(dancer1);
            var pos2 = dancers.IndexOf(dancer2);
            Exchange(pos1, pos2);
        }

        public override string ToString()
        {
            return new String(dancers.ToArray());
        }
    }
}
