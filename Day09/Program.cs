using System;
using System.IO;

namespace Day09
{
    class Program
    {
        static void Main(string[] args)
        {
            // var testInput = "{{<a!>},{<a!>},{<a!>},{<ab>}}";
            // var b = System.Text.Encoding.UTF8.GetBytes(testInput);
            // var stream = new MemoryStream(b,false);
            // using(var reader = new StreamReader(stream))
            // {
            //     PartOne(reader); 
            // }

            using(var reader = new StreamReader(args[0]))
            {
                PartOne(reader);
            }

            Console.ReadLine();
        }

        const int lb = '{';
        const int rb = '}';
        const int lt = '<';
        const int rt = '>';
        const int esc = '!';

        static void PartOne(StreamReader reader)
        {
            int totalScore = 0;
            int groupScore = 0;
            bool inGarbage = false;
            while(!reader.EndOfStream)
            {
                int next = reader.Read();
                if(inGarbage)
                {
                    if(next == esc)
                    {
                        // Consume next char
                        reader.Read();
                    }
                    else if(next == rt)
                    {
                        inGarbage = false;
                    }
                }
                else
                {
                    if(next == lb)
                    {
                        groupScore++;
                    }
                    else if(next == rb)
                    {
                        totalScore += groupScore;
                        groupScore--;
                    }
                    else if(next == lt)
                    {
                        // Garbage
                        inGarbage = true;
                    }
                }
            }

            Console.WriteLine("total score: " + totalScore);
        }
    }
}
