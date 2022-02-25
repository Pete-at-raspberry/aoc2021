using System;
using System.Collections.Generic;
using System.IO;

namespace _19
{
    class Program
    {
        private static List<Scanner> scanners = new List<Scanner>();

        static void Main(string[] args)
        {
            ReadInput();
            Part1();
            //Part2();
        }

        static void ReadInput()
        {
            using (StreamReader inputRdr = new StreamReader("d:\\adventofcode\\AOC2021\\19\\input.txt"))
            {
                Scanner s = null;
                // Read each instruction line
                while (!inputRdr.EndOfStream)
                {
                    String line = inputRdr.ReadLine();
                    if (line.StartsWith("---"))
                    {
                        s = new Scanner(line);
                        scanners.Add(s);
                    }
                    else if (line.Length > 5)
                    {
                        s.AddBeacon(line);
                    }
                }

            }
        }

        static void Part1()
        {
            // Make a list of all the scanners that are joined up. 
            List<Scanner> placedScanners = new List<Scanner>();

            // Add the first. 
            Scanner first = scanners[0];
            placedScanners.Add(first);
            scanners.RemoveAt(0);

            List<XYZ> allBeacons = new List<XYZ>();
            first.AddBeaconsToList(allBeacons);

            while (scanners.Count > 0)
            {
                Scanner scannerToMove = null;
                // Find one that matches. 
                foreach (Scanner s in scanners)
                {
                    // Does this one match? 
                    if (s.Intersects(allBeacons))
                    {
                        scannerToMove = s;
                        s.AddBeaconsToList(allBeacons);
                        break;
                    }
                }
                placedScanners.Add(scannerToMove);
                scanners.Remove(scannerToMove);
            }


            Console.WriteLine($"Part 1 - Beacon count is {allBeacons.Count}");

            // Part 2 - find the shortest Manhatten distance.
            int longest = 0;
            for (int i=0; i< placedScanners.Count - 1; ++i)
            {
                for (int j = i +1; j<placedScanners.Count; ++j)
                {
                    int diff = placedScanners[i].FindDiff(placedScanners[j]);
                    if (diff > longest)
                        longest = diff;
                }
            }

            Console.WriteLine($"Part 2 - longest Manhatten is {longest}");
        }
    }

    public class Scanner
    {
        private String name;
        private List<XYZ> beacons = new List<XYZ>();
        private XYZ offset = new XYZ(0,0,0);
        private Orientation orient = new Orientation();

        public Scanner(String nm)
        {
            name = nm;
        }

        public void AddBeacon(String coords)
        {
            String[] numbers = coords.Split(',');
            int x = int.Parse(numbers[0].Trim());
            int y = int.Parse(numbers[1].Trim());
            int z = int.Parse(numbers[2].Trim());
            beacons.Add(new XYZ(x, y, z));
        }

        public int FindDiff(Scanner b)
        {
            XYZ d = offset.Minus(b.offset);
            return (int)d.Checksum();
        }

        // Work out if it intersects, and set the orientation and offset (of s) accordingly.
        public bool Intersects(List<XYZ> allBeacons)
        {
            bool intersects = false;
            Console.WriteLine($"Trying {name}");

            // Try a pair of points in the beacons list.
            for (int bFromIdx = 0; bFromIdx < allBeacons.Count - 1; ++bFromIdx)
            {
                for (int bToIdx = bFromIdx + 1; bToIdx < allBeacons.Count; ++bToIdx)
                {
                    XYZ existXYZ = allBeacons[bFromIdx].Minus(allBeacons[bToIdx]);
                    long sig = existXYZ.Checksum();
                    for (int sFromIdx = 0; sFromIdx < beacons.Count - 1; ++sFromIdx)
                    {
                        for (int sToIdx = sFromIdx + 1; sToIdx < beacons.Count; ++sToIdx)
                        {
                            XYZ newXYZ = beacons[sFromIdx].Minus(beacons[sToIdx]);
                            long sig2 = newXYZ.Checksum();
                            if (sig == sig2)
                            {
                                // Found potential match!!!
                                orient = new Orientation(existXYZ, newXYZ);
                                if (orient.Failed())
                                {
                                    break;
                                }
                                XYZ newBeaconPos = orient.orient(beacons[sFromIdx]);
                                offset = allBeacons[bFromIdx].Minus(newBeaconPos);  // newBeaconPos.Minus(allBeacons[bFromIdx]);
                                if (SeeIfItsOK(allBeacons))
                                {
                                    Console.WriteLine("Found it");
                                    return true;
                                }
                                else
                                {
                                    // Try the other way around. 
                                    newXYZ = beacons[sToIdx].Minus(beacons[sFromIdx]);
                                    orient = new Orientation(existXYZ, newXYZ);
                                    offset = allBeacons[bFromIdx].Minus(newBeaconPos);  // newBeaconPos.Minus(allBeacons[bFromIdx]);
                                    if (SeeIfItsOK(allBeacons))
                                    {
                                        Console.WriteLine("Found it");
                                        return true;
                                    }

                                }
                            }
                        }
                    }
                }
            }

            return intersects;
        }

        private bool SeeIfItsOK(List<XYZ> allBeacons)
        {
            int matchCount = 0;
            // Go through all the beacons and see if at least 12 match up with the locations. 
            foreach(XYZ b in beacons)
            {
                XYZ pos = AdjustedXYZ(b);
                foreach(XYZ bcn in allBeacons)
                {
                    if (bcn.IsEqual(pos))
                    {
                        ++matchCount;
                        break;
                    }
                }
            }
            return matchCount >= 12;
        }

        public void AddBeaconsToList(List<XYZ> allBeacons)
        {
            List<XYZ> toAdd = new List<XYZ>();
            foreach(XYZ b in beacons)
            {
                bool found = false;
                XYZ beacon = AdjustedXYZ(b);
                foreach (XYZ bcn in allBeacons)
                {
                    if (bcn.IsEqual(beacon))
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                    toAdd.Add(beacon);
            }
            allBeacons.InsertRange(0, toAdd);
        }

        private XYZ AdjustedXYZ(XYZ xyz)
        {
            XYZ corrected = orient.orient(xyz);
            return offset.Add(corrected);
        }
    }

    public class XYZ
    {
        public int x, y, z;

        public XYZ(int xx, int yy, int zz)
        {
            x = xx;
            y = yy;
            z = zz;
        }

        public XYZ Minus(XYZ b)
        {
            return new XYZ(x - b.x, y - b.y, z - b.z);
        }

        public XYZ Add(XYZ b)
        {
            return new XYZ(x + b.x, y + b.y, z + b.z);
        }

        public bool IsEqual(XYZ b)
        {
            return b.x == x && b.y == y && b.z == z;
        }

        public long Checksum()
        {
            int p, q, r;
            p = Math.Abs(x);
            q = Math.Abs(y);
            r = Math.Abs(z);
            return p + q + r;
            //Array.Sort(nums);
            //return (nums[0] * 1000000) + (nums[1] * 1000) * nums[2];
        }
    }

    public class Orientation
    {
        private char xFrom, yFrom, zFrom;
        private bool xNeg, yNeg, zNeg;

        public Orientation()
        { 
            xFrom = 'X'; yFrom = 'Y'; zFrom = 'Z';
            xNeg = yNeg = zNeg = false;
        }

        // Work out the orientation from two (equal) XYZs with differing orients
        public Orientation(XYZ a, XYZ b)
        {
            try
            {
                if (Math.Abs(a.x) == Math.Abs(b.x))
                {
                    xFrom = 'X';
                    xNeg = a.x != b.x;
                }
                else if (Math.Abs(a.x) == Math.Abs(b.y))
                {
                    xFrom = 'Y';
                    xNeg = a.x != b.y;
                }
                else if (Math.Abs(a.x) == Math.Abs(b.z))
                {
                    xFrom = 'Z';
                    xNeg = a.x != b.z;
                }
                else throw new Exception("Can't match on X!");
                if (Math.Abs(a.y) == Math.Abs(b.x))
                {
                    yFrom = 'X';
                    yNeg = a.y != b.x;
                }
                else if (Math.Abs(a.y) == Math.Abs(b.y))
                {
                    yFrom = 'Y';
                    yNeg = a.y != b.y;
                }
                else if (Math.Abs(a.y) == Math.Abs(b.z))
                {
                    yFrom = 'Z';
                    yNeg = a.y != b.z;
                }
                else throw new Exception("Can't match on Y!");
                if (Math.Abs(a.z) == Math.Abs(b.x))
                {
                    zFrom = 'X';
                    zNeg = a.z != b.x;
                }
                else if (Math.Abs(a.z) == Math.Abs(b.y))
                {
                    zFrom = 'Y';
                    zNeg = a.z != b.y;
                }
                else if (Math.Abs(a.z) == Math.Abs(b.z))
                {
                    zFrom = 'Z';
                    zNeg = a.z != b.z;
                }
                else throw new Exception("Can't match on Z!");

                if (xFrom == yFrom || xFrom == zFrom || yFrom == zFrom)
                    throw new Exception("Matching gone wrong");
            }
            catch(Exception e)
            {
                xFrom = '.';
            }
        }

        public bool Failed()
        {
            return xFrom == '.';
        }

        public XYZ orient(XYZ i)
        {
            int x, y, z;
            x = GetValueForPos(i, xFrom);
            y = GetValueForPos(i, yFrom);
            z = GetValueForPos(i, zFrom);
            if (xNeg) x = -x;
            if (yNeg) y = -y;
            if (zNeg) z = -z;

            XYZ res = new XYZ(x, y, z);
            return res;
        }

        private int GetValueForPos(XYZ i, char pos)
        {
            switch (pos)
            {
                case 'X': return i.x;
                case 'Y': return i.y;
                case 'Z': return i.z;
            }
            return 0;
        }
    }
}
