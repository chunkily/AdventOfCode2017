using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Day18
{
    class Registers2
    {
        Dictionary<char, long> registers = new Dictionary<char, long>();

        List<Action> instructions = new List<Action>();

        private const int initialValue = 0;
        private int currentLine = 0;

        public bool Stalled { get; set; }
        public Queue<long> Input { get; }
        public Queue<long> Output { get; }
        public int SendCount { get; set; }

        private static readonly Regex regex = new Regex(@"^([a-z]{3}) (-?[a-z0-9]+) ?(-?[a-z0-9]+)?$");
        private static readonly Regex isChar = new Regex(@"[a-z]");

        public Registers2(Queue<long> input, Queue<long> output, long programNumber)
        {
            Stalled = false;
            SendCount = 0;
            Input = input;
            Output = output;
            SetValue('p', programNumber);
        }

        public void Read(string[] input)
        {
            foreach (var line in input)
            {
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
                            instructions.Add(() => Send(xChar));
                        }
                        else
                        {
                            instructions.Add(() => Send(xNum));
                        }
                        break;
                    case "set":
                        if (yIsChar)
                        {
                            instructions.Add(() => SetValue(xChar, yChar));
                        }
                        else
                        {
                            instructions.Add(() => SetValue(xChar, yNum));
                        }
                        break;
                    case "add":
                        if (yIsChar)
                        {
                            instructions.Add(() => AddValue(xChar, yChar));
                        }
                        else
                        {
                            instructions.Add(() => AddValue(xChar, yNum));
                        }
                        break;
                    case "mul":
                        if (yIsChar)
                        {
                            instructions.Add(() => MultiplyValue(xChar, yChar));
                        }
                        else
                        {
                            instructions.Add(() => MultiplyValue(xChar, yNum));
                        }
                        break;
                    case "mod":
                        if (yIsChar)
                        {
                            instructions.Add(() => ModValue(xChar, yChar));
                        }
                        else
                        {
                            instructions.Add(() => ModValue(xChar, yNum));
                        }
                        break;
                    case "rcv":
                        instructions.Add(() => Receive(xChar));
                        break;
                    case "jgz":
                        if (yIsChar)
                        {
                            if (xIsChar)
                            {
                                instructions.Add(() => JumpGZ(xChar, yChar));
                            }
                            else
                            {
                                instructions.Add(() => JumpGZ(xNum, yChar));
                            }
                        }
                        else
                        {
                            if (xIsChar)
                            {
                                instructions.Add(() => JumpGZ(xChar, yNum));
                            }
                            else
                            {
                                instructions.Add(() => JumpGZ(xNum, yNum));
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
            instructions[currentLine++]();
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

        public void Send(long x)
        {
            Output.Enqueue(x);
            SendCount++;
        }

        public void Send(char x)
        {
            Send(GetValue(x));
        }

        public void SetValue(char x, long y)
        {
            registers[x] = y;
        }

        public void SetValue(char x, char y)
        {
            SetValue(x, GetValue(y));
        }

        public void AddValue(char x, long y)
        {
            SetValue(x, checked(GetValue(x) + y));
        }

        public void AddValue(char x, char y)
        {
            AddValue(x, GetValue(y));
        }

        public void MultiplyValue(char x, long y)
        {
            SetValue(x, checked(GetValue(x) * y));
        }

        public void MultiplyValue(char x, char y)
        {
            MultiplyValue(x, GetValue(y));
        }

        public void ModValue(char x, long y)
        {
            SetValue(x, GetValue(x) % y);
        }

        public void ModValue(char x, char y)
        {
            ModValue(x, GetValue(y));
        }

        public void Receive(char x)
        {
            if (Input.Count == 0)
            {
                Stalled = true;
                currentLine--;
            }
            else
            {
                Stalled = false;
                SetValue(x, Input.Dequeue());
            }
        }

        public void JumpGZ(long x, long y)
        {
            if (x > 0)
            {
                currentLine = currentLine + (int)y - 1;
            }
        }

        public void JumpGZ(char x, long y)
        {
            JumpGZ(GetValue(x), y);
        }

        public void JumpGZ(long x, char y)
        {
            JumpGZ(x, GetValue(y));
        }

        public void JumpGZ(char x, char y)
        {
            JumpGZ(GetValue(x), GetValue(y));
        }
    }
}
