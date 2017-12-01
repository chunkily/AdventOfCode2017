using System;
using System.IO;

namespace Day01
{
    class Program
    {
        static void Main(string[] args)
        {
            PartOne(args);
            // PartTwo(args);
        }

        static void PartOne(string[] args)
        {
            string filepath = args[0];

            int sum = 0;

            using (StreamReader reader = new StreamReader(filepath))
            {
                char first = (char)reader.Peek();
                char c = '0';
                char n;

                while (!reader.EndOfStream)
                {
                    c = (char)reader.Read();
                    n = (char)reader.Peek();
                    if (c == n)
                    {
                        sum += Int32.Parse(c.ToString());
                    }
                }

                if (c == first)
                {
                    sum += Int32.Parse(c.ToString());
                }
            }

            Console.WriteLine(sum);
            Console.ReadLine();

        }

        static void PartTwo(string[] args)
        {
            var input = File.ReadAllText(args[0]);

            int offset = input.Length / 2;

            int sum = 0;
            for (int i = 0; i < offset; i++)
            {
                char c = input[i];
                char next = input[i + offset];

                if (c == next)
                {
                    sum += Int32.Parse(c.ToString()) * 2;
                }
            }

            Console.WriteLine(sum);
            Console.ReadLine();
        }
    }
}
