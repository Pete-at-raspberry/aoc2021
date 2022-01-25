using System;
using System.IO;

namespace _11
{
    class Program
    {
        static int[,] board;

        static void Main(string[] args)
        {
            ReadInput();
            Part1();
            //Part2();
        }

        static void ReadInput()
        {

            board = new int[10, 10];

            using (StreamReader inputRdr = new StreamReader("d:\\adventofcode\\AOC2021\\11\\input.txt"))
            {
                int row = 0;
                while (!inputRdr.EndOfStream)
                {
                    String line = inputRdr.ReadLine();
                    for (int x = 0; x < 10; ++x)
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

            int flashes = 0;
            for (int step = 0; step < 250; ++step)
            {
                for (int x = 0; x<10; ++x)
                {
                    for (int y = 0; y<10; ++y)
                    {
                        // Add one to this cell, and propagate if we need to. 
                        IncrementCell(x, y);
                    }
                }

                // Now, count the flashes and reset those cells. 
                int theseFlashes = 0;
                for (int x = 0; x < 10; ++x)
                {
                    for (int y = 0; y < 10; ++y)
                    {
                        if (board[x,y] >= 10)
                        {
                            board[x, y] = 0;
                            ++theseFlashes;
                        }
                    }
                }
                flashes += theseFlashes;
                if (theseFlashes == 100)
                    Console.WriteLine($"sync'd after step {step+1}");
            }
            Console.WriteLine($"Total flashes is {flashes}");
        }

        private static void IncrementCell(int x, int y)
        {
            // Add one, if it flashes then move to the surrounding cells.
            if (10 == ++board[x, y])
            {
                // It flashed!
                for (int xd = x-1; xd <= x+1; ++ xd)
                {
                    for (int yd = y-1; yd <= y+1; ++yd)
                    {
                        if (!(xd == x && yd == y))
                        {
                            if (IsOnBoard(xd,yd))
                            {
                                IncrementCell(xd, yd);
                            }
                        }
                    }
                }
            }
        }

        private static bool IsOnBoard(int x, int y)
        {
            return (x >= 0 && x < 10 && y >= 0 && y < 10);
        }

    }
}
