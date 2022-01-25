using System;
using System.Collections.Generic;
using System.IO;

namespace _13
{
    class Program
    {
        static int[,] board;
        static int xExtent, yExtent;
        static List<KeyValuePair<char, int>> folds = new List<KeyValuePair<char, int>>();

        static void Main(string[] args)
        {
            ReadInput();
            Part1();
            //Part2();
        }

        static void ReadInput()
        {

            board = new int[10, 10];

            using (StreamReader inputRdr = new StreamReader("d:\\adventofcode\\AOC2021\\13\\input.txt"))
            {
                while (!inputRdr.EndOfStream)
                {
                    String line = inputRdr.ReadLine();
                    if (line.Length < 2)
                        break;
                    String[] words = line.Split(',');
                    int x = Int32.Parse(words[0]);
                    int y = Int32.Parse(words[1]);
                    if (x > xExtent)
                        xExtent = x;
                    if (y > yExtent)
                        yExtent = y;
                }
                ++xExtent;
                ++yExtent;
                board = new int[xExtent, yExtent];
            }
            using (StreamReader inputRdr = new StreamReader("d:\\adventofcode\\AOC2021\\13\\input.txt"))
            {
                while (!inputRdr.EndOfStream)
                {
                    String line = inputRdr.ReadLine();
                    if (line.Contains(','))
                    {
                        // Add these to the board
                        String[] words = line.Split(',');
                        int x = Int32.Parse(words[0]);
                        int y = Int32.Parse(words[1]);
                        board[x, y] = 1;
                    }
                    else if (line.Contains("fold"))
                    {
                        String[] parts = line.Split('=');
                        char xy = parts[0][11];
                        int pos = Int32.Parse(parts[1]);
                        folds.Add(new KeyValuePair<char, int>(xy, pos));
                    }
                }
            }
        }

        private static void Part1()
        {
            Console.WriteLine("Part 1");

            foreach(KeyValuePair<char, int> firstfold in folds)
                if (firstfold.Key == 'x')
                    FoldX(firstfold.Value);
                else
                    FoldY(firstfold.Value);

            // Count the set places on the board.
            //int count = 0;
            //for (int x = 0; x < xExtent; ++x)
            //{
            //    for (int y=0; y<yExtent; ++y)
            //    {
            //        if (board[x, y] != 0)
            //            ++count;
            //    }
            //}
            //Console.WriteLine($"Found {count} cells");
            for (int y = 0; y < yExtent; ++y)
            {
                for (int x = 0; x < xExtent; ++x)
                {
                    if (board[x, y] != 0)
                        Console.Write('*');
                    else
                        Console.Write(' ');
                }
                Console.WriteLine();
            }
        }

        private static void FoldX(int line)
        {
            for (int y = 0; y < yExtent; ++y)
            {
                for (int x = 0; x < line; ++x)
                {
                    board[x, y] += board[xExtent - 1 - x, y];
                }
            }
            xExtent = line;
        }
        private static void FoldY(int line)
        {
            for (int y = 0; y < line; ++y)
            {
                for (int x = 0; x < xExtent; ++x)
                {
                    int yVal = line + line - y;
                    if (yVal < yExtent)
                        board[x, y] += board[x, yVal];
                }
            }
            yExtent = line;
        }
    }
}
