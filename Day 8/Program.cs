using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace Day_8
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

        static void PartOne(List<string> rawCode)
        {
            int currentLine = 0;
            long accumulator = 0;
            Dictionary<int, bool> codeTracker3000 = new Dictionary<int, bool>();
            do
            {
                // WL($"[{currentLine}] {rawCode[currentLine]}");
                codeTracker3000.Add(currentLine, true);
                if (rawCode[currentLine].Contains("acc"))
                {
                    accumulator += int.Parse(rawCode[currentLine][3..]);
                }
                else if (rawCode[currentLine].Contains("jmp"))
                {
                    currentLine += int.Parse(rawCode[currentLine][3..]);
                    continue;
                }
                currentLine++;
            } while (!codeTracker3000.TryGetValue(currentLine, out _));
            WL($"Part one accumulator: {accumulator}");
        }

        static void PartTwo(List<string> rawCode, bool isJmp)
        {
            int jmpCounterTotal = 0;
            int jmpnopCounterCurrent = -1;
            rawCode.ForEach(x => jmpCounterTotal += x.Contains(isJmp ? "jmp" : "nop") ? 1 : 0);
            for (int i = 0; i < jmpCounterTotal; i++)
            {
                int currentLine = 0;
                long accumulator = 0;
                Dictionary<int, bool> codeTracker3000 = new Dictionary<int, bool>();
                var jmpnopLocal = -1;
                bool infiniteLoopCheck = false;

                while (!infiniteLoopCheck && currentLine != rawCode.Count)
                {
                    codeTracker3000.Add(currentLine, true);
                    // W($"[{currentLine}] {rawCode[currentLine]}");
                    switch (rawCode[currentLine][0..3])
                    {
                        case "acc":
                            accumulator += int.Parse(rawCode[currentLine][3..]);
                            // WL("");
                            break;
                        case "nop":
                            // var asdsad = " NOP";
                            if (!isJmp && jmpnopCounterCurrent == jmpnopLocal)
                            {
                                //  asdsad = " NOP -> JMP";
                                currentLine += int.Parse(rawCode[currentLine][3..]);
                                currentLine--;
                            }
                            // WL(!isJmp ? asdsad : "");
                            if (!isJmp)
                                jmpnopLocal++;
                            break;
                        case "jmp":
                            {
                                //  var asdsad2 = " JMP -> NOP";
                                if (!isJmp || jmpnopCounterCurrent != jmpnopLocal)
                                {
                                    //  asdsad2 = " JMP";
                                    currentLine += int.Parse(rawCode[currentLine][3..]);
                                    currentLine--;
                                }
                                // WL(isJmp ? asdsad2 : "");
                                if (isJmp)
                                    jmpnopLocal++;
                            }
                            break;
                    }
                    currentLine++;
                    infiniteLoopCheck = codeTracker3000.TryGetValue(currentLine, out _);
                }

                // WL($"Part two accumulator: [{accumulator}] {infiniteLoopCheck} {jmpnopCounterCurrent} {currentLine} == {rawCode.Count} {currentLine == rawCode.Count}");
                jmpnopCounterCurrent++;
                if (currentLine == rawCode.Count)
                {
                    WL($"Part two accumulator: {accumulator}");
                    break;
                }
            }
        }

        static void Main(string[] args)
        {
            List<string> rawData = new List<string>(File.ReadLines("input.txt"));
            PartOne(rawData);
            PartTwo(rawData, true); // 1125 
        }
    }
}
