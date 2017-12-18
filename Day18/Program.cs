using System;
using System.Collections.Generic;
using System.IO;

namespace Day18
{
    class Program
    {
        static void Main(string[] args)
        {
            //PartOne(new string[] {
            //    "set a 1",
            //    "add a 2",
            //    "mul a a",
            //    "mod a 5",
            //    "snd a",
            //    "set a 0",
            //    "rcv a",
            //    "jgz a -1",
            //    "set a 1",
            //    "jgz a -2"
            //});
            // PartOne(File.ReadAllLines("input.txt"));

            //PartTwo(new string[] {
            //    "snd 1",
            //    "snd 2",
            //    "snd p",
            //    "rcv a",
            //    "rcv b",
            //    "rcv c",
            //    "rcv d"
            //});
            PartTwo(File.ReadAllLines("input.txt"));

            Console.ReadLine();
        }

        static void PartOne(string[] input)
        {
            var registers = new Registers();
            registers.Read(input);
            registers.Run();
        }

        static void PartTwo(string[] input)
        {
            // I'm not smart enough to do multithreaded programs that block on each other
            // while having a way to detect deadlocks.
            // So what I'm going to do instead is simulate cycles.

            Queue<long> aToB = new Queue<long>();
            Queue<long> bToA = new Queue<long>();
            var a = new Registers2(bToA, aToB, 0);
            var b = new Registers2(aToB, bToA, 1);

            a.Read(input);
            b.Read(input);

            while(!a.Stalled || !b.Stalled)
            {
                a.Run();
                b.Run();
            }

            Console.WriteLine(b.SendCount);
        }
    }
}
