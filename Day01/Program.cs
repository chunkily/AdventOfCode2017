using System;
using System.IO;

namespace Day01
{
    class Program
    {
        static void Main(string[] args)
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
    }
}
