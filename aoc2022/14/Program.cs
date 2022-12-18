using System;
using System.Collections.Generic;
using System.IO;

namespace _14
{
    internal class Program
    {
        static List<String> input = new List<String>();
        static int xMin, xMax;
        static int yMax;
        static char[,] cave;

        static void Main(string[] args)
        {
            ReadInput();
            Parse();
            Part1();
            //Part2();
        }

        static void ReadInput()
        {
            using (StreamReader inputRdr = new StreamReader("d:\\adventofcode\\AOC2022\\14\\input.txt"))
            {
                while (!inputRdr.EndOfStream)
                {
                    String line = inputRdr.ReadLine();
                    input.Add(line);
                }
            }
        }

        static void Parse()
        {
            xMin = 1000;
            xMax = 0;
            yMax = 0;
            foreach (string line in input)
            {
                string[] parts = line.Trim().Split(' ');
                foreach (string part in parts)
                {
                    if (part == "->")
                        continue;
                    string[] numbers = part.Split(',');
                    int x = int.Parse(numbers[0]);
                    int y = int.Parse(numbers[1]);
                    if (x > xMax)
                        xMax = x;
                    if (x < xMin)
                        xMin = x;
                    if (y > yMax)
                        yMax = y;
                }
            }

            // Part 2.
            yMax += 1;
            xMax += 500;

            // Allocate the cave.
            cave = new char[xMax + 1, yMax + 1];

            // Add the rocks
            foreach (string line in input)
            {
                int xPrev = -1;
                int yPrev = -1;
                string[] parts = line.Trim().Split(' ');
                foreach (string part in parts)
                {
                    if (part == "->")
                        continue;
                    string[] numbers = part.Split(',');
                    int xNew = int.Parse(numbers[0]);
                    int yNew = int.Parse(numbers[1]);
                    if (xPrev != -1)
                    {
                        int xInc = 0;
                        if (xNew > xPrev) xInc = 1;
                        else if (xNew < xPrev) xInc = -1;
                        int yInc = 0;
                        if (yNew > yPrev) yInc = 1;
                        else if (yNew < yPrev) yInc = -1;
                        for (int x = xPrev, y = yPrev; x != xNew || y != yNew; x += xInc, y += yInc)
                        {
                            cave[x, y] = '#';
                        }
                        cave[xNew, yNew] = '#';
                    }
                    xPrev = xNew;
                    yPrev = yNew;
                }
            }
        }

        static void Part1()
        {
            Console.WriteLine("Part1");
            int sand = 0;
            int x, y;
            do
            {
                sand++;
                x = 500;
                y = 0;
                do
                {
                    // Part 1
                    //if (y >= yMax)
                    //    goto over;

                    // Part 2
                    if (cave[500, 0] != 0)
                        goto over;
                    if (y >= yMax)
                    {
                        cave[x, y] = 'o';
                        break;
                    }
                    if (cave[x, y + 1] == 0)
                        y++;
                    else
                    {
                        if (cave[x - 1, y + 1] == 0)
                        {
                            y++;
                            x--;
                        }
                        else if (cave[x + 1, y + 1] == 0)
                        {
                            y++;
                            x++;
                        }
                        else
                        {
                            cave[x, y] = 'o';
                            break;
                        }
                    }
                } while (true);
            } while (true);
            over:

            Print();
            Console.WriteLine($"Total sand: {sand-1}");
        }

        static void Print()
        {
            for (int y=0;y<=yMax; ++y)
            {
                for (int x = xMin; x <= xMax; ++x)
                {
                    if (cave[x, y] == 0)
                        Console.Write(' ');
                    else
                        Console.Write(cave[x, y]);
                }
                Console.WriteLine();

            }
            Console.WriteLine();
        }
    }
}
