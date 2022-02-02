using System;
using System.Collections.Generic;

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
        //static String input1 = "BCBD";
        //static String input2 = "ADCA";
        static String input1 = "BCAB";
        static String input2 = "CDDA";


        static void Main(string[] args)
        {
            Console.WriteLine("Start day 23");
            Board bd = SetupBoard();
            // Play...
            bd.TryAllMoves();

            foreach(Board b in Board.minBoards)
            {
                b.Print();
            }
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
        public static int minEnergy = Int32.MaxValue;
        public static List<Board> minBoards;
        char[] hall = new char[11];
        char[] roomA = new char[2];
        char[] roomB = new char[2];
        char[] roomC = new char[2];
        char[] roomD = new char[2];
        int energy = 0;

        // Just keep this to display the moves
        private static Stack<Board> boards = new Stack<Board>();

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

        public Board(Board b)
        {
            energy = b.energy;
            Array.Copy(b.hall, hall, hall.Length);
            Array.Copy(b.roomA, roomA, 2);
            Array.Copy(b.roomB, roomB, 2);
            Array.Copy(b.roomC, roomC, 2);
            Array.Copy(b.roomD, roomD, 2);
        }

        public void TryAllMoves()
        {
            boards.Push(this);

            // TEMP!!
            //Print();

            // Enumerate all the moves
            List<Move> allMoves = FindMoves();
            if (allMoves.Count == 0)
            {
                // See if we are done. 
                if (Finished())
                {
                    if (energy < minEnergy)
                    {
                        Console.WriteLine($"Found new min energy {energy}");
                        minEnergy = energy;
                        minBoards = new List<Board>(boards.ToArray());
                    }
                }
            }
            else
            {
                foreach (Move m in allMoves)
                {
                    Board newBd = ApplyMove(m);
                    newBd.TryAllMoves();
                }
            }
            boards.Pop();
        }

        private List<Move> FindMoves()
        {
            List<Move> allMoves = new List<Move>();
            for(int i=0; i<hall.Length; ++i)
            {
                if (hall[i] != '.')
                {
                    // Can only move to their destination. 
                    char p = hall[i];
                    char[] room = GetArrFromLoc(p);

                    // Only if the destination contains only those type
                    if ((room[0] == '.' && room[1] == p) || (room[0] == '.' && room[1] == '.'))
                    {
                        int entrance = GetRoomEntranceIdx(p);
                        int start = (entrance > i) ? i + 1 : i - 1; 
                        if (HallIsEmpty(start, entrance))
                        {
                            // All good. Allow this move. 
                            Move m = new Move();
                            m.fromIdx = i;
                            m.fromLoc = 'H';
                            m.toIdx = (room[1] == '.') ? 1 : 0;
                            m.toLoc = p;
                            m.FindEnergy(p);
                            allMoves.Add(m);
                        }
                    }
                }
            }

            // And now the rooms...
            for (char roomName = 'A'; roomName <= 'D'; ++roomName)
            {
                char[] room = GetArrFromLoc(roomName);
                int hallStart = GetRoomEntranceIdx(roomName);
                char piece = '.';
                int pos = 0;
                if (room[0] != '.')
                {
                    // Got a piece next to the hall
                    piece = room[0];
                }
                else if (room[1] != '.')
                {
                    // Got a piece next to the wall
                    piece = room[1];
                    pos = 1;
                }
                if (piece != '.')
                {
                    // Can't move if this is in the correct location and not blocking another
                    if (piece == roomName && (pos == 1 || room[1] == roomName))
                        continue;

                    // Try a home run first
                    if (piece != roomName)
                    {
                        int destPos = -1;
                        char[] destRoom = GetArrFromLoc(piece);
                        if (destRoom[0] == '.' && destRoom[1] == piece)
                        {
                            destPos = 0;
                        }
                        else if (destRoom[0] == '.' && destRoom[1] == '.')
                        {
                            destPos = 1;
                        }
                        if (destPos != -1)
                        {
                            // Is the hall clear? 
                            int end = GetRoomEntranceIdx(piece);
                            if (HallIsEmpty(hallStart, end))
                            {
                                Move m = new Move();
                                m.fromIdx = pos;
                                m.fromLoc = roomName;
                                m.toIdx = destPos;
                                m.toLoc = piece;
                                m.FindEnergy(piece);
                                allMoves.Add(m);

                                // Don't consider any other moves with this piece.
                                continue;
                            }
                        }
                    }

                    // Now consider moves along the hall
                    for (int h = 0; h<hall.Length; ++h)
                    {
                        if (h == 2 || h == 4 || h == 6 || h == 8)
                            continue;
                        if (HallIsEmpty(h, hallStart))
                        {
                            Move m = new Move();
                            m.fromLoc = roomName;
                            m.fromIdx = pos;
                            m.toLoc = 'H';
                            m.toIdx = h;
                            m.FindEnergy(piece);
                            allMoves.Add(m);
                        }
                    }
                }
            }

            // 
            return allMoves;
        }

        private bool HallIsEmpty(int s, int e)
        {
            for (int i=s; i!=e; i+=(e>s)?1:-1)
            {
                if (hall[i] != '.')
                    return false;
            }
            return true;
        }

        public static int GetRoomEntranceIdx(char rm)
        {
            switch (rm)
            {
                case 'A': return 2;
                case 'B': return 4;
                case 'C': return 6;
                case 'D': return 8;
                default:
                    break;
            }
            return -1;
        }

        private bool Finished()
        {
            return roomA[0] == 'A' && roomA[1] == 'A' && roomB[0] == 'B' && roomB[1] == 'B' && roomC[0] == 'C' && roomC[1] == 'C' && roomD[0] == 'D' && roomD[1] == 'D';
        }

        private Board ApplyMove(Move m)
        {
            Board b = new Board(this);
            b.energy += m.energy;
            char piece = GetPiece(m.fromLoc, m.fromIdx);
            b.SetPiece(m.fromLoc, m.fromIdx, '.');
            b.SetPiece(m.toLoc, m.toIdx, piece);
            return b;
        }

        private char GetPiece(char loc, int idx)
        {
            char[] arr = GetArrFromLoc(loc);
            return arr[idx];
        }

        private void SetPiece(char loc, int idx, char val)
        {
            char[] arr = GetArrFromLoc(loc);
            arr[idx] = val;
        }

        private char[] GetArrFromLoc(char loc)
        {
            switch (loc)
            {
                case 'A': return roomA;
                case 'B': return roomB;
                case 'C': return roomC;
                case 'D': return roomD;
                default:
                    break;
            }
            return hall;
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
            Console.WriteLine($"  #{roomA[1]}#{roomB[1]}#{roomC[1]}#{roomD[1]}#     Energy {energy}");
            Console.WriteLine("  #########");
        }
    }
    public class Move
    {
        public char fromLoc;
        public int fromIdx;
        public char toLoc;
        public int toIdx;
        public int energy;

        public Move() { }

        // Calculate the energy required for this move
        public void FindEnergy(char piece)
        {
            int pieceValue = 1;
            while (piece > 'A')
            {
                piece--;
                pieceValue *= 10;
            }

            // Count the steps. 
            int hStart = fromIdx;
            int rStart = 0;
            if (fromLoc != 'H')
            {
                hStart = Board.GetRoomEntranceIdx(fromLoc);
                rStart = fromIdx + 1;
            }
            int hEnd = toIdx;
            int rEnd = 0;
            if (toLoc != 'H')
            {
                hEnd = Board.GetRoomEntranceIdx(toLoc);
                rEnd = toIdx + 1;
            }

            energy = pieceValue * (Math.Abs(hEnd - hStart) + rStart + rEnd);
        }
    }
}
