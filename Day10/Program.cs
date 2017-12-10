using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day10
{
    class Program
    {
        static void Main(string[] args)
        {
            // PartOne(5, new int[] { 3, 4, 1, 5 });

            string[] inputText = File.ReadAllText(args[0]).Split(',');
            int[] input = inputText.Select(i => { return Int32.Parse(i); }).ToArray();
            PartOne(256, input);

            Console.ReadLine();
        }

        static void PartOne(int size, int[] lengths)
        {
            var circularArr = new CircularArray(size);
            foreach(var l in lengths)
            {
                circularArr.Transform(l);
            }
            Console.WriteLine("first two values multiplied: " + circularArr.MultiplyFirstTwo());
        }
    }

    class CircularArray
    {
        private int[] arr;
        private int currentPosition = 0;
        private int skipSize = 0;

        public CircularArray(int length)
        {
            arr = new int[length];
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = i;
            }
        }

        public void Transform(int length)
        {
            // Reverse section
            var section = new int[length];
            for (int i = 0; i < section.Length; i++)
            {
                var j = (currentPosition + i) % arr.Length;
                section[i] = arr[j];
            }
            for (int i = 0; i< section.Length; i++)
            {
                var j = (currentPosition + i) % arr.Length;
                var k = section.Length - i - 1;
                arr[j] = section[k];
            }

            currentPosition += length + skipSize;
            skipSize++;

            currentPosition = currentPosition % arr.Length;
        }

        public int MultiplyFirstTwo()
        {
            return arr[0] * arr[1];
        }
    }
}
