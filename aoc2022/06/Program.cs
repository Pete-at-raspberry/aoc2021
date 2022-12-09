using System;
using System.IO;

namespace _06
{
    internal class Program
    {
        static String input = "";

        static void Main(string[] args)
        {
            ReadInput();
            Part1();
            Part2();
        }

        static void ReadInput()
        {
            using (StreamReader inputRdr = new StreamReader("d:\\adventofcode\\AOC2022\\06\\input.txt"))
            {
                while (!inputRdr.EndOfStream)
                {
                    String line = inputRdr.ReadLine();
                    input += line;
                }
            }
        }

        static void Part1()
        {
            Console.WriteLine("Part1");
            int packetStart = -4;
            for (int i=0; i<input.Length - 4; ++i)
            {
                char a = input[i];
                char b = input[i + 1];
                char c = input[i + 2];
                char d = input[i + 3];
                if (a != b && a != c && a != d && b != c && b != d && c != d)
                {
                    packetStart = i + 4;
                    break;
                }
            }
            Console.WriteLine($"Start of packet {packetStart}");
        }

        static void Part2()
        {
            Console.WriteLine("Part1");
            int packetStart = -4;
            for (int i = 0; i < input.Length - 14; ++i)
            {
                bool same = false;
                for (int j=i; j<i+13; ++j)
                {
                    for (int k=j+1; k<i+14; ++k)
                    {
                        if (input[j] == input[k])
                        {
                            same = true;
                            goto doneLoop;
                        }
                    }
                }
                doneLoop:
                if (!same)
                {
                    packetStart = i + 14;
                    break;
                }
            }
            Console.WriteLine($"Start of packet {packetStart}");
        }

    }
}
