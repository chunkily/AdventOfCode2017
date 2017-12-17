using System;
using System.Collections.Generic;
using System.Linq;

namespace Day17
{
    class Program
    {
        static void Main(string[] args)
        {
            PartOne(3);
            // PartOne(316);

            Console.ReadLine();
        }

        static void PartOne(int input)
        {
            var spinlock = new SpinLock(input);

            int pos = 0;
            for (int i = 0; i < 2017; i++)
            {
                pos = spinlock.Next();
            }
            Console.WriteLine(spinlock.List[pos+1]);
        }
    }

    class SpinLock
    {
        private readonly int step;
        private List<int> list;
        private int currentPosition;
        private int nextValue;

        public SpinLock(int step)
        {
            this.step = step;
            List = new List<int>() { 0 };
            currentPosition = 0;
            nextValue = 1;
        }

        public List<int> List { get => list; set => list = value; }

        public int Next()
        {
            currentPosition = (currentPosition + step) % List.Count + 1;
            List.Insert(currentPosition, nextValue++);
            return currentPosition;
        }
    }
}
