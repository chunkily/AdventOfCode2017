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
        Dictionary<char, long> registers = new Dictionary<char, long>();

        List<Func<long?>> instructions = new List<Func<long?>>();

        private const int initialValue = 0;
        private long lastPlayed = 0;
        private int currentLine = 0;

        private static readonly Regex regex = new Regex(@"^([a-z]{3}) (-?[a-z0-9]+) ?(-?[a-z0-9]+)?$");
        private static readonly Regex isChar = new Regex(@"[a-z]");

        public void Read(string[] input)
        {
            foreach (var line in input)
            {
                // Console.Write(line + ":");
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
                            //Console.WriteLine($"snd {xChar}");
                            instructions.Add(() => PlaySound(xChar));
                        }
                        else
                        {
                            //Console.WriteLine($"snd {xNum}");
                            instructions.Add(() => PlaySound(xNum));
                        }
                        break;
                    case "set":
                        if (yIsChar)
                        {
                            //Console.WriteLine($"set {xChar} {yChar}");
                            instructions.Add(() => SetValue(xChar, yChar));
                        }
                        else
                        {
                            //Console.WriteLine($"set {xChar} {yNum}");
                            instructions.Add(() => SetValue(xChar, yNum));
                        }
                        break;
                    case "add":
                        if (yIsChar)
                        {
                            //Console.WriteLine($"add {xChar} {yChar}");
                            instructions.Add(() => AddValue(xChar, yChar));
                        }
                        else
                        {
                            //Console.WriteLine($"add {xChar} {yNum}");
                            instructions.Add(() => AddValue(xChar, yNum));
                        }
                        break;
                    case "mul":
                        if (yIsChar)
                        {
                            //Console.WriteLine($"mul {xChar} {yChar}");
                            instructions.Add(() => MultiplyValue(xChar, yChar));
                        }
                        else
                        {
                            //Console.WriteLine($"mul {xChar} {yNum}");
                            instructions.Add(() => MultiplyValue(xChar, yNum));
                        }
                        break;
                    case "mod":
                        if (yIsChar)
                        {
                            //Console.WriteLine($"mod {xChar} {yChar}");
                            instructions.Add(() => ModValue(xChar, yChar));
                        }
                        else
                        {
                            //Console.WriteLine($"mod {xChar} {yNum}");
                            instructions.Add(() => ModValue(xChar, yNum));
                        }
                        break;
                    case "rcv":
                        if (xIsChar)
                        {
                            //Console.WriteLine($"rcv {xChar}");
                            instructions.Add(() => Recover(xChar));
                        }
                        else
                        {
                            //Console.WriteLine($"rcv {xNum}");
                            instructions.Add(() => Recover(xNum));
                        }
                        break;
                    case "jgz":
                        if (yIsChar)
                        {
                            if (xIsChar)
                            {
                                //Console.WriteLine($"jgz {xChar} {yChar}");
                                instructions.Add(() =>
                                {
                                    return JumpGZ(xChar, yChar);
                                });
                            }
                            else
                            {
                                //Console.WriteLine($"jgz {xNum} {yChar}");
                                instructions.Add(() =>
                                {
                                    return JumpGZ(xNum, yChar);
                                });
                            }
                        }
                        else
                        {
                            if (xIsChar)
                            {
                                //Console.WriteLine($"jgz {xChar} {yNum}");
                                instructions.Add(() =>
                                {
                                    return JumpGZ(xChar, yNum);
                                });
                            }
                            else
                            {
                                //Console.WriteLine($"jgz {xNum} {yNum}");
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
            long? rec = null;
            while (rec == null)
            {
                rec = instructions[currentLine++]();
            }
            Console.WriteLine(rec);
        }
        public long GetValue(char x)
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

        public int? PlaySound(char x)
        {
            return PlaySound(GetValue(x));
        }

        public int? PlaySound(long x)
        {
            lastPlayed = x;
            return null;
        }

        public int? SetValue(char x, long y)
        {
            registers[x] = y;
            return null;
        }

        public int? SetValue(char x, char y)
        {
            return SetValue(x, GetValue(y));
        }

        public int? AddValue(char x, long y)
        {
            return SetValue(x, checked(GetValue(x) + y));
        }

        public int? AddValue(char x, char y)
        {
            return AddValue(x, GetValue(y));
        }

        public int? MultiplyValue(char x, long y)
        {
            return SetValue(x, checked(GetValue(x) * y));
        }

        public int? MultiplyValue(char x, char y)
        {
            return MultiplyValue(x, GetValue(y));
        }

        public int? ModValue(char x, long y)
        {
            return SetValue(x, GetValue(x) % y);
        }

        public long? ModValue(char x, char y)
        {
            return ModValue(x, GetValue(y));
        }

        public long? Recover(long x)
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

        public long? Recover(char x)
        {
            return Recover(GetValue(x));
        }

        public long? JumpGZ(long x, long y)
        {
            if (x > 0)
            {
                if (y > Int32.MaxValue)
                {
                    throw new InvalidOperationException("y is larger than integer");
                }

                checked
                {
                    currentLine = currentLine + (int)y - 1;
                }
            }

            return null;
        }

        public long? JumpGZ(char x, long y)
        {
            return JumpGZ(GetValue(x), y);
        }

        public long? JumpGZ(long x, char y)
        {
            return JumpGZ(x, GetValue(y));
        }

        public long? JumpGZ(char x, char y)
        {
            return JumpGZ(GetValue(x), GetValue(y));
        }
    }
}
