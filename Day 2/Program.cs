using System;
using System.IO;
using System.Collections.Generic;

namespace Day_2
{
    class Program
    {
        // https://adventofcode.com/2020/day/2/input

        static void PartOne(List<string> data)
        {
            int validPasswords = 0;
            foreach (var item in data)
            {
                // 4-10 s: ssskssphrlpscsxrfsr
                // min-max letter: password
                var charCount = 0;
                var items = item.Replace('-', ' ').Replace(":", "").Split(' ');
                for (int i = 0; i < items[3].Length; i++)
                {
                    charCount += items[3][i].Equals(items[2][0]) ? 1 : 0;
                }
                validPasswords += charCount >= Convert.ToInt32(items[0]) && charCount <= Convert.ToInt32(items[1]) ? 1 : 0;
            }
            Console.WriteLine($"pwd count {data.Count} - valid {validPasswords}");
        }

        static void PartTwo(List<string> data)
        {
            int validPasswords = 0;
            foreach (var item in data)
            {
                // 1 - 3 a: abcde is valid: position 1 contains a and position 3 does not.
                // 1 - 3 b: cdefg is invalid: neither position 1 nor position 3 contains b.
                // 2 - 9 c: ccccccccc is invalid: both position 2 and position 9 contain c.
                var items = item.Replace('-', ' ').Replace(":", "").Split(' ');
                var pos1 = Convert.ToInt32(items[0]) - 1;
                var pos2 = Convert.ToInt32(items[1]) - 1;
                validPasswords += items[3][pos1] == items[3][pos2] ? 0 : (items[3][pos1] == items[2][0] ? 1 : (items[3][pos2] == items[2][0] ? 1 : 0));
            }
            Console.WriteLine($"pwd count {data.Count} - valid {validPasswords}");
        }

        static void Main(string[] args)
        {
            List<string> data = new List<string>(File.ReadAllLines("input.txt"));
            PartOne(data);
            PartTwo(data);
        }
    }
}
