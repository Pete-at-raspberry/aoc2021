using System;
using System.Collections.Generic;
using System.IO;

namespace _03
{
    internal class Program
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
            using (StreamReader inputRdr = new StreamReader("d:\\adventofcode\\AOC2022\\03\\input.txt"))
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
        }

        static void Part2()
        {
            Console.WriteLine("Part2");
        }
    }
}
