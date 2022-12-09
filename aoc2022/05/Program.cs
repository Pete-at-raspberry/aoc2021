using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;

namespace _05
{
    internal class Program
    {
        static List<String> input = new List<String>();
        static List<List<char>> stacks = new List<List<char>>();

        static void Main(string[] args)
        {
            ReadInput();
            //Part1();
            Part2();
        }

        static void ReadInput()
        {
            using (StreamReader inputRdr = new StreamReader("d:\\adventofcode\\AOC2022\\05\\input.txt"))
            {
                while (!inputRdr.EndOfStream)
                {
                    String line = inputRdr.ReadLine();
                    // Where are we
                    if (line.Contains('['))
                    {
                        int lastCol = 0;
                        while (true)
                        {
                            int pos = line.IndexOf('[', lastCol);
                            if (pos < 0)
                                break;
                            // Work out which stack we are on here. 
                            int stackNum = pos / 4 + 1;
                            while (stacks.Count <= stackNum)
                                stacks.Add(new List<char>());
                            lastCol = pos + 1;
                            stacks[stackNum].Insert(0, line[lastCol]);
                        }
                    }
                    else if (line.StartsWith("move"))
                    {
                        input.Add(line);
                    }
                }
            }
        }

        static void Part1()
        {
            Console.WriteLine("Part1");

            foreach(string line in input)
            {
                int from, to, count;
                count = ParseLine(line, out from, out to);
                for (int i = 0; i < count; i++)
                {
                    List<char> fromStack = stacks[from];
                    List<char> toStack = stacks[to];
                    char crate = fromStack[fromStack.Count - 1];
                    fromStack.RemoveAt(fromStack.Count - 1);
                    toStack.Add(crate);
                }
            }

            // Work out the top row. 
            String topRow = "";
            foreach (List<char> stack in stacks)
            {
                if (stack.Count > 0)
                    topRow += stack[stack.Count - 1];
            }
            Console.WriteLine(topRow);
        }

        static void Part2()
        {
            Console.WriteLine("Part2");

            foreach (string line in input)
            {
                int from, to, count;
                count = ParseLine(line, out from, out to);
                List<char> fromStack = stacks[from];
                List<char> toStack = stacks[to];
                toStack.AddRange(fromStack.GetRange(fromStack.Count - count, count));
                fromStack.RemoveRange(fromStack.Count - count, count);
            }

            // Work out the top row. 
            String topRow = "";
            foreach (List<char> stack in stacks)
            {
                if (stack.Count > 0)
                    topRow += stack[stack.Count - 1];
            }
            Console.WriteLine($"Part2: {topRow}");
        }


        static int ParseLine(String line, out int from, out int to)
        {
            // move 1 from 2 to 1
            String[] bits = line.Split(' ');
            from = Int32.Parse(bits[3]);
            to = Int32.Parse(bits[5]);
            return Int32.Parse(bits[1]);
        }
    }
}
