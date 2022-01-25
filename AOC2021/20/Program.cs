using System;
using System.IO;

namespace _20
{
    class Program
    {
        static bool[,] board;
        static bool[] imageMap = new bool[512];
        static int xExtent, yExtent;
        static int border = 55;

        static void Main(string[] args)
        {
            ReadInput();
            //Part1();
            Part2();
        }

        static void ReadInput()
        {
            using (StreamReader inputRdr = new StreamReader("d:\\adventofcode\\AOC2021\\20\\input.txt"))
            {
                // Start with the collection of 9-bit indexed bool values.
                String line = inputRdr.ReadLine();

                if (line.Length != 512)
                    throw new Exception("Unexpected image map size");

                for(int i=0; i<512; ++i)
                {
                    if (line[i] == '.')
                        imageMap[i] = false;
                    else
                        imageMap[i] = true;
                }

                // Blank
                inputRdr.ReadLine();

                yExtent = 0;
                while (!inputRdr.EndOfStream)
                {
                    line = inputRdr.ReadLine();

                    if (line.Length < 2)
                        break;
                    xExtent = line.Length;
                    ++yExtent;
                }

                // Board will be extra all round.
                xExtent +=  2 * border;
                yExtent += 2 * border;
                board = new bool[xExtent, yExtent];
            }
            using (StreamReader inputRdr = new StreamReader("d:\\adventofcode\\AOC2021\\20\\input.txt"))
            {
                inputRdr.ReadLine();
                inputRdr.ReadLine();

                int y = border;
                while (!inputRdr.EndOfStream)
                {
                    String line = inputRdr.ReadLine();
                    for (int x = 0; x < line.Length; ++x)
                    {
                        board[x + border, y] = (line[x] == '#') ? true : false;
                    }
                    ++y;
                }
            }
        }

        private static void Part1()
        {
            PrintBoard();
            Console.WriteLine();
            ProcessImage();
            PrintBoard();
            ProcessImage();
            PrintBoard();
            Console.WriteLine($"Total 1's remaining is {CountBoard()}");
        }

        private static void Part2()
        {
            for (int step =0; step<50; ++step)
            {
                ProcessImage();
            }
            Console.WriteLine($"Total 1's remaining is {CountBoard()}");
        }

        private static int CountBoard()
        {
            int ct = 0;
            for (int x = 0; x < xExtent; ++x)
            {
                for (int y = 0; y < yExtent; ++y)
                {
                    if (board[x, y])
                        ++ct;
                }
            }
            return ct;
        }


        private static void ProcessImage()
        {
            bool[,] newBoard = new bool[xExtent, yExtent];
            for(int x=0; x< xExtent; ++x)
            {
                for (int y=0; y<yExtent; ++y)
                {
                    int idx;
                    // Edges are all or nothing.
                    if (x == 0 || x == xExtent-1 || y == 0 || y== yExtent - 1)
                    {
                        idx = (board[x, y]) ? 511 : 0;
                    }
                    else
                    {
                        idx = 0;
                        for (int yy=y-1; yy<=y+1; ++yy)
                        {
                            for (int xx=x-1; xx<=x+1; ++xx)
                            {
                                idx <<= 1;
                                if (board[xx, yy])
                                    idx += 1;
                            }
                        }
                    }
                    newBoard[x, y] = imageMap[idx];
                }
            }
            board = newBoard;
        }

        private static void PrintBoard()
        {
            Console.WriteLine();
            for (int y = 0; y < yExtent; ++y)
            {
                for (int x = 0; x < xExtent; ++x)
                {
                    Console.Write(board[x, y] ? '#' : '.');
                }
                Console.WriteLine();
            }
        }
    }
}

