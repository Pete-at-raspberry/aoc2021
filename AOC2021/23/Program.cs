using System;

namespace _23
{
    class Program
    {
        /* Starting positions: Just represent as Strings. 
         * Example
         *  #############
            #...........#
            ###B#C#B#D###
              #A#D#C#A#
              #########

        * My Input
        * #############
          #...........#
          ###B#C#A#B###
            #C#D#D#A#
            #########
        */
        static String input1 = "BCBD";
        static String input2 = "ADCA";
        //static String input1 = "BCAB";
        //static String input2 = "CDDA";


        static void Main(string[] args)
        {
            Console.WriteLine("Start day 23");
            Board bd = SetupBoard();
            bd.Print();
            // Play...
            bd.TryAllMoves();

            Console.WriteLine($"Minimum energy is {Board.minEnergy}");
        }

        static private Board SetupBoard()
        {
            // Initialise from the input strings.
            return new Board(input1, input2);
        }
    }

    public class Board
    {
        public static int minEnergy = 0;
        char[] hall = new char[11];
        char[] roomA = new char[2];
        char[] roomB = new char[2];
        char[] roomC = new char[2];
        char[] roomD = new char[2];
        int energy = 0;

        public Board(String line1, String line2)
        {
            for (int i = 0; i < hall.Length; ++i)
                hall[i] = '.';

            // Set the initial state in the rooms. 
            roomA[0] = line1[0];
            roomB[0] = line1[1];
            roomC[0] = line1[2];
            roomD[0] = line1[3];
            roomA[1] = line2[0];
            roomB[1] = line2[1];
            roomC[1] = line2[2];
            roomD[1] = line2[3];

            energy = 0;
        }

        public void TryAllMoves()
        {
            // Enumerate all the moves

        }

        public void Print()
        {
            Console.WriteLine();
            Console.WriteLine("#############");
            Console.Write("#");
            foreach (char c in hall)
                Console.Write(c);
            Console.WriteLine("#");
            Console.WriteLine($"###{roomA[0]}#{roomB[0]}#{roomC[0]}#{roomD[0]}###");
            Console.WriteLine($"  #{roomA[1]}#{roomB[1]}#{roomC[1]}#{roomD[1]}#");
            Console.WriteLine("  #########");
        }
    }
}
