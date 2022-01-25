using System;
using System.Collections.Generic;
using System.IO;

namespace _10
{
    class Program
    {
        static String openers = "{[(<";

        static void Main(string[] args)
        {
            //Part1();
            Part2();
        }

        static void Part1()
        {
            int score = 0;
            // Parse the file line by line.
            using (StreamReader inputRdr = new StreamReader("d:\\adventofcode\\AOC2021\\10\\input.txt"))
            {
                while (!inputRdr.EndOfStream)
                {
                    String line = inputRdr.ReadLine();
                    score += ParseLine(line);
                }
            }
            Console.WriteLine($"Part 1 score is {score}");
        }

        static void Part2()
        {
            List<long> scores = new List<long>();

            // Parse the file line by line.
            using (StreamReader inputRdr = new StreamReader("d:\\adventofcode\\AOC2021\\10\\input.txt"))
            {
                while (!inputRdr.EndOfStream)
                {
                    String line = inputRdr.ReadLine();
                    long score = Parse2Line(line);
                    if (score != 0)
                    {
                        scores.Add(score);
                    }
                }
            }

            scores.Sort();
            long final = scores[scores.Count / 2];
            Console.WriteLine($"Part 2 score is {final}");
        }

        private static int ParseLine(String line)
        {
            // Create a stack of the expected closing codes. 
            Stack<char> tokenStack = new Stack<char>();

            foreach (char c in line.ToCharArray())
            {
                // If this is an open symbol, then add to the stack. 
                if (openers.Contains(c))
                {
                    tokenStack.Push(c);
                }
                else
                {
                    // must be closing a token. 
                    char expected = GetClosingToken(tokenStack.Peek());
                    if (expected == c)
                    {
                        // Fine, this matches. 
                        tokenStack.Pop();
                    }
                    else
                    {
                        // not right. 
                        return GetScoreForToken(c);
                    }
                }
            }
            return 0;
        }

        private static long Parse2Line(String line)
        {
            // Create a stack of the expected closing codes. 
            Stack<char> tokenStack = new Stack<char>();

            foreach (char c in line.ToCharArray())
            {
                // If this is an open symbol, then add to the stack. 
                if (openers.Contains(c))
                {
                    tokenStack.Push(c);
                }
                else
                {
                    // must be closing a token. 
                    char expected = GetClosingToken(tokenStack.Peek());
                    if (expected == c)
                    {
                        // Fine, this matches. 
                        tokenStack.Pop();
                    }
                    else
                    {
                        // not right. Ignore corruption.
                        return 0;
                    }
                }
            }

            // All done. Any missing? 
            long score = 0;
            while (tokenStack.Count > 0)
            {
                score *= 5;
                char c = GetClosingToken(tokenStack.Pop());
                score += GetToken2Score(c);
            }
            return score;
        }

        private static int GetToken2Score(char c)
        {
            switch (c)
            {
                case ')': return 1;
                case ']': return 2;
                case '}': return 3;
                case '>': return 4;
            }
            return 0;
        }

        private static char GetClosingToken(char open)
        {
            switch (open)
            {
                case '(': return ')';
                case '[': return ']';
                case '{': return '}';
                case '<': return '>';
            }
            return ' ';
        }

        private static int GetScoreForToken(char c)
        {
            switch (c)
            {
                case ')': return 3;
                case ']': return 57;
                case '}': return 1197;
                case '>': return 25137;
            }
            return 0;
        }
    }
}
