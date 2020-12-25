using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Day_6
{
    class Program
    {
        static void W(string str)
        {
            Console.Write(str + " ");
        }

        static void WL(string str)
        {
            Console.WriteLine(str);
        }

        static void PartOne(List<string> rawData)
        {
            var totalTotal = 0;
            bool[] answers = new bool[26];
            foreach (var dudeAnswers in rawData)
            {
                if (dudeAnswers.Length != 0)
                {                    
                    foreach (var Answer in dudeAnswers)
                    {
                        answers[Answer - 'a'] = true;
                    }
                }
                else
                {
                    var tot = 0;
                    foreach (var item in answers)
                    {
                        tot += item ? 1 : 0;
                    }                    
                    totalTotal += tot;
                    answers = new bool[26];
                }
            }
            WL($"Part one total: {totalTotal}");
        }

        static void PartTwo(List<string> rawData)
        {
            var dudeCounter = 0;
            var totalTotal = 0;
            int[] answers = new int[26];
            foreach (var dudeAnswers in rawData)
            {
                if (dudeAnswers.Length != 0)
                {
                    dudeCounter++;
                    foreach (var Answer in dudeAnswers)
                    {
                        answers[Answer - 'a']++;
                    }
                }
                else
                {
                    var tot = 0;
                        foreach (var item in answers)
                        {
                            tot += item == dudeCounter ? 1 : 0;
                        }
                    answers = new int[26];
                    totalTotal += tot;
                    dudeCounter = 0;
                }
            }
            WL($"Part two total: {totalTotal}");
        }

        static void Main(string[] args)
        {
            // one dude per line
            // 26 yes-or-no questions marked a through z.
            // group (1+ people)separated by empty line
            List<string> rawData = new List<string>(File.ReadLines("input.txt"));
            rawData.Add(""); // goddamn last line
            PartOne(rawData);
            PartTwo(rawData);
        }
    }
}
