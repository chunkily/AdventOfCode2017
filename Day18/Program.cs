using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace Day18
{
    class Program
    {
        static void Main(string[] args)
        {
            //PartOne(new string[] {
            //    "set a 1",
            //    "add a 2",
            //    "mul a a",
            //    "mod a 5",
            //    "snd a",
            //    "set a 0",
            //    "rcv a",
            //    "jgz a -1",
            //    "set a 1",
            //    "jgz a -2"
            //});
            PartOne(File.ReadAllLines("input.txt"));
            Console.ReadLine();
        }

        static void PartOne(string[] input)
        {
            var registers = new Registers();
            registers.Read(input);
            registers.Run();
        }
    }

    class Registers
    {
        Dictionary<char, int> registers = new Dictionary<char, int>();

        List<Func<int?>> instructions = new List<Func<int?>>();

        private const int initialValue = 0;
        private int lastPlayed = 0;
        private int currentLine = 0;

        private static readonly Regex regex = new Regex(@"^([a-z]{3}) (-?[a-z0-9]+) ?(-?[a-z0-9]+)?$");
        private static readonly Regex isChar = new Regex(@"[a-z]");

        public void Read(string[] input)
        {
            foreach (var line in input)
            {
                Console.Write(line + ":");
                var match = regex.Match(line);

                string opr = match.Groups[1].Value;

                string x = match.Groups[2].Value;

                bool xIsChar = false;
                int xNum = 0;
                char xChar = ' ';

                if (isChar.IsMatch(x))
                {
                    xIsChar = true;
                    xChar = x[0];
                }
                else
                {
                    xIsChar = false;
                    xNum = Int32.Parse(x);
                }

                bool yIsChar = false;
                int yNum = 0;
                char yChar = ' ';

                if (match.Groups[3].Success)
                {
                    string y = match.Groups[3].Value;
                    if (isChar.IsMatch(y))
                    {
                        yChar = y[0];
                        yIsChar = true;
                    }
                    else
                    {
                        yNum = Int32.Parse(y);
                        yIsChar = false;
                    }
                }

                switch (opr)
                {
                    case "snd":
                        if (xIsChar)
                        {
                            Console.WriteLine($"snd {xChar}");
                            instructions.Add(() => PlaySound(GetValue(xChar)));
                        }
                        else
                        {
                            Console.WriteLine($"snd {xNum}");
                            instructions.Add(() => PlaySound(xNum));
                        }
                        break;
                    case "set":
                        if (yIsChar)
                        {
                            Console.WriteLine($"set {xChar} {yChar}");
                            instructions.Add(() => SetValue(xChar, GetValue(yChar)));
                        }
                        else
                        {
                            Console.WriteLine($"set {xChar} {yNum}");
                            instructions.Add(() => SetValue(xChar, yNum));
                        }
                        break;
                    case "add":
                        if (yIsChar)
                        {
                            Console.WriteLine($"add {xChar} {yChar}");
                            instructions.Add(() => AddValue(xChar, GetValue(yChar)));
                        }
                        else
                        {
                            Console.WriteLine($"add {xChar} {yNum}");
                            instructions.Add(() => AddValue(xChar, yNum));
                        }
                        break;
                    case "mul":
                        if (yIsChar)
                        {
                            Console.WriteLine($"mul {xChar} {yChar}");
                            instructions.Add(() => MultiplyValue(xChar, GetValue(yChar)));
                        }
                        else
                        {
                            Console.WriteLine($"mul {xChar} {yNum}");
                            instructions.Add(() => MultiplyValue(xChar, yNum));
                        }
                        break;
                    case "mod":
                        if (yIsChar)
                        {
                            Console.WriteLine($"mod {xChar} {yChar}");
                            instructions.Add(() => ModValue(xChar, GetValue(yChar)));
                        }
                        else
                        {
                            Console.WriteLine($"mod {xChar} {yNum}");
                            instructions.Add(() => ModValue(xChar, yNum));
                        }
                        break;
                    case "rcv":
                        if (xIsChar)
                        {
                            Console.WriteLine($"rcv {xChar}");
                            instructions.Add(() => Recover(GetValue(xChar)));
                        }
                        else
                        {
                            Console.WriteLine($"rcv {xNum}");
                            instructions.Add(() => Recover(xNum));
                        }
                        break;
                    case "jgz":
                        if (yIsChar)
                        {
                            if (xIsChar)
                            {
                                Console.WriteLine($"jgz {xChar} {yChar}");
                                instructions.Add(() =>
                                {
                                    return JumpGZ(GetValue(xChar), GetValue(yChar));
                                });
                            }
                            else
                            {
                                Console.WriteLine($"jgz {xNum} {yChar}");
                                instructions.Add(() =>
                                {
                                    return JumpGZ(xNum, GetValue(yChar));
                                });
                            }
                        }
                        else
                        {
                            if (xIsChar)
                            {
                                Console.WriteLine($"jgz {xChar} {yNum}");
                                instructions.Add(() =>
                                {
                                    return JumpGZ(GetValue(xChar), yNum);
                                });
                            }
                            else
                            {
                                Console.WriteLine($"jgz {xNum} {yNum}");
                                instructions.Add(() =>
                                {
                                    return JumpGZ(xNum, yNum);
                                });
                            }
                        }
                        break;
                    default:
                        throw new Exception("Line not handled: " + line);
                }
            }
        }

        public void Run()
        {
            int? rec = null;
            while (rec == null)
            {
                rec = instructions[currentLine++]();
            }
            Console.WriteLine(rec);
        }
        public int GetValue(char x)
        {
            if (registers.ContainsKey(x))
            {
                return registers[x];
            }
            else
            {
                return initialValue;
            }
        }

        public int? PlaySound(int x)
        {
            lastPlayed = x;
            return null;
        }

        public int? SetValue(char x, int y)
        {
            registers[x] = y;
            return null;
        }

        public int? AddValue(char x, int y)
        {
            int v = GetValue(x);
            return SetValue(x, v + y);
        }

        public int? MultiplyValue(char x, int y)
        {
            int v = GetValue(x);
            return SetValue(x, v * y);
        }

        public int? ModValue(char x, int y)
        {
            int v = GetValue(x);
            return SetValue(x, v % y);
        }

        public int? Recover(int x)
        {
            if (x != 0)
            {
                return lastPlayed;
            }
            else
            {
                return null;
            }
        }

        public int? JumpGZ(int x, int y)
        {
            if (x > 0)
            {
                currentLine = currentLine + y - 1;
            }

            return null;
        }
    }
}
