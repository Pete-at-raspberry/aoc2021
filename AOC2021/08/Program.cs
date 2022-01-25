using System;
using System.Collections.Generic;
using System.IO;

namespace _08
{
    class Program
    {
        static List<Entry> entries = new List<Entry>();
        static void Main(string[] args)
        {
            // Worked example from the website
            //Entry ent = new Entry("acedgfb cdfbe gcdfa fbcad dab cefabd cdfgeb eafb cagedb ab | cdfeb fcadb cdfeb cdbaf");
            //ent.Parse();
            ReadInput();
            //Part1();
            Part2();
        }

        static void ReadInput()
        {
            using (StreamReader inputRdr = new StreamReader("d:\\adventofcode\\AOC2021\\08\\input.txt"))
            {
                while (!inputRdr.EndOfStream)
                {
                    String line = inputRdr.ReadLine();
                    entries.Add(new Entry(line));
                }
            }
        }

        static void Part1()
        {
            int cnt = 0;
            foreach(Entry entry in entries)
            {
                cnt += entry.Count1478();
            }
            Console.WriteLine($"Total Count = {cnt}");
        }

        static void Part2()
        {
            int total = 0;
            foreach(Entry entry in entries)
            {
                entry.Parse();
                total += entry.GetValue();
            }
            Console.WriteLine($"Total value = {total}");
        }
    }

    public class Entry
    {
        private List<String> codes = new List<string>();
        private List<String> digits = new List<String>();
        private char segA, segB, segC, segD, segE, segF, segG;

        public Entry(String line)
        {
            String[] halves = line.Split('|');
            String[] codeStrs = halves[0].Split(' ', StringSplitOptions.RemoveEmptyEntries);
            foreach (String code in codeStrs)
                codes.Add(code);
            String[] digitStrs = halves[1].Split(' ', StringSplitOptions.RemoveEmptyEntries);
            foreach (String digit in digitStrs)
                digits.Add(digit);
        }

        public void Parse()
        {
            // Work out which groups are which. 
            String code1 = "";
            String code4 = "";
            String code7 = "";
            String code8 = "";
            List<String> code235 = new List<string>();
            List<String> code069 = new List<string>();

            foreach(String code in codes)
            {
                switch (code.Length)
                {
                    case 2: code1 = code; break;
                    case 3: code7 = code; break;
                    case 4: code4 = code; break;
                    case 5: code235.Add(code); break;
                    case 6: code069.Add(code); break;
                    case 7: code8 = code; break;    // N.B. Don't really care about this one!
                }
            }

            // Segments:
           /*  aaa
              b   c
              b   c 
               ddd
              e   f
              e   f
               ggg
           */
            // Find segment A
            String segmentA = RemoveString(code7, code1);
            segA = segmentA[0];

            // Now A,D,G
            String a_d_g = Intersection(code235[0], code235[1]);
            a_d_g = Intersection(a_d_g, code235[2]);

            // Find D
            String segmentD = Intersection(a_d_g, code4);
            segD = segmentD[0];

            // now B
            String b_d = RemoveString(code4, code1);
            String segmentB = RemoveString(b_d, segmentD);
            segB = segmentB[0];

            // And now G
            String segmentG = RemoveString(a_d_g, segmentA);
            segmentG = RemoveString(segmentG, segmentD);
            segG = segmentG[0];

            // Find 3
            String code3 = "";
            foreach(String code in code235)
            {
                if (Intersection(code, code4).Length == 4)
                {
                    code3 = code;
                    break;
                }
            }

            // Find 6,9,0
            String code6 = "", code9 = "", code0 = "";
            foreach(string code in code069)
            {
                if (!code.Contains(segD))
                {
                    code0 = code;
                }
                else if (Intersection(code, code1).Length == 2)
                {
                    code9 = code;
                }
                else
                {
                    code6 = code;
                }
            }

            // Now we can split C and F.
            String segmentF = Intersection(code6, code1);
            segF = segmentF[0];
            String segmentC = RemoveString(code1, segmentF);
            segC = segmentC[0];

            // Don't care about E.
        }

        private String RemoveString(String s, String rem)
        {
            foreach (char c in rem)
                s = s.Replace(c.ToString(), "");
            return s;
        }

        private String Intersection(String s1, String s2)
        {
            String res = "";
            foreach(char c in s1)
            {
                if (s2.Contains(c))
                    res = res + c;
            }

            return res;
        }
        
        public int GetValue()
        {
            int value = 0;
            foreach (String digit in digits)
            {
                value *= 10;
                value += DecodeDigit(digit);
            }
            return value;
        }

        public int Count1478()
        {
            int cnt = 0;
            foreach(String digit in digits)
            {
                int number = DecodeDigit(digit);
                if (number == 1 || number == 4 || number == 7 || number == 8)
                    cnt++;
            }
            return cnt;
        }

        private int DecodeDigit(String digit)
        {
            switch (digit.Length)
            {
                case 2: return 1;
                case 3: return 7;
                case 4: return 4;
                case 7: return 8;
                case 5:
                    // 2,3 or 5.
                    if (!digit.Contains(segC))
                        return 5;
                    if (digit.Contains(segF))
                        return 3;
                    return 2;
                case 6:
                    // 6,9, or 0.
                    if (!digit.Contains(segD))
                        return 0;
                    if (digit.Contains(segC))
                        return 9;
                    return 6;
            }
            // won't get here
            throw new Exception("Failed to interpret digit");
        }
    }
}
