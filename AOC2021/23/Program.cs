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
            Board bd = SetupBoard();

            // Play...
        }

        static private Board SetupBoard()
        {
            // Initialise from the input strings.
        }
    }

    public class Board
    {

    }
}
