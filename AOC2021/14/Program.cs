using System;
using System.Collections.Generic;
using System.IO;

namespace _14
{
    class Program
    {
        static Dictionary<String, Char> rules = new Dictionary<string, char>();
        static Dictionary<String, long> poly = new Dictionary<string, long>();

        static void Main(string[] args)
        {
            ReadInput();
            Part1();
            //Part2();
        }

        static void ReadInput()
        {
            using (StreamReader inputRdr = new StreamReader("d:\\adventofcode\\AOC2021\\14\\input.txt"))
            {
                // Read the start state
                String line = inputRdr.ReadLine();
                for (int i = 0; i < line.Length - 1; ++i)
                {
                    String pair = line.Substring(i, 2);
                    AddToPair(poly, pair, 1);
                }
                inputRdr.ReadLine();

                // Now the rules
                while (!inputRdr.EndOfStream)
                {
                    line = inputRdr.ReadLine();
                    String pair = line.Substring(0, 2);
                    char insert = line.Substring(6)[0];
                    rules.Add(pair, insert);
                }
            }
        }

        private static void Part1()
        {
            Console.WriteLine("Part 1");
            for (int steps=0; steps<40; ++steps)
            {
                Dictionary<string, long> newList = new Dictionary<string, long>();
                foreach (String pair in poly.Keys)
                {
                    if (poly[pair] > 0 && rules.ContainsKey(pair))
                    {
                        Char insert = rules[pair];
                        long brokenPairs = poly[pair];

                        // New pairs come from rules where first+new, new+last.
                        Char[] p1 = new char[2];
                        p1[0] = pair[0];
                        p1[1] = insert;
                        String np1 = new string(p1);
                        AddToPair(newList, np1, brokenPairs);

                        Char[] p2 = new char[2];
                        p2[0] = insert;
                        p2[1] = pair[1];
                        String np2 = new string(p2);
                        AddToPair(newList, np2, brokenPairs);
                    }
                }

                // Swap the lists. 
                poly = newList;
            }

            // Now count. 
            Dictionary<Char, long> counts = new Dictionary<char, long>();
            bool first = true;
            foreach (KeyValuePair<String, long> pair in poly)
            {
                if (first)
                {
                    first = false;
                    counts.Add(pair.Key[0], pair.Value);
                }
                Char element = pair.Key[1];
                if (counts.ContainsKey(element))
                {
                    counts[element] += pair.Value;
                }
                else
                {
                    counts.Add(element, pair.Value);
                }
            }

            long most = 0, least = 10000000000000;
            foreach(long val in counts.Values)
            {
                if (most < val)
                    most = val;
                if (least > val && val != 0)
                    least = val;
            }
            Console.WriteLine($"difference between most and least is {most - least}");
        }

        private static void AddToPair(Dictionary<String, long> newList, String pair, long value)
        {
            if (!newList.ContainsKey(pair))
            {
                newList.Add(pair, value);
            }
            else
            {
                newList[pair] += value;
            }
        }
    }
}
