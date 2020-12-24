using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace Day_4
{
    class Program
    {
        static void PartOne(List<string> data)
        {
            /*
            byr (Birth Year)
            iyr (Issue Year)
            eyr (Expiration Year)
            hgt (Height)
            hcl (Hair Color)
            ecl (Eye Color)
            pid (Passport ID)
            cid (Country ID)
            */
            var validCounter = 0;
            foreach (var passport in data)
            {
                var fieldsCount = passport.Replace(':', ' ').Split(' ').Length / 2;
                var isValid = fieldsCount == 8 || (fieldsCount == 7 && !passport.Contains("cid"));
                validCounter += isValid ? 1 : 0;
                //Console.WriteLine($"{fieldsCount} valid { (isValid ? "yep" : "no")}");
            }
            Console.WriteLine($"{validCounter}");
        }

        static readonly List<string> eyecolors = new List<string> { "amb", "blu", "brn", "gry", "grn", "hzl", "oth" };

        static bool ValidateField(string field, string data)
        {
            if (field.Equals("cid"))
            {
                // cid (Country ID) - ignored, missing or not.
                return true;
            }
            else if (field.Equals("pid"))
            {
                // pid (Passport ID) - a nine-digit number, including leading zeroes.                
                if (data.Length == 9)
                {
                    return int.TryParse(data, out _);
                }
            }
            else if (field.Equals("ecl"))
            {
                //ecl (Eye Color) - exactly one of: amb blu brn gry grn hzl oth.
                return eyecolors.Contains(data) && data.Length == 3;
            }
            else if (field.Equals("hcl"))
            {
                // hcl (Hair Color) - a # followed by exactly six characters 0-9 or a-f.
                if (data.Length == 7)
                {
                    return Regex.IsMatch(data, @"(#)[0-9a-fA-F]+\b");
                }
            }
            else if (field.Equals("hgt"))
            {
                // hgt(Height) - a number followed by either cm or in:
                // If cm, the number must be at least 150 and at most 193.
                // If in, the number must be at least 59 and at most 76.
                if (data.EndsWith("cm") || data.EndsWith("in"))
                {
                    try
                    {
                        int h = Convert.ToInt32(data[0..^2]);
                        return (data.EndsWith("cm") && (h >= 150 || h <= 193)) || (data.EndsWith("in") && (h >= 59 || h <= 76));
                    }
                    catch (Exception)
                    {
                        return false;
                    }
                }
            }
            else if (field.Equals("eyr"))
            {
                // eyr (Expiration Year) - four digits; at least 2020 and at most 2030.
                if (data.Length == 4)
                {
                    return int.TryParse(data, out int eyr) && (eyr >= 2020 && eyr <= 2030);
                }
            }
            else if (field.Equals("iyr"))
            {
                // iyr (Issue Year) - four digits; at least 2010 and at most 2020.
                if (data.Length == 4)
                {
                    return int.TryParse(data, out int iyr) && (iyr >= 2010 && iyr <= 2020);
                }
            }
            else if (field.Equals("byr"))
            {
                // byr (Birth Year) - four digits; at least 1920 and at most 2002.
                if (data.Length == 4)
                {
                    return int.TryParse(data, out int byr) && (byr >= 1920 && byr <= 2002);
                }
            }
            return false;
        }

        static void PartTwo(List<string> passportList)
        {
            var validCounter = 0;
            foreach (var passport in passportList)
            {
                var bob = 0;
                var fields = passport.Replace(':', ' ').Split(' ');
                if (fields.Length == 16 || (fields.Length == 14 && !passport.Contains("cid")))
                {
                    var currentField = 0;
                    for (; currentField < fields.Length;)
                    {
                        var field = fields[currentField++];
                        var data = fields[currentField++];
                        var isValid = ValidateField(field, data);
                        Console.Write($"{field}:{data}[{(isValid?"OK":"NO")}] ");
                        if (!isValid)
                        {
                            //break;
                        }
                        else
                        {
                            bob++;
                        }
                    }
                    Console.WriteLine();
                    if ((fields.Length == 16 && bob == 8) || (fields.Length == 14 && bob == 7))
                    {
                        validCounter++;
                    }
                }
            }
            Console.WriteLine($"{validCounter}");
        }

        static void Main(string[] args)
        {
            var rawData = File.ReadAllText("input.txt");
            var CRLF = "\r\n"; // text file saved on windows            
            List<string> data = new List<string>(rawData.Replace(CRLF + CRLF, "#@!?").Replace(CRLF, " ").Split("#@!?"));
            PartOne(data);
            PartTwo(data);
        }
    }
}
