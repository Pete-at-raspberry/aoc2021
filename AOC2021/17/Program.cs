using System;

namespace _17
{
    class Program
    {
        // Real input
        static string input = "target area: x=244..303, y=-91..-54";
        // Sample
        //static string input = "target area: x=20..30, y=-10..-5";

        // Target area
        private static int xlo, xhi, ylo, yhi;
        private static int maxYstart = 5000;

        static void Main(string[] args)
        {
            ReadInput();
            Part1();
            Part2();
        }

        static void Part1()
        {
            // To be falling needs to be almost vertical, so assume x component has run out. 
            int xMin = 0, xMax = 0;
            for (int x=2; x<1000; ++x)
            {
                int endPos = (x * (x + 1)) / 2;
                if (endPos > xlo && xMin == 0)
                    xMin = x - 1;
                if (endPos > xhi && xMax == 0)
                {
                    xMax = x - 1;
                    break;
                }
            }

            // OK, now just need to find a  big value of y in this range of x values that works. 
            for (int y = 5000; y > 1; --y)
            {
                for (int x = xMin; x<=xMax; ++x)
                {
                    int ht = HitsTarget(x, y);
                    if (ht > 0)
                    {
                        Console.WriteLine($"Max height is {ht}");
                        maxYstart = y;
                        return;
                    }
                }
            }
        }

        static void Part2()
        {
            int count = 0;
            // Go round all combinations and coumt/
            for (int y = ylo; y<= maxYstart; ++y)
            {
                // Min x is a bit less than sqrt(xlo), so
                int minXstart = (int)Math.Floor(Math.Sqrt((double)xlo)) - 5;
                for (int x = minXstart; x<=xhi; ++x)
                {
                    if (HitsTarget(x, y) > 0)
                        ++count;
                }
            }
            Console.WriteLine($"Count of starting positions is {count}");
        }

        private static int HitsTarget(int x, int y)
        {
            int maxHt = 1;
            int px = 0, py = 0;
            for (int step = 0; step < 10000; ++step)
            {
                // Do the step. 
                px += x;
                py += y;
                if (x > 0)
                    x--;
                y--;

                if (py > maxHt)
                    maxHt = py;

                // Is it in the box? 
                if (px >= xlo && px <= xhi && py >= ylo && py <= yhi)
                    return maxHt;
                if (py < ylo)
                    break;
            }

            return -1;
        }

        private static void ReadInput()
        {
            String[] xy = input.Split(',');
            String[] xwords = xy[0].Split('=');
            String[] xNumbers = xwords[1].Split("..");
            xlo = Int32.Parse(xNumbers[0]);
            xhi = Int32.Parse(xNumbers[1]);
            String[] ywords = xy[1].Split('=');
            String[] yNumbers = ywords[1].Split("..");
            ylo = Int32.Parse(yNumbers[0]);
            yhi = Int32.Parse(yNumbers[1]);
        }
    }
}
