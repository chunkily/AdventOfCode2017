using System;
using System.Linq;
using System.IO;

namespace Day04
{
    class Program
    {
        static void Main(string[] args)
        {
            //PartOne(new string[] {
            //    "aa bb cc dd ee",
            //    "aa bb cc dd aa",
            //    "aa bb cc dd aaa"
            //});
            PartOne(File.ReadAllLines(args[0]));

            Console.ReadLine();
        }

        static void PartOne(string[] lines)
        {
            var valid = lines.Count(IsPassPhraseValid);
            Console.WriteLine(valid + " passphrases are valid");
        }

        static bool IsPassPhraseValid(string passphrase)
        {
            var words = passphrase.Split(" ");
            return words.ToHashSet().Count() == words.Count();
        }

        static void PartTwo(string[] lines)
        {

        }
    }
}
