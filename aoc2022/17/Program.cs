using System;
using System.Collections.Generic;
using System.IO;

namespace _17
{
    internal class Program
    {
        static String wind = "";
        static List<String> board = new List<string>();
        static int topLine = 0;

        static void Main(string[] args)
        {
            ReadInput();
            Part1();
            // Part2();
        }

        static void ReadInput()
        {
            using (StreamReader inputRdr = new StreamReader("d:\\adventofcode\\AOC2022\\17\\input_test.txt"))
            {
                while (!inputRdr.EndOfStream)
                {
                    String line = inputRdr.ReadLine();
                    wind = wind + line.Trim();
                }
            }
        }

        static void Part1()
        {
            Console.WriteLine("Part1");
            // Start the board. 
            board.Add("+-------+");
            topLine = 1;

            for (int i = 0; i < 2/*2022*/; ++i)
            {
                Rock r = Rock.Get(i);

                // Starts 3 lines above the top. Make sure we have enough board. 
                while (board.Count < topLine + 3 + r.height)
                    board.Add("|.......|");

                Print();
            }
        }
    
        static void Print()
        {
            Console.WriteLine();
            for (int i = board.Count-1; i>0 && i > board.Count - 20; --i)
            {
                Console.WriteLine(board[i]);
            }
        }

        public class Rock
        {
            public static Rock Get(int i)
            {
                switch (i % 5)
                {
                    case 0: return new Rock0();
                    case 1: return new Rock1();
                    case 2: return new Rock2();
                    case 3: return new Rock3();
                    case 4: return new Rock4();
                }
                return null;
            }

            public int width;
            public int height;
        }

        // - shape
        public class Rock0 : Rock
        {
            public Rock0()
            {
                width = 4;
                height = 1;
            }
        }

        // + shape
        public class Rock1 : Rock
        {
            public Rock1()
            {
                width = 3;
                height = 3;
            }
        }

        // _| shape
        public class Rock2 : Rock
        {
            public Rock2()
            {
                width = 3;
                height = 3;
            }
        }

        // | shape
        public class Rock3 : Rock
        {
            public Rock3()
            {
                width = 1;
                height = 4;
            }
        }

        // [] shape
        public class Rock4 : Rock
        {
            public Rock4()
            {
                width = 2;
                height = 2;
            }
        }


    }

}
