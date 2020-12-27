using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Day_9
{
    class Program
    {
        static void W(string str)
        {
            Console.Write(str);
        }

        static void WL(string str)
        {
            Console.WriteLine(str);
        }

        static long PartOne(List<long> rawData, int preamble)
        {
            // 1-2-3-4-5 6
            // 6 = a + b && a!=b a&&b = 1..5
            var index = 0;
            bool found = false;
            do
            {
                var C = index + preamble;
                //WL($"{rawData[index]} {rawData[index+preamble-1]}");
                for (int i = 0; i < preamble; i++)
                {
                    found = false;
                    var A = index + i;
                    for (int ii = 0; ii < preamble; ii++)
                    {
                        var B = index + ii;
                        if (rawData[C] == rawData[A] + rawData[B] && A != B)
                        {
                            //WL($"{rawData[index + preamble]} == {rawData[index + i]} + {rawData[index + ii]}");
                            i = ii = 9001; // instead of breaks
                            found = true;
                        }
                    }
                }
                //WL($">> {rawData[C]} {(found ? " - PASS" : " - FAIL")}");
                index++;
            } while (found && (index + preamble < rawData.Count));
            WL($"Part one result: {rawData[--index + preamble]}");
            return rawData[index + preamble];
        }

        static void PartTwo(List<long> rawData, long weakness)
        {
            long
                min = 0,
                max = 0;
            for (int i = 0; i < rawData.Count; i++)
            {
                var elIndexo = i;
                long sommaContinua = 0;
                min = rawData[elIndexo];
                while ((elIndexo < rawData.Count) && (sommaContinua < weakness) && !(sommaContinua == weakness))
                {
                    sommaContinua += rawData[elIndexo];
                    min = rawData[elIndexo] < min ? rawData[elIndexo] : min;
                    max = rawData[elIndexo] > max ? rawData[elIndexo] : max;
                    elIndexo++;
                }
                if (sommaContinua == weakness)
                {
                    WL($"Part two result: {min} + {max} = {min + max}");
                    return;
                }
            }
        }

        static void Main(string[] args)
        {
            bool test = false;

            List<long> rawData = new List<long>();
            new List<string>(File.ReadLines(test ? "example.txt" : "input.txt")).ForEach(x => rawData.Add(long.Parse(x)));            
            PartTwo(rawData, PartOne(rawData, test ? 5 : 25));
        }
    }
}
