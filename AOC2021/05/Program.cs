using System;
using System.Collections.Generic;
using System.IO;

namespace _05
{
    class Program
    {
        static List<LineSeg> input = new List<LineSeg>();
        static int xExtent = 0;
        static int yExtent = 0;

        static void Main(string[] args)
        {
            ReadInput();
            Part1();
            //Part2();
        }

        static void ReadInput()
        {
            using (StreamReader inputRdr = new StreamReader("d:\\adventofcode\\AOC2021\\05\\input.txt"))
            {
                while (!inputRdr.EndOfStream)
                {
                    String line = inputRdr.ReadLine();
                    LineSeg ls = new LineSeg(line);
                    input.Add(ls);
                    if (ls.xExtent() > xExtent)
                        xExtent = ls.xExtent();
                    if (ls.yExtent() > yExtent)
                        yExtent = ls.yExtent();

                }

                // Extent is 1 larger
                ++xExtent;
                ++yExtent;
            }
        }

        private static void Part1()
        {
            Console.WriteLine("Part 1");

            // Assume initialised to 0
            int[,] board = new int[xExtent, yExtent];

            foreach(LineSeg ls in input)
            {
                ls.AddToBoard(board);
            }

            // Count. 
            int over2 = 0;
            for (int x = 0; x< xExtent; ++x)
            {
                for (int y = 0; y< yExtent; ++y)
                {
                    if (board[x, y] >= 2)
                        ++over2;
                }
            }

            Console.WriteLine($"Count = {over2}");
        }
    }

    class LineSeg
    {
        public int x1, x2, y1, y2;

        public LineSeg(string line)
        {
            String[] pairs = line.Split(" -> ");
            String[] first = pairs[0].Split(',');
            x1 = Int32.Parse(first[0].Trim());
            y1 = Int32.Parse(first[1].Trim());
            String[] second = pairs[1].Split(',');
            x2 = Int32.Parse(second[0].Trim());
            y2 = Int32.Parse(second[1].Trim());

        }

        public void AddToBoard(int[,] board)
        {
            if (x1 != x2 && y1 != y2)
            {
                // Diagonal. 
                int x, y;
                int dx = 1, dy = 1;
                if (x2 < x1)
                    dx = -1;
                if (y2 < y1)
                    dy = -1;
                for (x = x1, y = y1; x != x2 + dx; x += dx, y += dy)
                    board[x, y]++;
            }
            else
            {
                int xlo = x1, xhi = x2;
                if (x2 < x1)
                {
                    xhi = x1;
                    xlo = x2;
                }
                int ylo = y1, yhi = y2;
                if (y2 < y1)
                {
                    yhi = y1;
                    ylo = y2;
                }

                // horiz / vert
                for (int x = xlo; x <= xhi; ++x)
                {
                    for (int y = ylo; y <= yhi; ++y)
                    {
                        board[x, y]++;
                    }
                }
            }
        }

        public int xExtent()
        {
            if (x2 > x1)
                return x2;
            return x1;
        }

        public int yExtent()
        {
            if (y2 > y1)
                return y2;
            return y1;
        }
    }
}
