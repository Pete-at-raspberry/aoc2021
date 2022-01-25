using System;
using System.Collections.Generic;
using System.IO;

namespace _22
{
    class Program
    {
        static List<Instr> instructions = new List<Instr>();

        static void Main(string[] args)
        {
            ReadInput();
            //Part1();
            //Attempt2();
            //Attempt3();
            Attempt4();
            //Part2();
        }

        static void ReadInput()
        {
            using (StreamReader inputRdr = new StreamReader("d:\\adventofcode\\AOC2021\\22\\input.txt"))
            {
                // Read each instruction line
                while (!inputRdr.EndOfStream)
                {
                    String line = inputRdr.ReadLine();
                    Instr i = new Instr(line);
                    instructions.Add(i);
                }

            }
        }

        public static void Part1()
        {
            int xLo = 0, xHi = 0, yLo = 0, yHi = 0, zLo = 0, zHi = 0;

            foreach (Instr i in instructions)
            {
                i.FindExtent(ref xLo, ref xHi, ref yLo, ref yHi, ref zLo, ref zHi);
            }

            // Part 1 only. 
            xLo = yLo = zLo = -50;
            xHi = yHi = zHi = 50;

            long total = 0;
            Console.WriteLine($"x Extent {xLo}..{xHi}"); 

            for (int x = xLo; x <= xHi; ++x)
            {
                for (int y = yLo; y <= yHi; ++y)
                {
                    List<LoHiPair> pairs = new List<LoHiPair>();
                    foreach (Instr i in instructions)
                    {
                        i.AddToLine(x, y, pairs);
                    }
                    // Now count the pair values. 
                    foreach(LoHiPair p in pairs)
                    {
                        total += p.hi - p.lo + 1;
                    }
                }
                if (x % 10000 == 0)
                    Console.Write('.');
            }

            Console.WriteLine($"Total on is {total}");
        }

        public static void Attempt2()
        {
            int xLo = 0, xHi = 0, yLo = 0, yHi = 0, zLo = 0, zHi = 0;

            foreach (Instr i in instructions)
            {
                i.FindExtent(ref xLo, ref xHi, ref yLo, ref yHi, ref zLo, ref zHi);
            }

            // Part 1 only. 
            xLo = yLo = zLo = -50;
            xHi = yHi = zHi = 50;

            long total = 0;
            Console.WriteLine($"x Extent {xLo}..{xHi}");

            for (int z = zLo; z <= zHi; ++z)
            {
                List<Rect> rects = new List<Rect>();
                foreach (Instr i in instructions)
                {
                    i.AddToArea(z, rects);
                }
                // Now count the cubes in the area. 
                foreach (Rect r in rects)
                {
                    total += r.Area();
                }
            }

            Console.WriteLine($"Total on is {total}");
        }

        private static void Attempt3()
        {
            Console.WriteLine("Attempt 3");

            long total = 0;
            List<Cuboid> clist = new List<Cuboid>();
            foreach (Instr i in instructions)
            {
                List<Cuboid> toAdd = new List<Cuboid>();
                Cuboid newCub = i.MakeCuboid();
                
                // Ignore offs
                if (!newCub.on)
                    continue;
                toAdd.Add(newCub);
                total += newCub.Count();

                // Deduct any intersections, and add back any areas that overlap with existing intersections
                foreach (Cuboid c in clist)
                {
                    Cuboid newIn = c.Intersect(newCub);
                    if (newIn != null)
                    {
                        if (c.isIntersection)
                        {
                            total += newIn.Count();
                        }
                        else
                        {
                            total -= newIn.Count();
                            toAdd.Add(newIn);
                        }
                    }
                }

                clist.AddRange(toAdd);
            }
            Console.WriteLine($"Total on is {total}");
        }
        private static void Attempt4()
        {
            Console.WriteLine("Attempt 4");
            List<Cuboid> allCubes = new List<Cuboid>();
            foreach (Instr i in instructions)
            {
                List<Cuboid> toRemove = new List<Cuboid>();
                List<Cuboid> addedCbs = new List<Cuboid>();
                Cuboid newCuboid = i.MakeCuboid();
                if (newCuboid.on)
                {
                    addedCbs.Add(newCuboid);
                    foreach (Cuboid existCb in allCubes)
                    {
                        addedCbs = existCb.FindOverlaps(addedCbs, toRemove);
                        if (addedCbs.Count == 0)
                            break;
                    }
                }
                else
                {
                    foreach (Cuboid existCb in allCubes)
                    {
                        addedCbs.AddRange(existCb.RemoveOverlaps(newCuboid, toRemove));
                    }
                }
                allCubes.RemoveAll(x => toRemove.Contains(x));
                allCubes.AddRange(addedCbs);
            }

            // Now count. 
            long total = 0;
            foreach(Cuboid c in allCubes)
            {
                total += c.Count();
            }
            Console.WriteLine($"Total cuboids is {allCubes.Count}, Total is {total}");
        }
    }


    public class Cuboid
    {
        public int xl, xh, yl, yh, zl, zh;
        public bool on;
        public bool isIntersection;

        public Cuboid(int x1, int x2, int y1, int y2, int z1, int z2, bool o, bool i)
        {
            xl = x1;
            xh = x2;
            yl = y1;
            yh = y2;
            zl = z1;
            zh = z2;
            on = o;
            isIntersection = i;
        }

        // Create a list of cuboids such that any intersections with the list passed are removed.
        // If this cuboid is to be removed, then add it to the list.
        public List<Cuboid> FindOverlaps(List<Cuboid> newOnes, List<Cuboid> toRemove)
        {
            // Find simple cases first. 
            bool anythingToDo = false;
            foreach(Cuboid c in newOnes)
            {
                if (IntersectAtAll(c))
                {
                    anythingToDo = true;
                }
                if (c.Contains(this))
                {
                    // This one is completely contained.
                    toRemove.Add(this);
                    return newOnes;
                }
            }

            if (!anythingToDo)
            {
                // No intersections.
                return newOnes;
            }
            List<Cuboid> newList = new List<Cuboid>();
            foreach (Cuboid c in newOnes)
            {
                if (IntersectAtAll(c))
                {
                    newList.AddRange(FindDifferences(c));
                }
                else
                {
                    newList.Add(c);
                }
            }

            return newList;
        }

        // Passed Cuboid is to be turned off. 
        public List<Cuboid> RemoveOverlaps(Cuboid offCb, List<Cuboid> toRemove)
        {
            // Nothing to do if no intersection
            if (IntersectAtAll(offCb))
            {
                toRemove.Add(this);
                if (!offCb.Contains(this)) 
                    return offCb.FindDifferences(this);
            }
            return new List<Cuboid>();
        }

        // True if c is contained in this
        private bool Contains(Cuboid c)
        {
            return c.xl >= xl && c.xh <= xh && c.yl >= yl && c.yh <= yh && c.zl >= zl && c.zh <= zh;
        }

        private List<Cuboid> FindDifferences(Cuboid c)
        {
            List<Cuboid> res = new List<Cuboid>();
            List<AxisSegment> xSegs = FindAxisSegments(xl, xh, c.xl, c.xh);
            List<AxisSegment> ySegs = FindAxisSegments(yl, yh, c.yl, c.yh);
            List<AxisSegment> zSegs = FindAxisSegments(zl, zh, c.zl, c.zh);
            foreach (AxisSegment xs in xSegs)
            {
                foreach (AxisSegment ys in ySegs)
                {
                    foreach (AxisSegment zs in zSegs)
                    {
                        if (xs.within && ys.within && zs.within)
                            continue;
                        res.Add(new Cuboid(xs.lo, xs.hi, ys.lo, ys.hi, zs.lo, zs.hi, true, false));
                    }
                } 
            }
            return res;
        }

        private List<AxisSegment> FindAxisSegments(int lo, int hi, int newLo, int newHi)
        {
            List<AxisSegment> res = new List<AxisSegment>();
            int withinLo = lo;
            int withinHi = hi;

            // Ignore if no overlap
            if (newHi < lo || newLo > hi)
                return res;

            if (newLo < lo)
            {
                res.Add(new AxisSegment(newLo, lo - 1, false));
            }
            else
            {
                withinLo = newLo;
            }
            if (newHi > hi)
            {
                res.Add(new AxisSegment(hi + 1, newHi, false));
            }
            else 
            {
                withinHi = newHi;
            }
            res.Add(new AxisSegment(withinLo, withinHi, true));

            return res;
        }

        private class AxisSegment
        {
            public int lo, hi;
            public bool within;
            public AxisSegment(int l, int h, bool inc)
            {
                lo = l;
                hi = h;
                within = inc;
            }
        }

        public Cuboid Intersect(Cuboid c2)
        {
            Cuboid ret = null;
            if (IntersectAtAll(c2))
            {
                int x1, x2, y1, y2, z1, z2;
                FindAxisIntersection(xl, xh, c2.xl, c2.xh, out x1, out x2);
                FindAxisIntersection(yl, yh, c2.yl, c2.yh, out y1, out y2);
                FindAxisIntersection(zl, zh, c2.zl, c2.zh, out z1, out z2);
                ret = new Cuboid(x1, x2, y1, y2, z1, z2, on, true);
            }

            return ret;
        }

        private static void FindAxisIntersection(int xl, int xh, int xxl, int xxh, out int x1, out int x2)
        {
            if (xl <= xxl)
            {
                x1 = xxl;
            }
            else
            {
                x1 = xl;
            }
            if (xh > xxh)
            {
                x2 = xxh;
            }
            else
            {
                x2 = xh;
            }
        }

        private bool IntersectAtAll(Cuboid c2)
        {
            return !(c2.xl > xh || c2.xh < xl || c2.yl > yh || c2.yh < yl || c2.zl > zh || c2.zh < zl);
        }

        public long Count()
        {
            return (long)(xh - xl + 1) * (long)(yh - yl + 1) * (long)(zh - zl + 1);
        }

    }

    public class Rect
    {
        public int xl, xh, yl, yh;
        public Rect(int x1, int y1, int x2, int y2)
        {
            xl = x1; xh = x2; yl = y1; yh = y2;
        }

        public int Area()
        {
            return (xh - xl + 1) * (yh - yl + 1);
        }

        public bool Intersects(int x1, int y1, int x2, int y2)
        {
            return !(x1 > xh || x2 < xl || y1 > yh || y2 < yl);
        }
    }

    public class LoHiPair
    {
        public LoHiPair(int l, int h)
        { lo = l; hi = h; }
        public int lo;
        public int hi;
    }

    public class Instr
    {
        private bool on = true;
        private int xlo, xhi, ylo, yhi, zlo, zhi;
        
        public void AddToArea(int z, List<Rect> rects)
        {
            if (z >= zlo && z <= zhi)
            {
                int xl = xlo;
                int xh = xhi;
                int yl = ylo;
                int yh = yhi;
                List<Rect> toAdd = new List<Rect>();
                List<Rect> toRemove = new List<Rect>();
                bool addThisOne = true;
                if (on)
                {
                    foreach(Rect r in rects)
                    {
                        if (r.Intersects(xl, yl, xh, yh))
                        {
                            // Simple cases first. r incorporates this
                            if (xl >= r.xl && xh <= r.xh && yl >= r.yl && yh <= r.yh)
                            {
                                // No need to add, and we are done.
                                addThisOne = false;
                                break;
                            }
                            else if (xl <= r.xl && xh >= r.xh && yl <= r.yl && yh >= r.yh)
                            {
                                // This rect incorporates r
                                toRemove.Add(r);
                                continue;
                            }
                            if (xl >= r.xl && xl <= r.xh && xh > r.xh)
                            {
                                // Left side within r, right side outside.
                                if (yl < r.yl)
                                {
                                    if (yh < r.yh)
                                    {
                                        Rect topBit = new Rect(xl, yh, r.xh, r.yh);
                                        toAdd.Add(topBit);
                                    }
                                    r.xh = xl - 1;
                                }
                            }
                        }
                    }
                }
                else
                {
                    // off. Remove any intersections. 
                }

                // Sort out the adding and removing. 
                rects.RemoveAll(x => toRemove.Contains(x));
                if (addThisOne)
                    rects.Add(new Rect(xl, yl, xh, yh));
                rects.AddRange(toAdd);
            }
        }

        public void AddToLine(int x, int y, List<LoHiPair> pairs)
        {
            if (x >= xlo && x <= xhi && y >= ylo && y <= yhi)
            {
                List<LoHiPair> toRemove = new List<LoHiPair>();
                int lo = zlo;
                int hi = zhi;
                bool addThisOne = true;
                if (on)
                {
                    foreach (LoHiPair pr in pairs)
                    {
                        if (pr.lo <= lo && pr.hi >= lo)
                        {
                            // lo side is in this pair. 
                            if (pr.hi >= hi)
                            {
                                // Contained. 
                                addThisOne = false;
                                break;
                            }
                            else
                            {
                                // Include this pair in the new one. 
                                lo = pr.lo;
                            }
                        }
                        if (pr.lo <= hi && pr.hi >= hi)
                        {
                            // hi side is in this pair. 
                            // Include in the new one. 
                            hi = pr.hi;
                        }

                        if (pr.lo >= lo && pr.hi <= hi)
                        {
                            // new pair includes the old one. 
                            toRemove.Add(pr);
                        }
                    }
                }
                else
                {
                    // Off. 
                    addThisOne = false;
                    foreach (LoHiPair pr in pairs)
                    {
                        if (pr.lo < lo && pr.hi > lo)
                        {
                            // lo side is in this pair. 
                            if (pr.hi > hi)
                            {
                                // Work out the lower portion.
                                int newHi = lo - 1;
                                // Contained deletion. 
                                addThisOne = true;
                                lo = hi + 1;
                                hi = pr.hi;
                                pr.hi = newHi;
                                break;
                            }
                            else
                            {
                                // Remove the hi portion.
                                pr.hi = lo - 1;
                            }
                        }
                        else if (pr.lo <= hi && pr.hi >= hi)
                        {
                            // hi side is in this pair. 
                            // Remove lo portion
                            pr.lo = hi + 1;
                        }
                        else if (pr.lo >= lo && pr.hi <= hi)
                        {
                            // new pair includes the old one. 
                            toRemove.Add(pr);
                        }
                    }
                }
                pairs.RemoveAll(x => toRemove.Contains(x));
                if (addThisOne)
                    pairs.Add(new LoHiPair(lo, hi));

            }
        }

        public void FindExtent(ref int xLo, ref int xHi, ref int yLo, ref int yHi, ref int zLo, ref int zHi)
        {
            if (xlo < xLo)
                xLo = xlo;
            if (xhi > xHi)
                xHi = xhi;
            if (ylo < yLo)
                yLo = ylo;
            if (yhi > yHi)
                yHi = yhi;
            if (zlo < zLo)
                zLo = zlo;
            if (zhi > zHi)
                zHi = zhi;
        }

        public Cuboid MakeCuboid()
        {
            return new Cuboid(xlo, xhi, ylo, yhi, zlo, zhi, on, false);
        }

        public Instr(String line)
        {
            if (line.StartsWith("off"))
            {
                on = false;
                line = line.Substring(4);
            }
            else
            {
                line = line.Substring(3);
            }

            String[] xyz = line.Split(',');
            ReadLoHiPair(xyz[0], out xlo, out xhi);
            ReadLoHiPair(xyz[1], out ylo, out yhi);
            ReadLoHiPair(xyz[2], out zlo, out zhi);
        }

        private static void ReadLoHiPair(String line, out int lo, out int hi)
        {
            // Like 'y=-12..45'. Ignore the y=.
            line = line.Trim();
            line = line.Substring(2);
            String[] loHi = line.Split("..");
            lo = Int32.Parse(loHi[0]);
            hi = Int32.Parse(loHi[1]);
        }
    }
}
