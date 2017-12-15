using System;
using System.Collections;
using System.Threading.Tasks;

namespace Day15
{
    class Program
    {
        static void Main(string[] args)
        {
            // PartOne(65, 8921);
            // PartOne(Int32.Parse(args[0]), Int32.Parse(args[1]));

            // PartTwo(65, 8921).Wait();
            PartTwo(Int32.Parse(args[0]), Int32.Parse(args[1])).Wait();

            Console.ReadLine();
        }

        static void PartOne(int startA, int startB)
        {
            var generatorA = new Generator(16807, startA);
            var generatorB = new Generator(48271, startB);

            var judge = new Judge();

            int sum = 0;
            for (int i = 0; i < 40_000_000; i++)
            {
                var a = generatorA.GetNext();
                var b = generatorB.GetNext();
                if (judge.Equal(a, b))
                {
                    sum++;
                }
            }

            Console.WriteLine("Judge: " + sum + " pairs match");
        }

        static async Task PartTwo(int startA, int startB)
        {
            var generatorA = new Generator(16807, startA);
            var generatorB = new Generator(48271, startB);

            var judge = new Judge();

            int sum = 0;
            for (int i = 0; i < 5_000_000; i++)
            {
                var a = generatorA.GetNextPickyAsync(4);
                var b = generatorB.GetNextPickyAsync(8);
                if (judge.Equal(await a, await b))
                {
                    sum++;
                }

                if (i % 50_000 == 0)
                {
                    Console.WriteLine("Working... " + (i / 50_000) + "%");
                }
            }

            Console.WriteLine("Judge: " + sum + " pairs match");
        }
    }

    class Generator
    {
        private int factor;
        private long current;

        public Generator(int factor, long startingValue)
        {
            this.factor = factor;
            current = startingValue;
        }

        public int GetNext()
        {
            current = (current * factor) % 2147483647;
            return (int)current;
        }

        public Task<int> GetNextPickyAsync(int picky)
        {
            return Task.Run(() =>
            {
                int n;
                do
                {
                    n = GetNext();
                } while (n % picky != 0);
                return n;
            });
        }
    }

    class Judge
    {
        public bool Equal(int a, int b)
        {
            return (a & 0xffff) == (b & 0xffff);
        }
    }
}
