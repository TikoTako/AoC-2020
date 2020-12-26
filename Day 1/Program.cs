using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Day_1
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
            foreach (var item in rawData)
            {
                if (rawData.Contains($"{2020 - int.Parse(item)}"))
                {
                    var mrX = int.Parse(item);
                    var mrY = 2020 - mrX;
                    WL($"{item} + {mrY} = {mrY + mrX} result {mrX * mrY}");
                    break;
                }
            }
        }

        static void PartTwo(List<string> rawData)
        {
            List<int> herp = new List<int>();
            // 2020 = a + b + c
            foreach (var item in rawData)
            {
                // 2020 - c = a + b
                herp.Add(2020 - int.Parse(item));
            }

            foreach (var ab in herp)
            {
                foreach (var possibleA in rawData)
                {
                    var a = int.Parse(possibleA);
                    if (ab > a)
                    {
                        var b = ab - a;
                        if (rawData.Contains(b.ToString()))
                        {
                            var c = 2020 - (a + b);
                            if (rawData.Contains(c.ToString()))
                            {
                                WL($"{a} + {b} + {c} = {a + b + c} result {a * b * c}");
                                return;
                            }
                        }
                    }
                }
            }
        }

        static void Main(string[] args)
        {            
            List<string> rawData = new List<string>(File.ReadLines("input.txt"));
            PartOne(rawData);
            PartTwo(rawData);
        }
    }
}
