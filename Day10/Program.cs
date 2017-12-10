using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Day10
{
    class Program
    {
        static void Main(string[] args)
        {
            //var testArray = new CircularArray(5);
            //PartOne(testArray, new int[] { 3, 4, 1, 5 });
            //Console.WriteLine("first two values multiplied: " + testArray.MultiplyFirstTwo());

            string inputText = File.ReadAllText(args[0]);
            //int[] input = inputText.Split(',').Select(i => { return Int32.Parse(i); }).ToArray();
            //var circularArray = new CircularArray(256);
            //PartOne(circularArray, input);
            //Console.WriteLine("first two values multiplied: " + circularArray.MultiplyFirstTwo());

            //Console.WriteLine(PartTwo(""));
            //Console.WriteLine(PartTwo("AoC 2017"));
            //Console.WriteLine(PartTwo("1,2,3"));
            //Console.WriteLine(PartTwo("1,2,4"));
            Console.WriteLine(PartTwo(inputText));

            Console.ReadLine();
        }

        static void PartOne(CircularArray circularArray, int[] lengths)
        {
            foreach(var l in lengths)
            {
                circularArray.Transform(l);
            }
        }

        static string PartTwo(string input)
        {
            int[] lengths = new int[input.Length + 5];
            for (int i = 0; i < input.Length; i++)
            {
                lengths[i] = input[i];
            }
            int[] suffix = new int[] { 17, 31, 73, 47, 23 };
            suffix.CopyTo(lengths, input.Length);

            var circularArray = new CircularArray(256);

            for (int i = 0; i < 64; i++)
            {
                PartOne(circularArray, lengths);
            }

            StringBuilder sb = new StringBuilder();
            foreach(var i in circularArray.GetDenseHash())
            {
                sb.AppendFormat("{0:x2}", i);
            }
            return sb.ToString();
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

        public int[] GetDenseHash()
        {
            int[] denseHash = new int[16];

            for (int i = 0; i < denseHash.Length; i++)
            {
                int first = i * 16;
                var last = first + 16;
                for (int j = first; j < last; j++)
                {
                    denseHash[i] = denseHash[i] ^ arr[j];
                }
            }

            return denseHash;
        }
    }
}
