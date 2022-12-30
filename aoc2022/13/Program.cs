using System;
using System.Collections.Generic;
using System.IO;

namespace _13
{
    internal class Program
    {
        static List<String> input = new List<String>();

        static void Main(string[] args)
        {
            ReadInput();
            Part1();
            Part2();
        }

        static void ReadInput()
        {
            using (StreamReader inputRdr = new StreamReader("d:\\adventofcode\\AOC2022\\13\\input.txt"))
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
            int sum = 0;
            for (int i = 0; i < input.Count; i += 3)
            {
                int pos = 0;
                Number left = Number.Parse(input[i], ref pos);
                pos = 0;
                Number right = Number.Parse(input[i + 1], ref pos);

                if (left.SmallerThan(right))
                {
                    sum += (i / 3) + 1;
                }
            }
            Console.WriteLine($"Sum of the correct indices is {sum}");
        }

        static void Part2()
        {
            Console.WriteLine("Part2");
            List<Number> numbers = new List<Number>();
            int pos = 0;
            Number mark2 = Number.Parse("[[2]]", ref pos);
            pos = 0;
            Number mark6 = Number.Parse("[[6]]", ref pos);
            numbers.Add(mark2); 
            numbers.Add(mark6);
            foreach (String line in input)
            {
                if (line.Length > 1)
                {
                    pos = 0;
                    numbers.Add(Number.Parse(line, ref pos));   
                }
            }
            numbers.Sort(delegate (Number l, Number r)
            {
                return r.CompareTo(l);
            });

            Console.WriteLine($"Score is {(numbers.IndexOf(mark2) + 1) * (numbers.IndexOf(mark6) + 1)}");
        }


        public class Number
        {
            public bool SmallerThan(Number right)
            {
                int comp = CompareTo(right);
                return (comp > 0);
            }

            // Return 1 if left > right, -1 if left < right.
            public virtual int CompareTo(Number right)
            {
                return 0;
            }

            public static Number Parse(String line, ref int pos)
            {
                NumList nl = new NumList();

                while (pos < line.Length)
                {
                    char c = line[pos++];
                    int val;
                    if (c == '[' && pos > 1)
                    {
                        // Starting a sub-list.
                        nl.Add(Number.Parse(line, ref pos));

                    }
                    else if (c == ']')
                    {
                        // Ending a list.
                        return nl;
                    }
                    else if (char.IsDigit(c))
                    {
                        // Its a number.
                        char[] valArray = new char[2];
                        valArray[0] = c;
                        if (char.IsDigit(line[pos]))
                            valArray[1] = line[pos++];
                        String valStr = new string(valArray);
                        val = int.Parse(valStr);
                        NumValue nv = new NumValue(val);
                        nl.Add(nv);
                    }
                    else
                    {
                        // Ignore. Commas are used to separate tokens.
                    }
                }
                return nl;
            }
        }

        public class NumValue : Number 
        { 
            public Int32 value;

            public NumValue(int val)
            {
                value = val;
            }

            public override int CompareTo(Number right)
            {
                if (right is NumValue)
                {
                    if (value > ((NumValue)right).value)
                        return -1;
                    else if (value < ((NumValue)right).value)
                        return 1;
                    else
                        return 0;
                }
                else if (right is NumList)
                {
                    NumList newLeft = new NumList();
                    newLeft.Add(this);
                    return newLeft.CompareTo(right);
                }
                return 0;
            }
        }

        public class NumList : Number
        {
            List<Number> list = new List<Number>();

            public void Add(Number n)
            {
                list.Add(n);
            }

            public override int CompareTo(Number right)
            {
                NumList rList = null;
                if (right is NumList)
                    rList = (NumList)right;
                else
                {
                    rList = new NumList();
                    if (right is NumValue)
                        rList.Add(right);
                }
                // Compare each element in the list. 
                for (int i=0; i<list.Count; i++)
                {
                    if (rList.list.Count <= i)
                        return -1;
                    int comp = this.list[i].CompareTo(rList.list[i]);
                    if (comp != 0)
                        return comp;
                }
                if (rList.list.Count > list.Count)
                    return 1;

                // Same.
                return 0;
            }
        }
    }
}
