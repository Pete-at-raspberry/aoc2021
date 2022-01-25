using System;
using System.Collections.Generic;
using System.IO;

namespace _04
{
    class Program
    {
        static List<int> drawValues = new List<int>();
        static List<Board> boards = new List<Board>(); 

        static void Main(string[] args)
        {
            ReadInput();

            //Part1();
            Part2();
        }

        private static void ReadInput()
        {
            List<String> lines = new List<string>();

            using (StreamReader inputRdr = new StreamReader("d:\\adventofcode\\AOC2021\\04\\input.txt"))
            {
                while (!inputRdr.EndOfStream)
                {
                    String line = inputRdr.ReadLine();
                    lines.Add(line);
                }
            }

            // First line is the input. 
            String[] words = lines[0].Split(',');
            foreach (String word in words)
            {
                drawValues.Add(Int32.Parse(word));
            }

            // Rest is boards. 
            Board bd = null;
            for (int ln = 1; ln < lines.Count; ++ln)
            {
                if (ln % 6 == 1)
                {
                    bd = new Board();
                    boards.Add(bd);
                }
                else
                {
                    bd.AddLine(lines[ln]);
                }
            }
        }

        private static void Part1()
        {
            Console.WriteLine("Part 1");

            // Play the game
            foreach(int draw in drawValues)
            {
                // Add to the boards
                foreach(Board b in boards)
                {
                    b.SetUsed(draw);

                    // Assume only one winner
                    if (b.IsDone())
                    {
                        Console.WriteLine("Done!");
                        int boardValue = b.GetValue();
                        Console.WriteLine($"Board value {boardValue}, draw {draw}, answer is {boardValue * draw}");
                        return;
                    }
                }


            }
        }

        private static void Part2()
        {
            Console.WriteLine("Part 2");
            int doneCount;
            Board lastBoard = null;

            // Play the game
            foreach (int draw in drawValues)
            {
                doneCount = 0;

                // Add to the boards
                foreach (Board b in boards)
                {
                    if (!b.IsDone())
                    {
                        b.SetUsed(draw);
                        if (doneCount++ == 0)
                        {
                            lastBoard = b;
                        }
                    }
                }

                if (doneCount == 1 && lastBoard.IsDone())
                { 
                    Console.WriteLine("Done!");
                    int boardValue = lastBoard.GetValue();
                    Console.WriteLine($"Board value {boardValue}, draw {draw}, answer is {boardValue * draw}");
                    return;
                }

            }
        }
    }


    class Board
    {
        public int[,] numbers = new int[5,5];
        public bool[,] used = new bool[5, 5];
        public int numLines = 0;

        public Board()
        {
        }

        public void AddLine(String line)
        {
            String[] words = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            for (int y = 0; y < 5; ++y)
            {
                numbers[numLines, y] = Int32.Parse(words[y]);
                used[numLines, y] = false;
            }
            numLines++;
        }

        public void SetUsed(int number)
        {
            for (int x = 0; x<5; ++x)
            {
                for (int y=0; y<5; ++y)
                {
                    if (numbers[x,y] == number)
                    {
                        used[x, y] = true;
                    }
                }
            }
        }

        public bool IsDone()
        {
            // Done if any row or column is all done. 
            for (int x=0; x<5; ++x)
            {
                bool allDone = true;
                for (int y=0; y<5; ++y)
                {
                    if (!used[x,y])
                    {
                        allDone = false;
                        break;
                    }
                }
                if (allDone)
                    return true;
            }

            for (int y = 0; y < 5; ++y)
            {
                bool allDone = true;
                for (int x = 0; x < 5; ++x)
                {
                    if (!used[x, y])
                    {
                        allDone = false;
                        break;
                    }
                }
                if (allDone)
                    return true;
            }
            return false;
        }
        public int GetValue()
        {
            int score = 0;
            for (int y =0; y<5; ++y)
            {
                for (int x=0; x<5; ++x)
                {
                    if (!used[x,y])
                    {
                        score += numbers[x, y];
                    }
                }
            }
            return score;
        }
    }
}
