using System;
using System.Collections.Generic;
using System.IO;

namespace _09
{
    internal class Program { 
        static List<string> input = new List<string>();
        static SortedSet<int> tails = new SortedSet<int>(); 

        static void Main(string[] args)
        {
            ReadInput();
            Part1();
            Part2();
        }

        static void ReadInput()
        {
            using (StreamReader inputRdr = new StreamReader("d:\\adventofcode\\AOC2022\\09\\input.txt"))
            {
                while (!inputRdr.EndOfStream)
                {
                    String line = inputRdr.ReadLine();
                    input.Add(line);
                }
            }
        }

        static void Part1()
        {
            Console.WriteLine("Part1");
            int xh = 0;
            int yh = 0;
            int xt = 0;
            int yt = 0;

            AddTailPos(xt, yt);
            foreach (string line in input)
            {
                string[] bits = line.Split(' ');
                char dir = bits[0][0];
                int ct = Int32.Parse(bits[1]);
                for (int i = 0; i < ct; i++)
                {
                    MoveHead(dir, ref xh, ref yh);
                    MoveTail(xh, yh, ref xt, ref yt);
                    AddTailPos(xt, yt);
                }
            }
            Console.WriteLine($"Number of tail positions: {tails.Count}");
        }

        static void Part2()
        {
            Console.WriteLine("Part2");
            tails.Clear();
            int[] xt = new int[10];
            int[] yt = new int[10];
            for (int i = 0; i < 10; i++)
            {
                xt[i] = yt[i] = 0;
            }

            AddTailPos(xt[9], yt[9]);
            foreach (string line in input)
            {
                string[] bits = line.Split(' ');
                char dir = bits[0][0];
                int ct = Int32.Parse(bits[1]);
                for (int i = 0; i < ct; i++)
                {
                    MoveHead(dir, ref xt[0], ref yt[0]);
                    for (int j = 0; j < 9; ++j)
                    {
                        MoveTail(xt[j], yt[j], ref xt[j + 1], ref yt[j + 1]);
                    }
                    AddTailPos(xt[9], yt[9]);
                }
            }
            Console.WriteLine($"Number of tail positions: {tails.Count}");
        }

        static void MoveHead(char dir, ref int x, ref int y)
        {
            switch (dir)
            {
                case 'U': ++y; break;
                case 'D': --y; break;
                case 'R': ++x; break;
                case 'L': --x; break;
            }
        }

        static void MoveTail(int xh, int yh, ref int xt, ref int yt)
        {
            // Do we need to move? 
            int xdiff = xh - xt;
            int ydiff = yh - yt;
            if (Math.Abs(xdiff) > 1)
            {
                xt = xt + xdiff / 2;
                if (Math.Abs(ydiff) > 1)
                {
                    yt = yt + ydiff / 2;
                }
                else
                {
                    yt = yt + ydiff;
                }
            }
            else if (Math.Abs(ydiff) > 1)
            {
                yt = yt + ydiff / 2;
                if (Math.Abs(xdiff) > 1)
                {
                    xt = xt + xdiff / 2;
                }
                else
                {
                    xt = xt + xdiff;
                }
            }
        }

        static void AddTailPos(int x, int y)
        {
            int hash = x * 100000 + y;
            if (!tails.Contains(hash))
                tails.Add(hash);
        }
    }
}
