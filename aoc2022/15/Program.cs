using System;
using System.Collections.Generic;
using System.IO;

namespace _15
{
    internal class Program
    {
        static List<String> input = new List<String>();
        static List<Sensor> sensors = new List<Sensor>();

        static void Main(string[] args)
        {
            ReadInput();
            //Part1();
            Part2();
        }

        static void ReadInput()
        {
            using (StreamReader inputRdr = new StreamReader("d:\\adventofcode\\AOC2022\\15\\input.txt"))
            {
                while (!inputRdr.EndOfStream)
                {
                    String line = inputRdr.ReadLine();
                    input.Add(line);
                    int x, y, bx, by;
                    String[] bits = line.Split(' ');
                    x = int.Parse(bits[2].Substring(2, bits[2].Length - 3));
                    y = int.Parse(bits[3].Substring(2, bits[3].Length - 3));
                    bx = int.Parse(bits[8].Substring(2, bits[8].Length - 3));
                    by = int.Parse(bits[9].Substring(2));
                    sensors.Add(new Sensor(x, y, bx, by));
                }
            }
        }

        static void Part1()
        {
            Console.WriteLine("Part1");
            int yPos = 2000000;
            SortedSet<int> badPos = new SortedSet<int>();
            foreach(Sensor s in sensors)
            {
                foreach (int x in s.GetRowUse(yPos))
                {
                    if (!badPos.Contains(x))
                        badPos.Add(x);
                }
            }
            foreach(Sensor s in sensors)
            {
                if (s.by == yPos)
                    badPos.Remove(s.bx);
            }
            Console.WriteLine($"total positions: {badPos.Count}");

        }

        static void Part2()
        {
            Console.WriteLine("Part2");
            
            // Go round each sensor one away from its edge, and test each point against the other sensors. 
            foreach(Sensor s in sensors)
            {
                for (int x = s.x - s.dist - 1, y = s.y; x < s.x; x++, y--)
                {
                    if (TryPoint(s, x, y))
                    {
                        // That's it!
                        Console.WriteLine($"x: {x}, y: {y}, tuning frequency: {x * (Int64)4000000 + y}");
                    }
                }
                for (int x = s.x, y = s.y - s.dist -1; y < s.y; x++, y++)
                {
                    if (TryPoint(s, x, y))
                    {
                        // That's it!
                        Console.WriteLine($"x: {x}, y: {y}, tuning frequency: {(Int64)x * 4000000 + y}");
                    }
                }
                for (int x = s.x + s.dist + 1, y = s.y; x > s.x; x--, y++)
                {
                    if (TryPoint(s, x, y))
                    {
                        // That's it!
                        Console.WriteLine($"x: {x}, y: {y}, tuning frequency: {x * 4000000 + y}");
                    }
                }
                for (int x = s.x, y = s.y + s.dist + 1; y > s.y; x--, y--)
                {
                    if (TryPoint(s, x, y))
                    {
                        // That's it!
                        Console.WriteLine($"x: {x}, y: {y}, tuning frequency: {x * 4000000 + y}");
                    }
                }
            }
        }

        static bool TryPoint(Sensor stest, int x, int y)
        {
            int bound = 4000000;
            if (x < 0 || y < 0 || x > bound || y > bound)
                return false;
            foreach (Sensor s in sensors)
            {
                if (s == stest)
                    continue;

                if (s.Contains(x, y))
                    return false;
            }
            return true;
        }

        public class Sensor
        {
            public int x, y;
            public int bx, by;
            public int dist;

            public Sensor(int x, int y, int bx, int by)
            {
                this.x = x;
                this.y = y;
                this.bx = bx;
                this.by = by;
                dist = Math.Abs(x - bx) + Math.Abs(y - by);
            }

            public bool Contains(int x, int y)
            {
                bool inside = (Math.Abs(x - this.x) + Math.Abs(y - this.y) <= dist);
                return inside; // && !(x == bx && y == by);
            }

            public List<int> GetRowUse(int row)
            {
                List<int> rowUse = new List<int>();
                int yDiff = Math.Abs(row - y);
                if (yDiff <= dist)
                {
                    int xDiff = dist - yDiff;
                    for (int xx = x - xDiff; xx <= x + xDiff; xx++)
                    {
                        rowUse.Add(xx);
                    }
                }
                return rowUse;
            }
        }
    }
}
