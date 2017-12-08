using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Day08
{
    class Program
    {
        static void Main(string[] args)
        {
            var testLines = new string[] 
            {
                "b inc 5 if a > 1",
                "a inc 1 if b < 5",
                "c dec -10 if a >= 1",
                "c inc -20 if c == 10"
            };

            var lines = File.ReadAllLines(args[0]);

            // PartOne(testLines);
            PartOne(lines);

            Console.ReadLine();
        }

        static void PartOne(string[] instructions)
        {
            Registers registers = new Registers();

            Regex regex = new Regex(@"(.+) (inc|dec) (-?[0-9]+) if (.+) (>|>=|<|<=|==|!=) (-?[0-9]+)");

            foreach (var instruction in instructions)
            {
                var match = regex.Match(instruction);

                string reg1 = match.Groups[1].Value;
                bool inc = match.Groups[2].Value == "inc";
                int value1 = Int32.Parse(match.Groups[3].Value);
                string reg2 = match.Groups[4].Value;
                string op = match.Groups[5].Value;
                int value2 = Int32.Parse(match.Groups[6].Value);

                bool _true = false;
                switch(op)
                {
                    case ">":
                        _true = registers[reg2] > value2;
                        break;
                    case ">=":
                        _true = registers[reg2] >= value2;
                        break;
                    case "<":
                        _true = registers[reg2] < value2;
                        break;
                    case "<=":
                        _true = registers[reg2] <= value2;
                        break;
                    case "==":
                        _true = registers[reg2] == value2;
                        break;
                    case "!=":
                        _true = registers[reg2] != value2;
                        break;
                    default:
                        throw new InvalidOperationException("Unrecognized operator");
                }

                if(_true)
                {
                    
                    if(inc)
                    {
                        registers[reg1] += value1;
                    }
                    else
                    {
                        registers[reg1] -= value1;
                    }
                }
            }

            Console.WriteLine("Current max value: " + registers.MaxCurrentValue());
            Console.WriteLine("Max historical value: " + registers.MaxHistoricalValue());
        }
    }

    class Registers
    {
        Dictionary<string, int> dictionary = new Dictionary<string, int>();
        int maxValue = 0;

        public int this[string register] 
        {
            get
            {
                if (dictionary.TryGetValue(register, out int value))
                {
                    return value;
                }
                return 0;
            }

            set
            {
                dictionary[register] = value;

                if(value > maxValue)
                {
                    maxValue = value;
                }
            }
        }

        public int MaxHistoricalValue()
        {
            return maxValue;
        }

        public int MaxCurrentValue()
        {
            return dictionary.Select(kvp => kvp.Value).Max();
        }
    }
}
