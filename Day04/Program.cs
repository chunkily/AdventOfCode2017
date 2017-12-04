using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;

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
            // PartOne(File.ReadAllLines(args[0]));

            //PartTwo(new string[] {
            //    "abcde fghij",
            //    "abcde xyz ecdab",
            //    "a ab abc abd abf abj",
            //    "iiii oiii ooii oooi oooo",
            //    "oiii ioii iioi iiio"
            //});
            PartTwo(File.ReadAllLines(args[0]));

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
            var valid = lines.Count(IsPassPhraseValidTwo);
            Console.WriteLine(valid + " passphrases are valid under the new policy");
        }

        static bool IsPassPhraseValidTwo(string passphrase)
        {
            var words = passphrase.Split(" ");
            for (int i = 0; i < words.Length; i++)
            {
                var cArr = words[i].ToCharArray();
                Array.Sort(cArr);
                words[i] = new String(cArr);
            }
            return words.ToHashSet().Count() == words.Count();
        }
    }
}
