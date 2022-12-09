using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;

namespace _08
{
    internal class Program
    {
        static List<string> input = new List<string>();
        static byte[,] forest;
        static int xSize, ySize;

        static void Main(string[] args)
        {
            ReadInput();
            Part1();
            Part2();
        }

        static void ReadInput()
        {
            using (StreamReader inputRdr = new StreamReader("d:\\adventofcode\\AOC2022\\08\\input.txt"))
            {
                while (!inputRdr.EndOfStream)
                {
                    String line = inputRdr.ReadLine();
                    input.Add(line);
                }
                // Make the forest. 
                xSize = input[0].Length;
                ySize = input.Count;
                forest = new byte[xSize, ySize];
                for (int y = 0; y < ySize; ++y)
                {
                    for (int x = 0; x < xSize; ++x)
                    {
                        forest[x, y] = (byte)(input[y][x] - '0');
                    }
                }
            }
        }

        static void Part1()
        {
            Console.WriteLine("Part1");
            int visible = 0;
            for (int y = 0; y < ySize; ++y)
            {
                for (int x = 0; x < xSize; ++x)
                {
                    if (IsVisible(x, y))
                        visible++;
                }
            }
            Console.WriteLine($"Visible: {visible}");
        }

        static void Part2()
        {
            Console.WriteLine("Part2");
            int best = 0;
            for (int y = 0; y < ySize; ++y)
            {
                for (int x = 0; x < xSize; ++x)
                {
                    int scenic = GetScenic(x, y);
                    if (scenic > best)
                        best = scenic;
                }
            }
            Console.WriteLine($"Highest Scenic score: {best}");
        }

        static int GetScenic(int xPos, int yPos)
        {
            byte ht = forest[xPos, yPos];
            // Multiply by the tree count in each direction.
            int scoreL = 0;
            for (int i=xPos-1; i>=0; --i)
            {
                ++scoreL;
                if (forest[i, yPos] >= ht)
                    break;
            }

            int scoreU = 0;
            for (int i = yPos - 1; i >= 0; --i)
            {
                ++scoreU;
                if (forest[xPos, i] >= ht)
                    break;
            }

            int scoreR = 0;
            for (int i = xPos + 1; i < xSize; ++i)
            {
                ++scoreR;
                if (forest[i, yPos] >= ht)
                    break;
            }

            int scoreD = 0;
            for (int i = yPos + 1; i < ySize; ++i)
            {
                ++scoreD;
                if (forest[xPos, i] >= ht)
                    break;
            }

            return scoreL * scoreU * scoreR * scoreD;
        }

        static bool IsVisible(int xPos, int yPos)
        {
            bool vis = true;
            byte ht = forest[xPos, yPos];

            // Test each of 4 ways.
            for (int i=0; i< xPos; ++i)
            {
                if (forest[i, yPos] >= ht)
                {
                    vis = false;
                    break;
                }
            }

            if (vis) return true;
            vis = true;
            for (int i = xPos + 1; i<xSize; ++i)
            {
                if (forest[i, yPos] >= ht)
                {
                    vis = false;
                    break;
                }
            }
            if (vis) return true;
            vis = true;
            for (int i = 0; i < yPos; ++i)
            {
                if (forest[xPos, i] >= ht)
                {
                    vis = false;
                    break;
                }
            }
            if (vis) return true;
            vis = true;
            for (int i = yPos + 1; i < ySize; ++i)
            {
                if (forest[xPos, i] >= ht)
                {
                    vis = false;
                    break;
                }
            }
            return vis;
        }
    }
}
