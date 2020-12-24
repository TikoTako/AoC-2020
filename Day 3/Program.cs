using System;
using System.IO;
using System.Collections.Generic;

namespace Day_3
{
    class Program
    {
        static void PartOne(List<string> data)
        {
            // Starting at the top-left corner of your map and following a slope of right 3 and down 1, how many trees would you encounter?
            int x = 0;
            int treeCounter = 0;
            data.RemoveAt(0);
            foreach (var line in data)
            {
                //x += x + 3 < line.Length ? 3 : -(line.Length -3);
                x += x + 3 >= line.Length ? -(line.Length - 3) : 3;
                /*x += 3;
                if (x >= line.Length)
                {
                    x = x - line.Length;
                }
                */
                treeCounter += line[x] == '#' ? 1 : 0;
            }
            Console.WriteLine($"Tot tree {treeCounter}");
        }

        static void PartTwo(List<string> data)
        {
            /* Right 1, down 1.
               Right 3, down 1. (This is the slope you already checked.)
               Right 5, down 1.
               Right 7, down 1.
               Right 1, down 2.
            */
            int[] goRight = { 1, 3, 5, 7, 1 };
            int[] goDown = { 1, 1, 1, 1, 2 };
            int[] results = { 0, 0, 0, 0, 0 };

            for (int i = 0; i < goRight.Length; i++)
            {
                int treeCounter = 0,
                    x = 0,
                    y = 0;
                do
                {
                    y += goDown[i];
                    x += x + goRight[i] >= data[y].Length ? -(data[y].Length - goRight[i]) : goRight[i];
                    treeCounter += data[y][x] == '#' ? 1 : 0;
                } while (y + goDown[i] < data.Count);
                Console.WriteLine($"{data.Count} {y} {goRight[i]} {goDown[i]} {treeCounter}");
                results[i] = treeCounter;
            }

            var r = results[0] * results[1] * results[2] * results[3] * results[4];
            Console.WriteLine($"Tot tree {r}");
        }

        static void Main(string[] args)
        {
            List<string> data = new List<string>(File.ReadAllLines("input.txt"));
            PartTwo(data);
            PartOne(data);
        }
    }
}
