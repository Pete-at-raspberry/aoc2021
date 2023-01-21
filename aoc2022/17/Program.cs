using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Xml.Linq;

namespace _17
{
    internal class Program
    {
        static String wind = "";
        static char[,] board = new char[9, 200000];
        static int topLine = 0;

        static void Main(string[] args)
        {
            ReadInput();
            Part1();
            Part2();
        }

        static void ReadInput()
        {
            using (StreamReader inputRdr = new StreamReader("d:\\adventofcode\\AOC2022\\17\\input.txt"))
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
            for (int i=0; i<9; i++)
            {
                if (i == 0 || i == 8)
                    board[i, 0] = '+';
                else
                    board[i, 0] = '-';
            }
            topLine = 1;
            for (int y = 1; y<board.GetLength(1); y++)
            {
                board[0, y] = '|';
                board[8, y] = '|';
            }

            int moveIdx = 0;
            for (int i = 0; i < 2022; ++i)
            {
                Rock r = Rock.Get(i);
                int x = 3;
                int y = topLine + 3;
                while(true)
                {
                    // Rock moves left / right
                    char move = wind[moveIdx++];
                    if (moveIdx >= wind.Length)
                        moveIdx = 0;
                    int newx = x;
                    if (move == '>')
                        newx++;
                    else
                        newx--;
                    if (r.CanMove(newx, y))
                        x = newx;

                    // Rock moves down
                    if (r.CanMove(x, y-1))
                    {
                        y--;
                    }
                    else
                    {
                        // Rock has settled.
                        r.AddToBoard(x, y, i);
                        break;
                    }
                }

                // Work out the top line
                do
                {
                    bool found = false;
                    for (int xt = 1; xt < 8; ++xt)
                    {
                        if (board[xt, topLine] != 0)
                        {
                            found = true;
                            break;
                        }
                    }
                    if (found)
                    {
                        topLine++;
                    }
                    else
                    {
                        break;
                    }
                } while (true);

                //Print();
            }
            Print();
            Console.WriteLine($"Top line is {topLine-1}");
        }
    

        private static void Part2()
        {
            Console.WriteLine();
            Console.WriteLine("Part2");

            Dictionary<int, Position> positions = new Dictionary<int, Position>();

            // Reset the board
            topLine = 1;
            for (int y = 1; y < board.GetLength(1); y++)
            {
                board[0, y] = '|';
                board[8, y] = '|';
                for (int x = 1; x < 8; x++)
                    board[x, y] = (char)0;
            }

            int moveIdx = 0;
            int lastRock = 1000000;
            long totalRocks = 1000000000000;
            long repeatedHeight =0;
            for (int i = 0; i< lastRock; i++)
            {
                Rock r = Rock.Get(i);
                int x = 3;
                int y = topLine + 3;
                while (true)
                {
                    // Rock moves left / right
                    char move = wind[moveIdx++];
                    if (moveIdx >= wind.Length)
                        moveIdx = 0;
                    int newx = x;
                    if (move == '>')
                        newx++;
                    else
                        newx--;
                    if (r.CanMove(newx, y))
                        x = newx;

                    // Rock moves down
                    if (r.CanMove(x, y - 1))
                    {
                        y--;
                    }
                    else
                    {
                        // Rock has settled.
                        r.AddToBoard(x, y, i);
                        break;
                    }
                }

                // Work out the top line
                do
                {
                    bool found = false;
                    for (int xt = 1; xt < 8; ++xt)
                    {
                        if (board[xt, topLine] != 0)
                        {
                            found = true;
                            break;
                        }
                    }
                    if (found)
                    {
                        topLine++;
                    }
                    else
                    {
                        break;
                    }
                } while (true);

                // Have we got a repeat? Save this position
                int position = moveIdx * 10 + (i % 5);
                if (positions.ContainsKey(position))
                {
                    // Is this a repeat? 
                    Position p = positions[position];
                    if (topLine - p.topLine == p.topLine - p.lastTopLine && i - p.count == p.count - p.lastCount && repeatedHeight == 0)
                    {
                        // Its a match!!!
                        Console.WriteLine($"Match: {position}, {topLine}");
                        Print();

                        totalRocks -= (long)i;
                        long repeatCount = (long)(p.count - p.lastCount);
                        long repeatHeight = (long)(p.topLine - p.lastTopLine);
                        long rocksLeft = totalRocks % repeatCount;
                        repeatedHeight = (totalRocks / repeatCount) * repeatHeight;
                        lastRock = (int)(i + (int)rocksLeft);
                    }
                    else
                    {
                        p.lastTopLine = p.topLine;
                        p.topLine = topLine;
                        p.lastCount = p.count;
                        p.count = i;
                    }
                }
                else
                {
                    Position p = new Position();
                    p.topLine = topLine;
                    p.lastTopLine = 0;
                    p.count = i;
                    p.lastCount = 0;
                    positions.Add(position, p);
                }
            } 
            Print();
            Console.WriteLine($"Top line is {repeatedHeight + (long)topLine - 1}");

        }

        static void Print()
        {
            Console.WriteLine();
            for (int y = topLine + 1; y >= 0 && y > topLine - 20; y--)
            {
                for (int x=0; x<9; x++)
                {
                    char b = board[x, y];
                    if (b == 0)
                        b = ' ';
                    Console.Write(b);
                }
                Console.WriteLine();
            }
        }

        public class Position
        {
            public int topLine;
            public int lastTopLine;
            public int count;
            public int lastCount;
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

            public bool CanMove(int x, int y)
            {
                for (int xx=x; xx < x+width; xx++)
                {
                    for (int yy=y; yy < y+height; yy++)
                    {
                        if (board[xx, yy] != 0 && mask[xx-x,yy-y] == '#')
                            return false;
                    }
                }
                return true;
            }

            public void AddToBoard(int x, int y, int i)
            {
                for (int xx = x; xx < x + width; xx++)
                {
                    for (int yy = y; yy < y + height; yy++)
                    {
                        if (mask[xx-x,yy-y] == '#')
                            board[xx, yy] = (char)((i % 26) + 'A');
                    }
                }
            }

            public int width;
            public int height;
            public char[,] mask;
        }

        // - shape
        public class Rock0 : Rock
        {
            public Rock0()
            {
                width = 4;
                height = 1;
                mask = new char[,] { { '#' }, { '#' }, { '#' }, { '#' } };
            }
        }

        // + shape
        public class Rock1 : Rock
        {
            public Rock1()
            {
                width = 3;
                height = 3;
                mask = new char[,] { { '.', '#', '.' }, { '#', '#', '#' }, { '.', '#', '.' } };
            }
        }

        // _| shape
        public class Rock2 : Rock
        {
            public Rock2()
            {
                width = 3;
                height = 3;
                mask = new char[,] { { '#', '.', '.' }, { '#', '.', '.' }, { '#', '#', '#' } };
            }
        }

        // | shape
        public class Rock3 : Rock
        {
            public Rock3()
            {
                width = 1;
                height = 4;
                mask = new char[,] { { '#', '#', '#', '#' } };
            }
        }

        // [] shape
        public class Rock4 : Rock
        {
            public Rock4()
            {
                width = 2;
                height = 2;
                mask = new char[,] { { '#', '#' }, { '#', '#' } };
            }
        }


    }

}
