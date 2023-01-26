using System;
using System.Collections.Generic;
using System.IO;

namespace _18
{
    internal class Program
    {
        static List<String> input = new List<String>();
        static HashSet<int> values = new HashSet<int>();
        static int minx = 1000, miny = 1000, minz = 1000;
        static int maxx = 0, maxy= 0, maxz = 0;
        static int totalSides;

        static void Main(string[] args)
        {
            ReadInput();
            Part1();
            Part2();
        }

        static void ReadInput()
        {
            using (StreamReader inputRdr = new StreamReader("d:\\adventofcode\\AOC2022\\18\\input.txt"))
            {
                while (!inputRdr.EndOfStream)
                {
                    String line = inputRdr.ReadLine();
                    input.Add(line);

                    string[] parts = line.Split(',');
                    int x = int.Parse(parts[0]);
                    int y = int.Parse(parts[1]);
                    int z = int.Parse(parts[2]);
                    int value = GetValue(x, y, z);
                    values.Add(value);
                    if (x < minx)
                        minx = x;
                    if (x > maxx)
                        maxx = x;
                    if (y < miny)
                        miny = y;
                    if (y > maxy)
                        maxy = y;
                    if (z < minz)
                        minz = z;
                    if (z > maxz)
                        maxz = z;
                }
            }
        }

        static int GetValue(int x, int y, int z)
        {
            return x * 65536 + y * 256 + z;
        }

        static void GetXYZ(int value, out int x, out int y, out int z)
        {
            z = value & 255;
            y = (value >> 8) & 255;
            x = (value >> 16) & 255;
        }

        static void Part1()
        {
            Console.WriteLine("Part1");

            // Go through and count
            int sides = 6 * values.Count; 
            foreach (int value in values)
            {
                int x, y, z;
                GetXYZ(value, out x, out y, out z);
                if (values.Contains(GetValue(x + 1, y, z)))
                    sides--;
                if (values.Contains(GetValue(x - 1, y, z)))
                    sides--;
                if (values.Contains(GetValue(x, y + 1, z)))
                    sides--;
                if (values.Contains(GetValue(x, y - 1, z)))
                    sides--;
                if (values.Contains(GetValue(x, y, z + 1)))
                    sides--;
                if (values.Contains(GetValue(x, y, z - 1)))
                    sides--;

            }
            totalSides = sides;
            Console.WriteLine($"Total exposed sides {sides}");
        }

 
        static private void Part2()
        {
            Console.WriteLine("Part2");

            // Floodfill to find the volume. 
            minx--;
            miny--;
            minz--;
            maxx++;
            maxy++;
            maxz++;

            HashSet<int> space = new HashSet<int>();
            Flood(space, minx, miny, minz);

            // Now find the edges that touch the void
            int sides = 0;
            foreach(int value in values)
            {
                int x, y, z;
                GetXYZ(value, out x, out y, out z);
                if (space.Contains(GetValue(x + 1, y, z)))
                    sides++;
                if (space.Contains(GetValue(x - 1, y, z)))
                    sides++;
                if (space.Contains(GetValue(x, y + 1, z)))
                    sides++;
                if (space.Contains(GetValue(x, y - 1, z)))
                    sides++;
                if (space.Contains(GetValue(x, y, z+1)))
                    sides++;
                if (space.Contains(GetValue(x, y, z-1)))
                    sides++;
            }
            Console.WriteLine($"Total sides {sides}");

            // Work out how many: total vol - flood points - occupied points
            //int total = (1 + maxx - minx) * (1 + maxy - miny) * (1 + maxz - minz);
            //int spacesLeft = total - space.Count - values.Count;
            //Console.WriteLine($"Total gaps is {spacesLeft}");
            //Console.WriteLine($"Total area is {totalSides - spacesLeft * 6}");
        }

        static private void Flood(HashSet<int> space, int x, int y, int z)
        {
            int value = GetValue(x, y, z);
            if (!space.Contains(value) && !values.Contains(value))
            {
                space.Add(GetValue(x,y,z));
                if (x > minx)
                    Flood(space, x - 1, y, z);
                if (x < maxx)
                    Flood(space, x + 1, y, z);
                if (y > miny)
                    Flood(space, x, y - 1, z);
                if (y < maxy)
                    Flood(space, x, y+1, z);
                if (z > minz)
                    Flood(space, x, y, z - 1);
                if (z < maxz)
                    Flood(space, x, y, z + 1);
            }
        }
    }
}
