using System;

namespace _21
{
    class Program
    {
        // Test input.
        //static int p1Start = 4;
        //static int p2Start = 8;
        // Real input
        static int p1Start = 8;
        static int p2Start = 5;

        static void Main(string[] args)
        {
            //Part1();
            Part2();
        }

        private static int[] threeRolls = new int[27];

        private static void Part2()
        {
            // Work out how many ways there are to roll a 3-sided die 3 times.
            int idx = 0;
            for (int p=1; p <= 3; ++p)
            {
                for (int q = 1; q <= 3; ++q)
                {
                    for (int r = 1; r <= 3; ++r)
                    {
                        threeRolls[idx++] = p + q + r;
                    }
                }
            }

            // Find out how many moves (all combinations) are required to win from a specific starting point. 
            long[] p1WinCounts = new long[20];
            long[] p2WinCounts = new long[20];
            long[] p1MoveCounts = new long[20];
            long[] p2MoveCounts = new long[20];
            FindAllMoveCounts(0, p1Start - 1, 0, p1MoveCounts, p1WinCounts);
            FindAllMoveCounts(0, p2Start - 1, 0, p2MoveCounts, p2WinCounts);

            // Find the maximum number of moves. 
            int maxMoves = 2;
            do
            {
                maxMoves++;
            } while (p1WinCounts[maxMoves] > 0 || p2WinCounts[maxMoves] > 0);

            // Now 'play' and see how many universes we had at each point. 
            long p1LastUnis = 1;
            long p2LastUnis = 1;
            int move = 0;
            long p1Unis = 0;
            long p2Unis = 0;
            do
            {
                // P1 moves. 
                if (p1WinCounts[move] > 0)
                {
                    p1Unis += p1WinCounts[move] * p2LastUnis;
                }
                p1LastUnis = p1MoveCounts[move];

                // P2 moves.
                if (p2WinCounts[move] > 0)
                {
                    p2Unis += p2WinCounts[move] * p1LastUnis;
                }
                p2LastUnis = p2MoveCounts[move];

                ++move;
            } while (move < maxMoves);

            Console.WriteLine($"P1 won in {p1Unis}");
            Console.WriteLine($"P2 won in {p2Unis}");
        }


        private static void FindAllMoveCounts(int move, int pos, int score, long[] moveCounts, long[] winCounts)
        {
            for (int s = 0; s<27; ++s)
            {
                int newPos = (pos + threeRolls[s]) % 10;
                int newScore = newPos + 1;

                // Done enough? 
                if (score + newScore >= 21)
                {
                    // Add this to the set of winning move counts. 
                    winCounts[move]++;
                }
                else
                {
                    // keep going. 
                    moveCounts[move]++;
                    FindAllMoveCounts(move + 1, newPos, score + newScore, moveCounts, winCounts);
                }
            }
        }

        private static void Part1()
        {
            int p1Score = 0;
            int p1Pos = p1Start - 1;
            int p2Score = 0;
            int p2Pos = p2Start - 1;
            bool p1Turn = true;
            do
            {
                int roll3 = RollDie() + RollDie() + RollDie();
                if (p1Turn)
                {
                    p1Pos = (p1Pos + roll3) % 10;
                    p1Score += p1Pos + 1;
                }
                else
                {
                    p2Pos = (p2Pos + roll3) % 10;
                    p2Score += p2Pos + 1;
                }
                p1Turn = !p1Turn;
            } while (p1Score < 1000 && p2Score < 1000);
            int losingScore;
            if (p1Score > p2Score)
            {
                Console.WriteLine("P1 wins!");
                losingScore = p2Score;
            }
            else
            {
                Console.WriteLine("P2 wins!");
                losingScore = p1Score;
            }

            // Die was rolled as many times as the counter says (-1)
            Console.WriteLine($"loser is {losingScore}, rolls is {dieVal - 1}, Total is {losingScore * (dieVal - 1)}");
        }

        static int dieVal = 1;
        private static int RollDie()
        {
            return dieVal++;
        }
    }
}
