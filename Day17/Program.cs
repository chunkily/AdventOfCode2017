using System;
using System.Collections.Generic;
using System.Linq;

namespace Day17
{
    class Program
    {
        static void Main(string[] args)
        {
            //PartOne(3);
            //PartOne(316);

            // YEAH LETS BRUTE FORCE IT.
            //PartTwoBruteForce(316);

            // Ok let's not brute force it...
            PartTwo(316);

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

        static void PartTwoBruteForce(int input)
        {
            var spinlock = new SpinLock(input);

            for (int i = 0; i < 50_000_000; i++)
            {
                spinlock.Next();

                if(i % 500_000 == 0)
                {
                    Console.WriteLine("Working... " + i / 500_000 + "%");
                }
            }
            var pos = spinlock.List.IndexOf(0);
            Console.WriteLine(spinlock.List[pos + 1]);
        }

        static void PartTwo(int input)
        {
            var spinlock = new AngrySpinLock(input);

            for (int i = 1; i < 50_000_000; i++)
            {
                spinlock.Next();

                if(i % 500_000 == 0)
                {
                    Console.WriteLine("Working... " + i / 500_000 + "%");
                }
            }
            Console.WriteLine(spinlock.ValueAfterZero);
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

    class AngrySpinLock
    {
        private readonly int step;
        private int currentPosition;
        private int nextValue;
        private int valueAfterZero;
        private int listCount;
        private int indexOfZero;

        public AngrySpinLock(int step)
        {
            this.step = step;
            // State after first insertion
            valueAfterZero = 1;
            listCount = 2;
            nextValue = 2;
            currentPosition = 1;
            indexOfZero = 0;
        }

        public void Next()
        {
            int nextPosition = (currentPosition + step) % listCount + 1;
            // Uh... wait this won't ever happen right?
            //if(nextPosition == indexOfZero)
            //{
            //    indexOfZero++;
            //}
            // If the nextPosition is the end of the list (index = listCount), the number is inserted at the end
            // If the nextPosition is at the start of the list (index = 0), the number is inserted at position 1

            if(nextPosition == indexOfZero + 1)
            {
                valueAfterZero = nextValue;
            }

            nextValue++;
            listCount++;
            currentPosition = nextPosition;
        }

        public int ValueAfterZero { get => valueAfterZero; }
    }
}
