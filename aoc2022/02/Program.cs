using System;
using System.Collections.Generic;
using System.IO;

namespace _02
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
            using (StreamReader inputRdr = new StreamReader("d:\\adventofcode\\AOC2022\\02\\input.txt"))
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
            foreach (String line in input)
            {
                sum += Score(line);
            }
            Console.WriteLine($"Score: {sum}");
        }

        static int Score(String line)
        {
            int sc = 0;
            Char elf = line[0];
            Char me = GetChar(line[2]);

            // Score for the choice of r/p/s
            sc = me - 'A' + 1;
            if (elf == me)
            {
                sc += 3;
            }
            else if ((me == 'A' && elf == 'C') || (me == 'B' && elf == 'A') || (me == 'C' && elf == 'B'))
            {
                sc += 6;
            }
            return sc;
        }

        static Char GetChar(Char ch)
        {
            switch (ch)
            {
                case 'X': return 'A';
                case 'Y': return 'B';
                case 'Z': return 'C';
            }
            return ' ';
        }

        static void Part2()
        {
            Console.WriteLine("Part2");
            int sum = 0;
            foreach (String line in input)
            {
                sum += Score2(line);
            }
            Console.WriteLine($"Score: {sum}");
        }

        static int Score2(String line)
        {
            int sc = 0;
            Char elf = line[0];
            Char me = ' ';
            switch (line[2])
            {
                case 'X': // lose
                    if (elf == 'A') me = 'C';
                    else if (elf == 'B') me = 'A';
                    else me = 'B';
                    break;
                case 'Y': //draw
                    me = elf; break;  
                case 'Z': // win
                    if (elf == 'A') me = 'B';
                    else if (elf == 'B') me = 'C';
                    else me = 'A';
                    break;
            }

            // Score for the choice of r/p/s
            sc = me - 'A' + 1;
            if (elf == me)
            {
                sc += 3;
            }
            else if ((me == 'A' && elf == 'C') || (me == 'B' && elf == 'A') || (me == 'C' && elf == 'B'))
            {
                sc += 6;
            }
            return sc;
        }
    }
}
