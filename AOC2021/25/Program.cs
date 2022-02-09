using System;
using System.IO;

namespace _25
{
    class Program
    {
        static char[,] board;
        static int xExtent, yExtent;

        static void Main(string[] args)
        {
            ReadInput();
            Part1();
        }

        static void ReadInput()
        {
            String inputFile = "d:\\adventofcode\\AOC2021\\25\\input.txt";
            using (StreamReader inputRdr = new StreamReader(inputFile))
            {
                yExtent = 0;
                while (!inputRdr.EndOfStream)
                {
                    String line = inputRdr.ReadLine();
                    if (line.Length < 2)
                        break;
                    xExtent = line.Length;
                    ++yExtent;
                }
                board = new char[xExtent, yExtent];
            }
            using (StreamReader inputRdr = new StreamReader(inputFile))
            {
                int y = 0;
                while (!inputRdr.EndOfStream)
                {
                    String line = inputRdr.ReadLine();
                    for (int x = 0; x < xExtent; ++x)
                    {
                        board[x, y] = line[x];
                    }
                    ++y;
                }
            }
        }

        private static void Part1()
        {
            int moves = 0;
            bool moved = false;
            do
            {
                moved = false;
                //PrintBoard();
                moves++;
                char[,] newBoard = new char[xExtent, yExtent];
                for (int x = 0; x < xExtent; ++x)
                {
                    for (int y = 0; y < yExtent; ++y)
                    {
                        newBoard[x,y] = board[x,y];
                    }
                }
                // Do the easters first.
                for (int x = 0; x < xExtent; ++x)
                {
                    for (int y = 0; y < yExtent; ++y)
                    {
                        if (board[x, y] == '>')
                        {
                            int newX = x + 1;
                            if (newX == xExtent)
                                newX = 0;
                            if (board[newX, y] == '.')
                            {
                                moved = true;
                                newBoard[newX, y] = '>';
                                newBoard[x, y] = '.';
                            }
                        }
                    }
                }

                board = newBoard;
                newBoard = new char[xExtent, yExtent];
                for (int x = 0; x < xExtent; ++x)
                {
                    for (int y = 0; y < yExtent; ++y)
                    {
                        newBoard[x, y] = board[x, y];
                    }
                }

                // Then the southers
                for (int x = 0; x < xExtent; ++x)
                {
                    for (int y = 0; y < yExtent; ++y)
                    {
                        if (board[x, y] == 'v')
                        {
                            int newY = y + 1;
                            if (newY == yExtent)
                                newY = 0;
                            if (board[x, newY] == '.')
                            {
                                moved = true;
                                newBoard[x, newY] = 'v';
                                newBoard[x, y] = '.';
                            }
                        }
                    }
                }

                // Swap.
                board = newBoard;

            } while (moved);

            Console.WriteLine($"Stopped moving in {moves} moves");
        }

        private static void PrintBoard()
        {
            Console.WriteLine();
            for (int y = 0; y < yExtent; ++y)
            {
                for (int x = 0; x < xExtent; ++x)
                {
                    Console.Write(board[x, y]);
                }
                Console.WriteLine();
            }
        }
    }
}
