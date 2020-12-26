using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace Day_7
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

        const int CONTAINLEN = 8;
        const string CONTAIN = "contain ";

        static (int, string) GetBagsCount(bool isFirst, bool isLast, string line)
        {
            var index = isFirst ? line.IndexOf(CONTAIN) + CONTAINLEN : 1;
            var lastex = line[^2] == 's' ? 6 : line[^2] == 'g' ? 5 : 4;
            var count = (byte)line[index];
            if (count == 'n')
            {
                return (0, null);
            }
            return (count - '0', line[(index + 2)..^lastex]);
        }

        class BagPartOne
        {
            public string Color;
            public List<string> ContainedBags;

            public BagPartOne(string bag)
            {
                Color = bag;
                ContainedBags = new List<string>();
            }
        }

        static void PartOne(List<string> rawData)
        {
            // wavy salmon bags contain 2 vibrant orange bags, 2 drab silver bags, 3 bright blue bags, 1 vibrant brown bag.
            // plaid lime bags contain 5 drab purple bags.
            // dotted black bags contain no other bags.
            List<BagPartOne> bagList = new List<BagPartOne>();
            foreach (var line in rawData)
            {
                var splitLine = line.Split(',');
                var bag = line[0..(line.IndexOf("bags ") - 1)];
                bagList.Add(new BagPartOne(bag));
                for (int i = 0; i < splitLine.Length; i++)
                {
                    var bEc = GetBagsCount(i == 0, i == splitLine.Length - 1, splitLine[i]);
                    // WL($"{bag} contains {bEc.Item1} {bEc.Item2}");
                    bagList[bagList.Count - 1].ContainedBags.Add(bEc.Item2);
                }
            }

            var muhShiny = "shiny gold";
            List<string> shinyContainers = new List<string>();
            List<string> allTheShinyContainers = new List<string>();
            var manyShiny = 0;
            foreach (var bag in bagList)
            {
                if (bag.ContainedBags.Contains(muhShiny))
                {
                    shinyContainers.Add(bag.Color);
                    manyShiny++;
                }
            }
            do
            {
                List<string> tmpBagList = new List<string>();
                foreach (var bag in bagList)
                {
                    foreach (var shinyContainer in shinyContainers)
                    {
                        if (bag.ContainedBags.Contains(shinyContainer) && !tmpBagList.Contains(bag.Color))
                        {
                            tmpBagList.Add(bag.Color);
                            if (!allTheShinyContainers.Contains(bag.Color))
                            {
                                allTheShinyContainers.Add(bag.Color);
                                manyShiny++;
                            }
                        }
                    }
                }
                shinyContainers = tmpBagList;
            } while (shinyContainers.Count > 0);
            WL($"Part one {muhShiny} >> {manyShiny}");
        }

        class BagPartTwo
        {
            public string Color;
            public List<Tuple<int, BagPartTwo>> ContainedBags;

            public BagPartTwo(string bag)
            {
                Color = bag;
                ContainedBags = new List<Tuple<int, BagPartTwo>>();
            }
        }

        static int PartTwo(string searchFor, List<string> rawData)
        {
            var fronk = 0;
            foreach (var line in rawData)
            {
                if (line.Contains($"{searchFor} bags contain "))
                {
                    var splitLine = line.Split(',');
                    var bag = line[0..(line.IndexOf("bags ") - 1)];
                    for (int i = 0; i < splitLine.Length; i++)
                    {
                        var bEc = GetBagsCount(i == 0, i == splitLine.Length - 1, splitLine[i]);
                        if (bEc.Item1 == 0)
                        {
                            //WL($"{bag}");
                            return 1;
                        }
                        var p = PartTwo(bEc.Item2, rawData);
                        var pp = p * bEc.Item1;
                        pp += p > 1 ? bEc.Item1 : 0;
                        //WL($"{bag} contains {bEc.Item1} {bEc.Item2} i {i} p {p} pp {pp}");                        
                        fronk += pp;
                    }
                    break;
                }
            }
            return fronk;
        }

        static void Main(string[] args)
        {
            List<string> rawData = new List<string>(File.ReadLines("input.txt"));
            PartOne(rawData);
            WL($"{PartTwo("shiny gold", rawData)}");
        }
    }
}
