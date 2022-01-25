using System;
using System.Collections.Generic;
using System.IO;

namespace _15
{
    class Program
    {
        static int[,] board;
        static int[,] minCost;
        static int xExtent, yExtent;
        static int costLimit;

        static void Main(string[] args)
        {
            ReadInput();
            //Part1();
            ExpandBoardForPart2();
            Attempt2();
        }

        static void ReadInput()
        {
            using (StreamReader inputRdr = new StreamReader("d:\\adventofcode\\AOC2021\\15\\input.txt"))
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
                board = new int[xExtent, yExtent];
                minCost = new int[xExtent, yExtent];
            }
            using (StreamReader inputRdr = new StreamReader("d:\\adventofcode\\AOC2021\\15\\input.txt"))
            {
                int y = 0;
                while (!inputRdr.EndOfStream)
                {
                    String line = inputRdr.ReadLine();
                    for (int x=0; x<xExtent; ++x)
                    {
                        board[x, y] = line[x] - '0';
                        minCost[x, y] = Int32.MaxValue;
                    }
                    ++y;
                }
            }
        }

        private static void ExpandBoardForPart2()
        {
            int[,] newBoard = new int[xExtent * 5, yExtent * 5];
            int[,] newMinCost = new int[xExtent * 5, yExtent * 5];

            // Tile the board. 
            for (int xx = 0; xx<5; ++xx)
            {
                for(int yy=0; yy<5; ++yy)
                {
                    for (int x = 0; x<xExtent; ++x)
                    {
                        for (int y=0; y<yExtent; ++y)
                        {
                            int tiledValue = board[x, y] + xx + yy;
                            if (tiledValue > 9)
                                tiledValue -= 9;
                            newBoard[xx * xExtent + x, yy * yExtent + y] = tiledValue;
                            newMinCost[xx * xExtent + x, yy * yExtent + y] = Int32.MaxValue;
                        }
                    }
                }
            }
            board = newBoard;
            minCost = newMinCost;
            xExtent *= 5;
            yExtent *= 5;
        }

        private static void Attempt2()
        {
            Console.WriteLine("Attempt 2");
            minCost[0, 0] = 0;
            List<Tuple<int, int>> activePoints = new List<Tuple<int, int>>();
            AddPoint(activePoints, 0, 0);
            costLimit = 1;
            do
            {
                List<Tuple<int, int>> newPoints = new List<Tuple<int, int>>();
                foreach (Tuple<int, int>point in activePoints)
                {
                    ExplorePoint(point.Item1, point.Item2, newPoints);
                }
                ++costLimit;
                activePoints = newPoints;
            } while (minCost[xExtent - 1, yExtent -1] == Int32.MaxValue);
            Console.WriteLine($"lowest cost is {minCost[xExtent - 1, yExtent - 1]}");
        }

        private static void ExplorePoint(int x, int y, List<Tuple<int,int>> activePoints)
        {
            int currCost = minCost[x, y];
            if (currCost > costLimit)
            {
                // Keep this in the new list for when it becomes under the limit.
                AddPoint(activePoints,x, y);
                return;
            }
            TryPoint(x, y + 1, currCost, activePoints);
            TryPoint(x + 1, y, currCost, activePoints);
            TryPoint(x, y - 1, currCost, activePoints);
            TryPoint(x - 1, y, currCost, activePoints);
            // Done this one now.
            RemovePoint(activePoints, x, y);
        }

        private static void TryPoint(int x, int y, int currCost, List<Tuple<int, int>> activePoints)
        {
            if (x >= 0 && x < xExtent && y >= 0 && y < yExtent)
            {
                // We only care if this is s better choice than we previously had. 
                int newCost = currCost + board[x, y];
                if (newCost < minCost[x, y])
                {
                    minCost[x, y] = newCost;
                    AddPoint(activePoints, x, y);
                    ExplorePoint(x, y, activePoints);
                }
            }
        }

        // Helpers for tuples. Clearly an extended class would be better!
        private static void AddPoint(List<Tuple<int, int>> active, int x, int y)
        {
            Tuple<int, int> xy = new Tuple<int, int>(x, y);
            if (!active.Contains(xy))
            {
                active.Add(xy);
            }
        }
        private static void RemovePoint(List<Tuple<int, int>> active, int x, int y)
        {
            Tuple<int, int> xy = new Tuple<int, int>(x, y);
            if (active.Contains(xy))
            {
                active.Remove(xy);
            }
        }

        private static void Part1()
        {
            Console.WriteLine("Part 1");
            minCost[0, 0] = 0;
            PresetMinCosts();
            FindPaths(0, 0);
            Console.WriteLine($"Lowest cost is {minCost[xExtent - 1, yExtent - 1]}");
        }

        static void PresetMinCosts()
        {
            int cost = - board[0,0];
            for (int y = 0; y < yExtent; ++y)
            {
                for (int x = 0; x < xExtent; ++x)
                {
                    cost += board[x, y];
                    minCost[x, y] = cost;
                }
                cost = minCost[0, y];
            }
        }

        static void FindPaths(int x, int y)
        {
            int currCost = minCost[x, y];
            // down, right, up, left.
            NextStep(x, y + 1, currCost);
            NextStep(x + 1, y, currCost);
            NextStep(x, y - 1, currCost);
            NextStep(x - 1, y, currCost);
        }

        static void NextStep(int x, int y, int currCost)
        {
            if (x >= 0 && x < xExtent && y >= 0 && y < yExtent)
            {
                int cost = currCost + board[x, y];
                if (cost <= minCost[x, y])
                {
                    minCost[x, y] = cost;
                    FindPaths(x, y);
                }
            }
        }
    }
}
