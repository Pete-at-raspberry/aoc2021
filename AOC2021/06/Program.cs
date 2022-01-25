using System;
using System.Collections.Generic;
using System.IO;

namespace _06
{
    class Program
    {
    
        static long[] fishBuffer = new long[7];
        static int day = 0;

        static void Main(string[] args)
        {
            ReadInput();
            Part1();
            //Part2();
        }

        static void ReadInput()
        {
            using (StreamReader inputRdr = new StreamReader("d:\\adventofcode\\AOC2021\\06\\input.txt"))
            {
                while (!inputRdr.EndOfStream)
                {
                    String line = inputRdr.ReadLine();
                    //line = "3,4,3,1,2";
                    String[] words = line.Split(',');
                    foreach(String word in words)
                    {
                        int i = Int32.Parse(word);
                        fishBuffer[i]++;
                    }
                }
            }
        }

        static void Part1()
        {
            long day8 = 0, day7 = 0;
            do
            {
                // Increment the day. 
                ++day;

                int day6index = (day + 6) % 7;
                long spawned = fishBuffer[day6index];

                // Add in the previous. 
                fishBuffer[day6index] += day7;
                day7 = day8;
                day8 = spawned;
            } while (day < 256  /*80*/);

            long total = day7 + day8;
            for (int i = 0; i < 7; ++i)
                total += fishBuffer[i];
            Console.WriteLine($"Total lanternfish = {total}");
        }
    }
}
