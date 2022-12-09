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
            int score = 0;
            foreach(string line in input)
            {
                string bag1 = line.Substring(0, line.Length / 2);
                string bag2 = line.Substring(line.Length / 2);
                foreach (Char c in bag1)
                {
                    if (bag2.Contains(c))
                    {
                        score += GetScore(c);
                        break;
                    }
                }

            }
            Console.WriteLine($"Total score {score}");
        }

        static int GetScore(char c)
        {
            int sc;
            if (c >= 'a' && c <= 'z')
                sc = c - 'a' + 1;
            else
                sc = c - 'A' + 27;
            return sc;
        }

        static void Part2()
        {
            Console.WriteLine("Part2");
            int score = 0;
            for (int i = 0; i < input.Count; i += 3)
            {
                string bag1 = input[i];
                string bag2 = input[i+1];
                string bag3 = input[i+2];
                foreach (Char c in bag1)
                {
                    if (bag2.Contains(c) && bag3.Contains(c))
                    {
                        score += GetScore(c);
                        break;
                    }
                }

            }
            Console.WriteLine($"Total score {score}");


        }
    }
}
