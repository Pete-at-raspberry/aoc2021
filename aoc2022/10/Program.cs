using System;
using System.Collections.Generic;
using System.IO;

namespace _10
{
    internal class Program
    { 
        static List<String> input = new List<String>();
        static List<Tuple<int, int>> result = new List<Tuple<int, int>>();

        static void Main(string[] args)
        {
            ReadInput();
            Run();
            Part1();
            Part2();
        }

        static void ReadInput()
        {
            using (StreamReader inputRdr = new StreamReader("d:\\adventofcode\\AOC2022\\10\\input.txt"))
            {
                while (!inputRdr.EndOfStream)
                {
                    String line = inputRdr.ReadLine();
                    input.Add(line);
                }
            }
        }

        static void Run()
        {
            int clock = 0;
            int x = 1;
            result.Add(new Tuple<int, int>(clock, x));
            foreach(string line in input)
            {
                if (line == "noop")
                {
                    ++clock;
                }
                else
                {
                    // addx 13
                    clock += 2;
                    x += Int32.Parse(line.Substring(5));

                    // Only need to record changes in x
                    result.Add(new Tuple<int, int>(clock, x));
                }
            }
        }

        static void Part1()
        {
            Console.WriteLine("Part1");
            int total = 0;
            int recordPoint = 20;
            int x = 1;
            foreach(Tuple<int, int> record in result)
            {
                if (record.Item1 >= recordPoint)
                {
                    //if (record.Item1 == recordPoint)
                    //{
                    //    x = record.Item2;
                    //}
                    total += recordPoint * x;
                    recordPoint += 40;
                    if (recordPoint > 220)
                        break;
                }
                x = record.Item2;
            }

            Console.WriteLine($"Total: {total}");
        }

        static void Part2()
        {
            int x = 1;
            int resCount = 0;
            for (int cycle = 1; cycle < 241; ++cycle)
            {
                int pixel_x = (cycle - 1) % 40;
                if (resCount < result.Count&& result[resCount].Item1 < cycle)
                {
                    x = result[resCount++].Item2;
                }
                if (pixel_x == 0)
                    Console.WriteLine();
                if (pixel_x >= x - 1 && pixel_x <= x + 1)
                    Console.Write('#');
                else
                    Console.Write('.');
            }

        }
    }
}
