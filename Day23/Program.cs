using System;
using System.IO;

namespace Day23
{
    class Program
    {
        static void Main(string[] args)
        {
            PartOne(File.ReadAllLines("input.txt"));
            // PartTwo(File.ReadAllLines("input.txt"));
            Console.ReadLine();
        }

        static void PartOne(string[] input)
        {
            var register = new Registers();

            register.Read(input, true);
            register.Run();
            Console.WriteLine(register.MulCount);
        }

        static void PartTwo(string[] input)
        {
            var register = new Registers();

            register.Read(input, false);
            register.SetValue('a', 1);
            register.Run();
            Console.WriteLine(register.GetValue('h'));
        }
    }
}
