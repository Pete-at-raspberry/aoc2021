using System;
using System.Collections.Generic;
using System.IO;

namespace _03
{
    class Program
    {
        static List<String> input = new List<String>();

        static void Main(string[] args)
        {
            ReadInput();
            //Part1();
            Part2();
        }

        static void ReadInput()
        {
            using (StreamReader inputRdr = new StreamReader("d:\\adventofcode\\AOC2021\\03\\input.txt"))
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

            // How many bits? 
            Int32 numbits = input[0].Length;

            Int32[] bitCount = new Int32[numbits];

            foreach(String line in input)
            {
                for (int pos=0; pos<numbits; ++pos)
                {
                    if (line[pos] == '1')
                    {
                        bitCount[numbits - 1 - pos] = bitCount[numbits - 1 - pos] + 1;
                    }
                }
            }

            // Gamma is the most popluar, Epsilon the least
            Int32 gamma = 0;
            Int32 epsilon = 0;

            Int32 halfWay = input.Count / 2;
            for (int n = 0; n < numbits; n++)
            {
                if (bitCount[n] > halfWay)
                    gamma += 1 << n;
                else
                    epsilon += 1 << n;
            }
            Console.WriteLine($"gamma * epsilon is {gamma} * {epsilon} = {gamma*epsilon}");

        }

        private static void Part2()
        {
            Console.WriteLine("Part1");

            // How many bits? 
            Int32 numbits = input[0].Length;

            List<String> theList = input;

            for (int n = 0; n<numbits; n++)
            {
                int countOf1s = Count1s(theList, n);
                char bitToKeep = '0';
                if (countOf1s >= theList.Count - countOf1s)
                {
                    bitToKeep = '1';
                }
                theList = KeepOnlyThoseOnes(theList, n, bitToKeep);

                // Got to the end? 
                if (theList.Count == 1)
                    break;
            }

            int oxygen = ParseBool(theList[0]);

            theList = input;
            for (int n = 0; n < numbits; n++)
            {
                int countOf1s = Count1s(theList, n);
                char bitToKeep = '0';
                if (countOf1s < theList.Count - countOf1s)
                {
                    bitToKeep = '1';
                }
                theList = KeepOnlyThoseOnes(theList, n, bitToKeep);

                // Got to the end? 
                if (theList.Count == 1)
                    break;
            }

            int scrubber = ParseBool(theList[0]);

            Console.WriteLine($"oxygen = {oxygen}, scrubber = {scrubber}, so {oxygen * scrubber}");
        }

        private static int Count1s(List<String> theList, int pos)
        {
            int cnt = 0;
            foreach (String line in theList)
            {
                if (line[pos] == '1')
                    cnt++;
            }
            return cnt;
        }

        private static List<String> KeepOnlyThoseOnes(List<String> theList, int pos, char val)
        {
            List<String> newList = new List<String>();
            foreach(String line in theList)
            {
                if (line[pos] == val)
                    newList.Add(line);
            }
            return newList;
        }

        private static Int32 ParseBool(String bValue)
        {
            Int32 res = 0;
            foreach (char c in bValue)
            {
                res <<= 1;
                if (c == '1')
                {
                    res = res + 1;
                }
            }
            return res;
        }
    }
}
