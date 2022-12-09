using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace _04
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
            using (StreamReader inputRdr = new StreamReader("d:\\adventofcode\\AOC2022\\04\\input.txt"))
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
            int count = 0;
            int count2 = 0;
            foreach(String line in input)
            {
                int al, ah, bl, bh;
                String[] twoparts = line.Split(',');
                GetLoHi(twoparts[0], out al, out ah);
                GetLoHi(twoparts[1], out bl, out bh);

                if ((al <= bl && ah >= bh) || (bl <= al && bh >= ah))
                    count++;

                if ((al <= bl && ah >= bl) || (al <= bh && ah >= bh)
                    || (bl <= al && bh >= al) || (bl <= ah && bh >= ah))
                    count2++;
            }

            Console.WriteLine($"Count of pairs {count}");
            Console.WriteLine($"Count2 of pairs {count2}");
        }

        static void GetLoHi(String s, out int l, out int h)
        {
            string[] numbers = s.Split('-');
            l = Int32.Parse(numbers[0]);
            h = Int32.Parse(numbers[1]);
        }

        static void Part2()
        {
            Console.WriteLine("Part2");
        }
    }
}
