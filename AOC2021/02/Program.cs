using System;
using System.Collections.Generic;
using System.IO;

namespace _02
{
    class Program
    {
        static List<String> input = new List<String>();

        static void Main(string[] args)
        {
            ReadInput();
            Part1();
            //Part2();
        }

        static void ReadInput()
        {
            using (StreamReader inputRdr = new StreamReader("d:\\adventofcode\\AOC2021\\02\\input.txt"))
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
            Int32 xpos = 0;
            Int32 ypos = 0;
            Int32 aim = 0;

            foreach(String line in input)
            {
                String[] words = line.Split(' ');

                Int32 number = Int32.Parse(words[1]);

                // Do the right thing by the keyword
                switch (words[0])
                {
                    case "forward":
                        xpos += number;
                        ypos += aim * number;
                        break;
                    case "up":
                        aim -= number;
                        break;
                    case "down":
                        aim += number;
                        break;

                }
            }

            Console.WriteLine($"The answer is {xpos}*{ypos}  {xpos*ypos}");
        }
    }
}
