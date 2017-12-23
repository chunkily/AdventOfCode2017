using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Day23
{
    class Registers
    {
        Dictionary<char, long> registers = new Dictionary<char, long>();

        List<Action> instructions = new List<Action>();

        private const int initialValue = 0;
        private int currentLine = 0;
        private int instructionsCount;
        private int mulCount = 0;
        private static readonly Regex regex = new Regex(@"^([a-z]{3}) (-?[a-z0-9]+) ?(-?[a-z0-9]+)?$");
        private static readonly Regex isChar = new Regex(@"[a-z]");

        public int MulCount { get => mulCount; }

        public void Read(string[] input, bool mulCount)
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
                    case "set":
                        if (yIsChar)
                        {
                            instructions.Add(() => SetValue(xChar, GetValue(yChar)));
                        }
                        else
                        {
                            instructions.Add(() => SetValue(xChar, yNum));
                        }
                        break;
                    case "sub":
                        if (yIsChar)
                        {
                            instructions.Add(() => SubValue(xChar, GetValue(yChar)));
                        }
                        else
                        {
                            instructions.Add(() => SubValue(xChar, yNum));
                        }
                        break;
                    case "mul":
                        if(mulCount)
                        {
                            if (yIsChar)
                            {
                                instructions.Add(() => MultiplyValue(xChar, GetValue(yChar)));
                            }
                            else
                            {
                                instructions.Add(() => MultiplyValue(xChar, yNum));
                            }
                        }
                        else
                        {
                            if (yIsChar)
                            {
                                instructions.Add(() => MultiplyValueOpt(xChar, GetValue(yChar)));
                            }
                            else
                            {
                                instructions.Add(() => MultiplyValueOpt(xChar, yNum));
                            }
                        }
                        break;
                    case "jnz":
                        if (yIsChar)
                        {
                            if (xIsChar)
                            {
                                instructions.Add(() => JumpNZ(GetValue(xChar), GetValue(yChar)));
                            }
                            else
                            {
                                if(xNum != 0)
                                {
                                    instructions.Add(() => Jump(GetValue(yChar)));
                                }
                            }
                        }
                        else
                        {
                            if (xIsChar)
                            {
                                instructions.Add(() => JumpNZ(GetValue(xChar), yNum));
                            }
                            else
                            {
                                if(xNum != 0)
                                {
                                    instructions.Add(() => Jump(yNum));
                                }
                            }
                        }
                        break;
                    default:
                        throw new Exception("Line not handled: " + line);
                }
            }

            instructionsCount = instructions.Count;
        }

        public void Run()
        {
            checked
            {
                while(currentLine >= 0 && currentLine < instructionsCount)
                {
                    instructions[currentLine++]();
                }
            }
        }

        public long GetValue(char x)
        {
            if(registers.TryGetValue(x, out long value))
            {
                return value;
            }
            else
            {
                return initialValue;
            }
        }

        public void SetValue(char x, long y)
        {
            registers[x] = y;
        }

        public void SubValue(char x, long y)
        {
            SetValue(x, checked(GetValue(x) - y));
        }

        public void MultiplyValue(char x, long y)
        {
            mulCount++;
            SetValue(x, checked(GetValue(x) * y));
        }

        public void MultiplyValueOpt(char x, long y)
        {
            SetValue(x, checked(GetValue(x) * y));
        }

        public void JumpNZ(long x, long y)
        {
            if (x != 0)
            {
                currentLine = currentLine + (int)y - 1;
            }
        }

        public void Jump(long y)
        {
            currentLine = currentLine + (int)y - 1;
        }
    }
}
