using System;
using System.Collections.Generic;
using System.IO;

namespace _12
{
    class Program
    {
        static Dictionary<String, Cave> caves = new Dictionary<string, Cave>();
        public static int routes = 0;

        static void Main(string[] args)
       
        {
            ReadInput();
            //Part1();
            Part2();
        }

        static void ReadInput()
        {
            using (StreamReader inputRdr = new StreamReader("d:\\adventofcode\\AOC2021\\12\\input.txt"))
            {
                while (!inputRdr.EndOfStream)
                {
                    String line = inputRdr.ReadLine();
                    String[] words = line.Split('-');

                    // Add these caves if this is the first time seen.
                    for (int n = 0; n < 2; ++n)
                    {
                        if (!caves.ContainsKey(words[n]))
                            caves.Add(words[n], new Cave(words[n]));
                    }

                    // Now add the link
                    caves[words[0]].AddLink(caves[words[1]]);
                }
            }
        }

        static void Part1()
        {
            // Just recurse.
            Stack<Cave> cs = new Stack<Cave>();
            Cave start = caves["start"];
            start.VisitCave(cs);

            Console.WriteLine($"Total routes = {routes}");
        }

        static void Part2()
        {
            // Just recurse.
            Stack<Cave> cs = new Stack<Cave>();
            Cave start = caves["start"];
            start.VisitCave(cs);

            // Now to add in the routes with double visits. 
            foreach(Cave extraCave in caves.Values)
            {
                cs.Clear();
                if (!extraCave.IsSmall() || extraCave.IsEnd() || extraCave == start)
                    continue;
                Cave.extraVisit = extraCave;
                start.VisitCave2(cs);
            }
            Console.WriteLine($"Total routes = {routes}");
        }


    }

    public class Cave
    {
        private String name;
        private List<Cave> caves = new List<Cave>();
        public static Cave extraVisit = null;

        public Cave(String n)
        {
            name = n;
        }

        public void AddLink(Cave to)
        {
            // A link joins both caves each way, so
            caves.Add(to);
            to.caves.Add(this);
        }

        public void VisitCave(Stack<Cave> cs)
        {
            // If this is the end we are done. 
            if (IsEnd())
            {
                Program.routes++;
                return;
            }

            cs.Push(this);
            foreach (Cave cTo in caves)
            {
                if (!(cTo.IsSmall() && cs.Contains(cTo)))
                {
                    cTo.VisitCave(cs);
                }
            }
            cs.Pop();
        }

        public void VisitCave2(Stack<Cave> cs)
        {
            // If this is the end we are done. 
            if (IsEnd())
            {
                // Only count if we had both visits from the extra cave.
                if (extraVisit.CountVisits(cs) == 2)
                    Program.routes++;
                return;
            }

            cs.Push(this);
            foreach (Cave cTo in caves)
            {
                if (cTo.CanVisit(cs))
                {
                    cTo.VisitCave2(cs);
                }
            }
            cs.Pop();
        }

        private bool CanVisit(Stack<Cave> visited)
        {
            bool can = true;
            if (IsSmall())
            {
                bool beenDone = visited.Contains(this);
                if (extraVisit != this)
                {
                    if (beenDone)
                        can = false;
                }
                else
                {
                    if (CountVisits(visited) > 1)
                        can = false;
                }
            }
            return can;
        }

        private int CountVisits(Stack<Cave> visited)
        {

            Cave[] visitList = visited.ToArray();
            int count = 0;
            for (int i = 0; i < visitList.Length; i++)
            {
                if (visitList[i] == this)
                {
                    ++count;                }
            }
            return count;
        }

        public bool IsSmall()
        {
            return Char.IsLower(name[0]);
        }

        public bool IsEnd()
        {
            return name.Equals("end");
        }
    }
}
