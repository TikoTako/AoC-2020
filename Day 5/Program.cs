using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Day_5
{
    class BoardingPass
    {
        private string Pass;
        private int Row;
        private int Col;
        private int ID;

        void MinMax(char bfrl, ref (int, int) minmax)
        {
            var tmpX = (minmax.Item2 - minmax.Item1) / 2 + 1;
            switch (bfrl)
            {
                case 'B': // upper half
                case 'R':
                    minmax.Item1 += tmpX;
                    break;
                case 'F': // lower half
                case 'L':
                    minmax.Item2 -= tmpX;
                    break;
            }
        }

        int Decode(string pass)
        {
            var minmax = (0, pass.Length == 7 ? 127 : 7);
            for (int i = 0; i < pass.Length; i++)
            {
                MinMax(pass[i], ref minmax);
            }
            if (minmax.Item1 != minmax.Item2)
            {
                throw new Exception("da fuq ?");
            }
            return minmax.Item1;
        }

        void DecodeRow()
        {
            // [1234567][123]
            //   0..127
            Row = Decode(Pass[0..^3]);
        }

        void DecodeCol()
        {
            // [1234567][123]
            //           0..7
            Col = Decode(Pass[^3..]);
        }

        void CalculateSeatID()
        {
            // Every seat also has a unique seat ID: multiply the row by 8, then add the column.
            ID = (Row * 8) + Col;
        }

        public BoardingPass(string SeatCode)
        {
            Pass = SeatCode;
            DecodeRow();
            DecodeCol();
            CalculateSeatID();
        }

        public int GetSeatID()
        {
            return ID;
        }

        public Tuple<string, int, int, int> GetAll()
        {
            return Tuple.Create(Pass, Row, Col, ID);
        }

        public override string ToString()
        {
            return $"{Pass} [{Row}-{Col}] {ID}";
        }
    }

    class Program
    {
        static Tuple<string, int, int, int>[] TESTS = { Tuple.Create("FBFBBFFRLR",  44, 5, 357),   // FBFBBFFRLR row 44, column 5. 357
                                                        Tuple.Create("BFFFBBFRRR",  70, 7, 567),   // BFFFBBFRRR: row 70, column 7, seat ID 567.
                                                        Tuple.Create("FFFBBBFRRR",  14, 7, 119),   // FFFBBBFRRR: row 14, column 7, seat ID 119.
                                                        Tuple.Create("BBFFBBFRLL", 102, 4, 820) }; // BBFFBBFRLL: row 102, column 4, seat ID 820.

        public static void W(string str)
        {
            Console.WriteLine(str);
        }

        static void Main(string[] args)
        {
            foreach (var Test in TESTS)
            {
                W(new BoardingPass(Test.Item1).ToString());
            }
            W("--------------------");
            List<BoardingPass> boardingPasses = new List<BoardingPass>();
            var rawData = File.ReadAllLines("input.txt");
            foreach (var item in rawData)
            {
                if (item.Length == 10)
                {
                    boardingPasses.Add(new BoardingPass(item));
                }
            }
            // What is the highest seat ID on a boarding pass?
            boardingPasses.Sort((x, y) => x.GetSeatID().CompareTo(y.GetSeatID()));
            W($"Highest seat ID [{boardingPasses[boardingPasses.Count - 1].GetSeatID()}]");
            // Your seat wasn't at the very front or back, though; the seats with IDs +1 and -1 from yours will be in your list.
            // What is the ID of your seat ?
            // TL;DR; find the first hole in the list
            for (int i = 1; i < boardingPasses.Count; i++)
            {
                if (boardingPasses[i - 1].GetSeatID() +1 != boardingPasses[i].GetSeatID())
                {
                    W($"Found my seat ID [{boardingPasses[i - 1].GetSeatID() + 1}]");
                }
            }
        }
    }
}
