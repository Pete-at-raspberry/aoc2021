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
            using (StreamReader inputRdr = new StreamReader("d:\\adventofcode\\AOC2021\\19\\example.txt"))
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


            Console.WriteLine($"Beacon count is {allBeacons.Count}");
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

        // Work out if it intersects, and set the orientation and offset (of s) accordingly.
        public bool Intersects(List<XYZ> allBeacons)
        {
            bool intersects = false;

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
                                XYZ newBeaconPos = orient.orient(beacons[sFromIdx]);
                                offset = allBeacons[bFromIdx].Minus(newBeaconPos);  // newBeaconPos.Minus(allBeacons[bFromIdx]);
                                if (!allBeacons[bToIdx].Equals(AdjustedXYZ(beacons[sToIdx])))
                                {
                                    offset = allBeacons[bToIdx].Minus(newBeaconPos); // newBeaconPos.Minus(allBeacons[bToIdx]);
                                    if (!allBeacons[bFromIdx].Equals(AdjustedXYZ(beacons[sToIdx])))
                                        throw new Exception("Makes no sense");
                                }
                            }
                        }
                    }
                }
            }

            return intersects;
        }

        public void AddBeaconsToList(List<XYZ> allBeacons)
        {
            foreach(XYZ b in beacons)
            {
                XYZ beacon = AdjustedXYZ(b);
                if (!allBeacons.Contains(beacon))
                    allBeacons.Add(beacon);
            }
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
