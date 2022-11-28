using System;
using System.Collections.Generic;
using System.IO;

namespace _01
{
    class Program
    {
        static List<Int32> input = new List<Int32>();

        static void Main(string[] args)
        {
            ReadInput();
            Part1();
            Part2();
        }

        static void ReadInput()
        {
            using (StreamReader inputRdr = new StreamReader("d:\\adventofcode\\AOC2022\\01\\input.txt"))
            {
                while (!inputRdr.EndOfStream)
                {
                    String line = inputRdr.ReadLine();
                    input.Add(Int32.Parse(line));
                }
            }
        }

        static void Part1()
        {
            Console.WriteLine("Part1");
            int up = 0;
            int down = 0;
            for (int n = 0; n < input.Count - 1; ++n)
            {
                if (input[n] > input[n + 1])
                    down++;
                else if (input[n] < input[n + 1])
                    up++;
            }

            Console.WriteLine($"Increased {up} times");
        }

        static void Part2()
        {
            Console.WriteLine("Part2");
            int up = 0;
            int down = 0;
            for (int n = 0; n < input.Count - 3; ++n)
            {
                Int32 window1 = input[n] + input[n + 1] + input[n + 2];
                Int32 window2 = input[n + 1] + input[n + 2] + input[n + 3];

                if (window1 > window2)
                    down++;
                else if (window1 < window2)
                    up++;
            }

            Console.WriteLine($"Increased {up} times");
        }
    }
}
