using System;
using System.Collections.Generic;
using System.IO;

namespace _07
{
    internal class Program
    {
        static List<string> input = new List<string>();

        static void Main(string[] args)
        {
            ReadInput();
            Part1();
            //Part2();
        }

        static void ReadInput()
        {
            using (StreamReader inputRdr = new StreamReader("d:\\adventofcode\\AOC2022\\07\\input.txt"))
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

            // Start at the root. 
            Dir currDir = new Dir("/");
            Dir root = currDir;
            // Ignore the first line
            for (int ln = 1; ln<input.Count; ++ln)
            {
                String line = input[ln];
                if (line.StartsWith("$"))
                {
                    // Command. Ignore ls. 
                    if (line.StartsWith("$ cd"))
                    {
                        currDir = currDir.Cd(line.Substring(5));
                    }
                }
                else 
                {
                    // Not a command, must be a directory listing. 
                    String[] parts = line.Split(" ");
                    if (parts[0] == "dir") 
                    {
                        Dir newDir = new Dir(parts[1]);
                        currDir.AddDir(newDir);
                    }
                    else
                    {
                        int sz = Int32.Parse(parts[0]);
                        currDir.AddFile(sz);
                    }
                }
            }

            root.Part1();
            root.Part2();
        }

        private class Dir
        {
            private int fileSize = 0;
            public string name;
            public Dir parent;
            public List<Dir> dirs = new List<Dir>();

            public Dir(String n) { name = n; }
            public void AddFile(int sz) { fileSize += sz; }
            public Dir Cd(String newDir)
            {
                if (newDir == "..")
                {
                    return parent;
                }
                foreach (Dir dir in dirs)
                {
                    if (newDir == dir.name)
                    {
                        return dir;
                    }
                }
                throw new Exception($"Can't find {newDir} in {name}!");
            }

            public void AddDir(Dir dir) 
            { 
                dirs.Add(dir); 
                dir.parent = this;
            }
            public int GetSize() 
            { 
                int sz = fileSize;
                foreach (Dir dir in dirs)
                {
                    sz += dir.GetSize();
                }
                return sz; 
            }

            // Find all dirs of <= 100,000 and sum. 
            public void Part1()
            {
                int total = AddToSum(0);
                Console.WriteLine($"Total size {total}");
            }

            // Find the smallest dir big enough to delete
            public void Part2()
            {
                int reqd = 30000000 - (70000000 - GetSize());
                int smallest = GetSize();
                foreach(Dir dir in dirs)
                {
                    smallest = dir.FindSmallest(reqd, smallest);
                }
                Console.WriteLine($"Smallest is {smallest}");
            }

            private int FindSmallest(int reqd, int smallest)
            {
                int mySize = GetSize();
                if (mySize >= reqd && mySize < smallest)
                {
                    smallest = mySize;
                }
                foreach(Dir dir in dirs)
                {
                    smallest = dir.FindSmallest(reqd, smallest);
                }
                return smallest;
            }

            private int AddToSum(int total)
            {
                int sz = GetSize();
                if (sz < 100000)
                {
                    total += sz;
                }
                foreach(Dir dir in dirs)
                {
                    total = dir.AddToSum(total);
                }
                return total;
            }
        }

    }
}
