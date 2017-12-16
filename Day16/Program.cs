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
            stage.RepeatDance();
            Console.WriteLine(stage);
        }

        static void PartTwo(string[] instructions, Stage stage, int numTimes)
        {
            foreach (var instruction in instructions)
            {
                stage.ParseInstruction(instruction);
            }

            string initial = stage.ToString();
            stage.RepeatDance();
            int numCycles = 1;

            while(stage.ToString() != initial)
            {
                stage.RepeatDance();
                numCycles++;
            }

            Console.WriteLine("The dance repeats itself every " + numCycles + " cycles");

            int numRepeat = numTimes % numCycles;
            for (int i = 0; i < numRepeat; i++)
            {
                stage.RepeatDance();
            }

            Console.WriteLine("The dancers will be in the following order after " + numTimes + " cycles");
            Console.WriteLine(stage);
        }
    }

    class Stage
    {
        private char[] dancers;
        int size;

        static Regex SpinRegex = new Regex(@"^s([0-9]+)$");
        static Regex ExchangeRegex = new Regex(@"^x([0-9]+)/([0-9]+)$");
        static Regex PartnerRegex = new Regex(@"^p([a-p]+)/([a-p]+)$");

        List<Action> moves;

        public Stage(int size)
        {
            this.size = size;
            dancers = new char[size];
            char dancer = 'a';
            for (int i = 0; i < size; i++)
            {
                dancers[i] = dancer++;
            }
            moves = new List<Action>();
        }

        public void RepeatDance()
        {
            foreach (var move in moves)
            {
                move();
            }
        }

        public void ParseInstruction(string instruction)
        {
            var sMatch = SpinRegex.Match(instruction);
            if (sMatch.Success)
            {
                int val = Int32.Parse(sMatch.Groups[1].Value);
                moves.Add(() => { Spin(val); });
                return;
            }
            var eMatch = ExchangeRegex.Match(instruction);
            if (eMatch.Success)
            {
                int pos1 = Int32.Parse(eMatch.Groups[1].Value);
                int pos2 = Int32.Parse(eMatch.Groups[2].Value);
                moves.Add(() => { Exchange(pos1, pos2); });
                return;
            }
            var pMatch = PartnerRegex.Match(instruction);
            if (pMatch.Success)
            {
                char c1 = pMatch.Groups[1].Value[0];
                char c2 = pMatch.Groups[2].Value[0];
                moves.Add(() => { Partner(c1, c2); });
                return;
            }
            throw new Exception("Unhandled instruction " + instruction);
        }

        public void Spin(int num)
        {
            char[] copy = new char[size];
            dancers.CopyTo(copy, 0);
            for (int i = 0; i < dancers.Length; i++)
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
            int pos1 = -1;
            int pos2 = -1;
            for (int i = 0; pos1 < 0 || pos2 < 0; i++)
            {
                var c = dancers[i];
                if (c == dancer1)
                {
                    pos1 = i;
                }
                else if (c == dancer2)
                {
                    pos2 = i;
                }
            }
            Exchange(pos1, pos2);
        }

        public override string ToString()
        {
            return new String(dancers);
        }
    }
}
