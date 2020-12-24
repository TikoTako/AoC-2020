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
                return (data.Length == 9) && Regex.IsMatch(data, @"[0-9]+\b");//&& int.TryParse(data, out _);
            }
            else if (field.Equals("ecl"))
            {
                //ecl (Eye Color) - exactly one of: amb blu brn gry grn hzl oth.
                return eyecolors.Contains(data) && data.Length == 3;
            }
            else if (field.Equals("hcl"))
            {
                // hcl (Hair Color) - a # followed by exactly six characters 0-9 or a-f.
                return (data.Length == 7) && Regex.IsMatch(data, @"(#)[0-9a-fA-F]+\b");
            }
            else if (field.Equals("hgt"))
            {
                // hgt(Height) - a number followed by either cm or in:
                // If cm, the number must be at least 150 and at most 193.
                // If in, the number must be at least 59 and at most 76.
                if (data.EndsWith("cm") || data.EndsWith("in"))
                {
                    return int.TryParse(data[0..^2], out int hgt) &&
                        ((data.EndsWith("cm") && (hgt >= 150 || hgt <= 193)) ||
                         (data.EndsWith("in") && (hgt >= 59 || hgt <= 76)));
                }
            }
            else if (field.Equals("eyr"))
            {
                // eyr (Expiration Year) - four digits; at least 2020 and at most 2030.
                return (data.Length == 4) && int.TryParse(data, out int eyr) && (eyr >= 2020 && eyr <= 2030);
            }
            else if (field.Equals("iyr"))
            {
                // iyr (Issue Year) - four digits; at least 2010 and at most 2020.
                return (data.Length == 4) && int.TryParse(data, out int iyr) && (iyr >= 2010 && iyr <= 2020);
            }
            else if (field.Equals("byr"))
            {
                // byr (Birth Year) - four digits; at least 1920 and at most 2002.
                return (data.Length == 4) && int.TryParse(data, out int byr) && (byr >= 1920 && byr <= 2002);
            }
            return false;
        }

        static string red = "\u001b[31;1m";
        static string reset = "\u001b[0m";

        static void PartTwo(List<string> passportList)
        {
            var invalidCounter = 0;
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
                        Console.Write($"{(!isValid ? red : "")}{field}:{data}{(!isValid ? reset : "")} ");
                        if (!isValid)
                        {
                            invalidCounter++;
                            break;
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
            Console.WriteLine($"valid [{validCounter}] invalid [{invalidCounter}]");
        }

        static void PartTwoB(List<Passport> passportList)
        {
            var validCounter = 0;
            var invalidCounter = 0;
            foreach (var passport in passportList)
            {
                if (passport.isValidPartTwo())
                {
                    validCounter++;
                }
                else
                {
                    invalidCounter++;
                }
                // Console.WriteLine(passport);
            }
            Console.WriteLine($"valid [{validCounter}] invalid [{invalidCounter}]");
        }

        static List<Passport> PartOneB(List<string> lotsOfFields)
        {
            Passport tmpPassport = new Passport();
            List<Passport> r = new List<Passport>();
            foreach (var field in lotsOfFields)
            {
                if (field.Length > 4) // xxx:x
                {
                    typeof(Passport).GetProperty(field[0..3]).SetValue(tmpPassport, field[4..]);
                    continue;
                }
                if (tmpPassport.isValidPartOne())
                {
                    r.Add(tmpPassport);
                }
                tmpPassport = new Passport();
            }
            if (!tmpPassport.validatedPartOne().HasValue) // if no empty lastline the continue exit the foreach before the tmpPassport.isValidPartOne()
            {                
                if (tmpPassport.isValidPartOne())
                {
                    r.Add(tmpPassport);
                }
            }
            Console.WriteLine($"{r.Count}");
            return r;
        }

        class Passport
        {
            public string byr { get; set; } = ""; // (Birth Year)
            public string iyr { get; set; } = ""; // (Issue Year)
            public string eyr { get; set; } = ""; // (Expiration Year)
            public string hgt { get; set; } = ""; // (Height)
            public string hcl { get; set; } = ""; // (Hair Color)
            public string ecl { get; set; } = ""; // (Eye Color)
            public string pid { get; set; } = ""; // (Passport ID)
            public string cid { get; set; } = ""; // (Country ID)

            bool? _validatedPartOne = null;

            public bool? validatedPartOne()
            {
                return _validatedPartOne;
            }

            public bool isValidPartOne()
            {
                foreach (var item in GetType().GetProperties())
                {
                    if (!item.Name.Equals("cid") && item.GetValue(this).Equals(""))
                    {
                        return (bool)(_validatedPartOne = false);
                    }
                }
                return (bool)(_validatedPartOne = true);
            }

            private bool ParseDate(string data, int min, int max)
            {
                return (data.Length == 4) && int.TryParse(data, out int intData) && (intData >= min && intData <= max);
            }

            bool ParseColor(string str, bool isHair)
            {
                return isHair ? Regex.IsMatch(str, @"^#[0-9a-fA-F]{6}$") : eyecolors.Contains(str);
            }

            bool ParsePid(string str)
            {
                return Regex.IsMatch(str, @"^[0-9]{9}$");
            }

            bool ParseHeight(string str)
            {
                return int.TryParse(str[0..^2], out int hgt) &&
                    (
                    (str[^2..].Equals("cm") && (hgt >= 150 && hgt <= 193)) ||
                    (str[^2..].Equals("in") && (hgt >= 59 && hgt <= 76))
                     );
            }

            public bool isValidPartTwo()
            {
                /*
                    byr (Birth Year) - four digits; at least 1920 and at most 2002.
                    iyr (Issue Year) - four digits; at least 2010 and at most 2020.
                    eyr (Expiration Year) - four digits; at least 2020 and at most 2030.
                    hgt (Height) - a number followed by either cm or in:
                        If cm, the number must be at least 150 and at most 193.
                        If in, the number must be at least 59 and at most 76.
                    hcl (Hair Color) - a # followed by exactly six characters 0-9 or a-f.
                    ecl (Eye Color) - exactly one of: amb blu brn gry grn hzl oth.
                    pid (Passport ID) - a nine-digit number, including leading zeroes.
                    cid (Country ID) - ignored, missing or not.
                 */
                return valid =
                    ParseDate(byr, 1920, 2002) &&
                    ParseDate(iyr, 2010, 2020) &&
                    ParseDate(eyr, 2020, 2030) &&
                    ParseHeight(hgt) &&
                    ParseColor(hcl, true) &&
                    ParseColor(ecl, false) &&
                    ParsePid(pid);
            }

            bool valid;

            public override string ToString()
            {
                var r = $"pid={pid} cid={(cid == "" ? "null" : "cid")} byr={byr} iyr={iyr} eyr={eyr} hgt={hgt} hcl={hcl} ecl={ecl}";
                return valid ? r : $"{red}{r}{reset}";
            }
        }

        static void Main(string[] args)
        {
            var CRLF = "\r\n"; // text file saved on windows            
            /*
            var rawData = File.ReadAllText("input.txt");
            List<string> data = new List<string>(rawData.Replace(CRLF + CRLF, "#@!?").Replace(CRLF, " ").Split("#@!?"));
            PartOne(data); // 206 ok
            PartTwo(data); // 125 incorrect -- correct is 123
            */
            List<string> dataB = new List<string>(File.ReadAllText("input.txt").Replace(CRLF, " ").Split(" "));
            var passportList = PartOneB(dataB); // 206 ok
            PartTwoB(passportList); // correct is 123

            /*
                ParseHeight HURRR DURRRRRRR
                         ((data.EndsWith("cm") && (hgt >= 150 || hgt <= 193)) ||
                          (data.EndsWith("in") && (hgt >= 59 || hgt <= 76)));
            
            mfw accidentally "OR" instead of "AND"
                        (str[^2..].Equals("cm") && (hgt >= 150 && hgt <= 193)) ||
                        (str[^2..].Equals("in") && (hgt >= 59 && hgt <= 76))
            */
        }
    }
}
