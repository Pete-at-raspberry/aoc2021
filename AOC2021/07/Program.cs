using System;
using System.Collections.Generic;
using System.IO;

namespace _07
{
    class Program
    {
        static List<int> positions = new List<int>();
        static void Main(string[] args)
        {
            {
                ReadInput();
                //Part1();
                Part2();
            }

            static void ReadInput()
            {
                using (StreamReader inputRdr = new StreamReader("d:\\adventofcode\\AOC2021\\07\\input.txt"))
                {
                    while (!inputRdr.EndOfStream)
                    {
                        String line = inputRdr.ReadLine();
                        //line = "16,1,2,0,4,2,7,1,2,14";
                        String[] words = line.Split(',');
                        foreach (String word in words)
                        {
                            int i = Int32.Parse(word);
                            positions.Add(i);
                        }
                    }
                }
            }

            static void Part1()
            {
                int median = GetMedian();
                int score = GetScore(median);
                Console.WriteLine($"Score is {score}");

            }

            static void Part2()
            {
                int median = GetMedian();
                int medScore = GetNewScore(median);
                int medPlus = GetNewScore(median + 1);

                int increment = 1;
                if (medScore < medPlus)
                    increment = -1;
                int lastScore = medScore;
                int value = median;
                do
                {
                    value += increment;
                    int newScore = GetNewScore(value);
                    if (newScore > lastScore)
                        break;
                    lastScore = newScore;
                } while (true);
                Console.WriteLine($"Best Score is {lastScore}");
            }

            static int GetNewScore(int value)
            {
                int score = 0;
                foreach (int pos in positions)
                {
                    int n = Math.Abs(pos - value);
                    score += (n * (n + 1)) / 2;
                }
                return score;
            }

            static int GetMedian()
            {
                // Find the median value. 
                positions.Sort();
                int medianIdxLo = positions.Count / 2;
                int medianIdxHi = (positions.Count + 1) / 2;
                return (positions[medianIdxLo] + positions[medianIdxHi]) / 2;
            }

            static int GetScore(int value)
            {
                int score = 0;
                foreach(int pos in positions)
                {
                    score += Math.Abs(pos - value);
                }
                return score;
            }
        } 
            
    }
}
