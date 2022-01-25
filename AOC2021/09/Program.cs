using System;
using System.Collections.Generic;
using System.IO;

namespace _09
{
    class Program
    {
        static int[,] board;
        static bool[,] done;
        static int xExtent = 0;
        static int yExtent = 0;

        static void Main(string[] args)
        {
            ReadInput();
            //Part1();
            Part2();
        }

        static void ReadInput()
        {
            using (StreamReader inputRdr = new StreamReader("d:\\adventofcode\\AOC2021\\09\\input.txt"))
            {
                yExtent = 0;
                while (!inputRdr.EndOfStream)
                {
                    String line = inputRdr.ReadLine();
                    xExtent = line.Length;
                    ++yExtent;
                }
            }
            board = new int[xExtent, yExtent];
            done = new bool[xExtent, yExtent];

            using (StreamReader inputRdr = new StreamReader("d:\\adventofcode\\AOC2021\\09\\input.txt"))
            {
                int row = 0;
                while (!inputRdr.EndOfStream)
                {
                    String line = inputRdr.ReadLine();
                    for (int x = 0; x < xExtent; ++x)
                    {
                        board[x, row] = line[x] - '0';
                    }
                    ++row;
                }
            }
        }

        private static void Part1()
        {
            Console.WriteLine("Part 1");
            int score = 0;
            for (int x=0; x< xExtent; ++x)
            {
                for (int y = 0; y < yExtent; ++y)
                {
                    if (IsLow(x,y))
                    {
                        score += board[x, y] + 1;
                    }
                }
            }

            Console.WriteLine($"Part 1 score is {score}");
        }

        private static void Part2()
        {
            // Use a floodfill, with a parallel array for cells that have been touched. 
            List<int> basinSizes = new List<int>();

            for (int x = 0; x < xExtent; ++x)
            {
                for (int y = 0; y < yExtent; ++y)
                {
                    // Is this cell part of a new basin? 
                    if (!done[x,y] && board[x,y] != 9)
                    {
                        // Yes. Find it. 
                        basinSizes.Add(findBasinSize(x, y));
                    }
                }
            }

            // OK, should have all the basins. 
            Console.WriteLine($"Found {basinSizes.Count} basins");
            basinSizes.Sort();
            int score = basinSizes[basinSizes.Count - 1] * basinSizes[basinSizes.Count - 2] * basinSizes[basinSizes.Count - 3];
            Console.WriteLine($"Final score = {score}");
        }

        private static int findBasinSize(int x, int y)
        {
            int sz = 1;
            done[x, y] = true;
            // Recurse. 
            if (IsOnBoard(x - 1, y) && !done[x - 1, y] && board[x - 1, y] != 9)
                sz += findBasinSize(x - 1, y);
            if (IsOnBoard(x + 1, y) && !done[x + 1, y] && board[x + 1, y] != 9)
                sz += findBasinSize(x + 1, y);
            if (IsOnBoard(x, y - 1) && !done[x, y - 1] && board[x, y - 1] != 9)
                sz += findBasinSize(x, y - 1);
            if (IsOnBoard(x, y + 1) && !done[x, y + 1] && board[x, y + 1] != 9)
                sz += findBasinSize(x, y + 1);
            return sz;
        }

        private static bool IsOnBoard(int x, int y)
        {
            return (x >= 0 && x < xExtent && y >= 0 && y < yExtent);
        }

        private static bool IsLow(int x, int y)
        {
            // Low if its 4 neighbours are not lower. 
            int val = board[x, y];
            if (IsOnBoard(x - 1, y) && board[x - 1, y] <= val)
                return false;
            if (IsOnBoard(x + 1, y) && board[x + 1, y] <= val)
                return false;
            if (IsOnBoard(x, y - 1) && board[x, y - 1] <= val)
                return false;
            if (IsOnBoard(x, y + 1) && board[x, y + 1] <= val)
                return false;
            return true;
        }
    }
}
