using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;

namespace _01
{
    class Program
    {
        static List<String> input = new List<String>();

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
                    input.Add(line);
                }
            }
        }

        static void Part1()
        {
            Console.WriteLine("Part1");
            int sum = 0;
            int max = 0;
            for (int n = 0; n < input.Count - 1; ++n)
            {
                if (input[n].Length == 0)
                {
                    if (max < sum)
                        max = sum;
                    sum = 0;
                }
                else
                {
                    sum = sum + Int32.Parse(input[n]);
                }
            }

            Console.WriteLine($"Max was {max}");
        }

        static void Part2()
        {
            Console.WriteLine("Part2");
            int sum = 0;
            List<Int32> sums = new List<int>();
            for (int n = 0; n < input.Count - 1; ++n)
            {
                if (input[n].Length == 0)
                {
                    sums.Add(sum);
                    sum = 0;
                }
                else
                {
                    sum = sum + Int32.Parse(input[n]);
                }
            }

            // Find the top ones. 
            sums.Sort();
            int m1 = sums[sums.Count - 1];
            int m2 = sums[sums.Count - 2];
            int m3 = sums[sums.Count - 3];
            Console.WriteLine($"Max was {m1}, {m2}, {m3} = {m1+m2+m3}");
        }
    }
}
