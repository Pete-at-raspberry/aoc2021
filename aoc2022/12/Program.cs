using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

/** This gives an answer that is 2 too many. I have no idea why. **/

namespace _12
{
    internal class Program
    {
        static List<String> input = new List<String>();
        static char[,] letters;
        static int[,] count;
        static int xExtent, yExtent;
        static int xStart, yStart;

        static void Main(string[] args)
        {
            ReadInput();
            Parse();
            //Part1();
            Part2();
        }

        static void ReadInput()
        {
            using (StreamReader inputRdr = new StreamReader("d:\\adventofcode\\AOC2022\\12\\input.txt"))
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
            xExtent = input[0].Length;
            yExtent = input.Count;
            letters = new char[xExtent, yExtent];
            count = new int[xExtent, yExtent];
            for (int y = 0; y < yExtent; y++)
            {
                string line = input[y];
                for (int x=0; x < xExtent; ++x)
                {
                    letters[x, y] = line[x];
                    count[x, y] = 0;
                    if (line[x] == 'S')
                    {
                        xStart = x;
                        yStart = y;
                    }
                }
            }
        }

        static void Part1()
        {
            Console.WriteLine("Part1");

            int steps = 0;
            List<Point> edge = new List<Point>();
            edge.Add(new Point(xStart, yStart));
            letters[xStart, yStart] = 'a';
            while (steps++ < 10000)
            {
                List<Point> newEdge = new List<Point>();
                foreach (Point p in edge)
                {
                    char letter = letters[p.X, p.Y];
                    if (letter == 'z')
                        goto done;
                    letter++;
                    letters[p.X, p.Y] = (char)255;
                    Point up = new Point(p.X, p.Y - 1);
                    Point down = new Point(p.X, p.Y + 1);
                    Point left = new Point(p.X - 1, p.Y);
                    Point right = new Point(p.X + 1, p.Y);
                    TryPoint(newEdge, letter, up);
                    TryPoint(newEdge, letter, down);
                    TryPoint(newEdge, letter, left);
                    TryPoint(newEdge, letter, right);
                }
                edge = newEdge;
             //   PrintBoard();
            }
            done:
            Console.WriteLine($"Total steps {steps}");
        }

        static int TryBoard(int x, int y)
        {
            int steps = 0;
            List<Point> edge = new List<Point>();
            edge.Add(new Point(x, y));
            while (steps++ < 10000)
            {
                List<Point> newEdge = new List<Point>();
                foreach (Point p in edge)
                {
                    char letter = letters[p.X, p.Y];
                    if (letter == 'z')
                        return steps;
                    letter++;
                    letters[p.X, p.Y] = (char)255;
                    Point up = new Point(p.X, p.Y - 1);
                    Point down = new Point(p.X, p.Y + 1);
                    Point left = new Point(p.X - 1, p.Y);
                    Point right = new Point(p.X + 1, p.Y);
                    TryPoint(newEdge, letter, up);
                    TryPoint(newEdge, letter, down);
                    TryPoint(newEdge, letter, left);
                    TryPoint(newEdge, letter, right);
                }
                if (newEdge.Count == 0)
                    return -1;
                edge = newEdge;
                //   PrintBoard();
            }
            return -1;
        }

        static void Part2()
        {
            int minSteps = 464;
            int minX, minY;
            for (int x = 0; x<xExtent; ++x)
            {
                for (int y = 0; y<yExtent; ++y)
                {
                    Parse();
                    if (letters[x,y] == 'a')
                    {
                        int steps = TryBoard(x, y);
                        if (steps != -1 && steps < minSteps)
                        {
                            minSteps = steps;
                            minX = x;
                            minY = y;
                        }
                    }
                }
            }
            Console.WriteLine($"min steps {minSteps}");
        }

        private static void PrintBoard()
        {
            for (int y=0; y<yExtent; y++)
            {
                for (int x=0; x<xExtent; x++)
                {
                    if (letters[x, y] > (char)250)
                        Console.Write('.');
                    else
                        Console.Write(letters[x, y]);
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        private static void TryPoint(List<Point> edge, char letter, Point p)
        {
            if (p.X >= 0 && p.X < xExtent && p.Y >= 0 && p.Y < yExtent)
            {
                if (letters[p.X, p.Y] <= letter)
                {
                    if (!edge.Contains(p))
                    {
                        edge.Add(p);
                    }
                }
            }
        }
    }
}
