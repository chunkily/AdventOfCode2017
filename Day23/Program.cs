using System;
using System.IO;

namespace Day23
{
    class Program
    {
        static void Main(string[] args)
        {
            // PartOne(File.ReadAllLines("input.txt"));
            // PartTwoBruteForce(File.ReadAllLines("input.txt"));
            PartTwo();
            Console.ReadLine();
        }

        static void PartOne(string[] input)
        {
            var register = new Registers();

            register.Read(input, true);
            register.Run();
            Console.WriteLine(register.MulCount);
        }

        static void PartTwoBruteForce(string[] input)
        {
            var register = new Registers();

            register.Read(input, false);
            register.SetValue('a', 1);
            register.Run();
            Console.WriteLine(register.GetValue('h'));
        }

        static void PartTwo()
        {
            int h = 0;
            for (int b = 107900; b <= 124900 ; b += 17)
            {
                if(!IsPrime(b))
                {
                    h++;
                }
            }
            Console.WriteLine(h);
        }

        // Note this gives incorrect results for inputs smaller than 2, 
        // but we don't care about those right now.
        static bool IsPrime(int num)
        {
            for (int i = 2; i * i <= num; i++)
            {
                if(num % i == 0)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
