using System;
using System.Collections.Generic;
using System.IO;

namespace _18
{

    class Program
    {
        private static List<SFNumber> numbers = new List<SFNumber>();

        static void Main(string[] args)
        {
            ReadInput();
            //Part1();
            Part2();
        }

        static void ReadInput()
        {
            using (StreamReader inputRdr = new StreamReader("d:\\adventofcode\\AOC2021\\18\\input.txt"))
            {
                while (!inputRdr.EndOfStream)
                {
                    String line = inputRdr.ReadLine();
                    SFNumber sfn = SFNumber.Parse(line);
                    numbers.Add(sfn);
                }
            }
        }

        static void Part1()
        {
            SFNumber n1 = SFNumber.Parse("[[[[4,3],4],4],[7,[[8,4],9]]]");
            SFNumber n2 = SFNumber.Parse("[1,1]");
            n1 = SFNumber.Add(n1, n2);
            n1.Print();
            Console.WriteLine();

            n1.Reduce();
            n1.Print();
            Console.WriteLine();

            foreach(SFNumber sfn in numbers)
            {
                sfn.Print();
                Console.WriteLine();
            }

            SFNumber sum = numbers[0];
            for (int n = 1; n<numbers.Count; ++n)
            {
                Console.Write("  ");
                sum.Print();
                Console.WriteLine();
                Console.Write("+ ");
                numbers[n].Print();
                Console.WriteLine();
                sum = SFNumber.Add(sum, numbers[n]);
                Console.Write("= ");
                sum.Print();
                Console.WriteLine();
                Console.WriteLine();
            }

            Console.WriteLine($"Final Magnitude is {sum.GetMagnitude()}");
        }

        static void Part2()
        {
            Console.WriteLine("Part 2");
            int maxMag = 0;

            for (int p = 0; p<numbers.Count; ++p)
            {
                for (int q = 0; q < numbers.Count; ++q)
                {
                    if (p != q)
                    {
                        SFNumber sum = SFNumber.Add(numbers[p], numbers[q]);
                        int mag = sum.GetMagnitude();
                        if (mag > maxMag)
                        {
                            sum.Print();
                            Console.WriteLine();
                            maxMag = mag;
                        }
                    }
                }
            }
            Console.WriteLine($"Max Magnitude is {maxMag}");

        }
    }

    public class SFNumber
    {
        public SFNumber() { }
        public virtual SFNumber Clone() { return new SFNumber(); }

        public static SFNumber Parse(String line)
        {
            int pos = 0;
            return Parse(line, ref pos);
        }

        public static SFNumber Add(SFNumber l, SFNumber r)
        {
            // This is going to be a pair, of course, so
            Pair result = new Pair(l, r);
            result.Reduce();
            return result;
        }

        protected static SFNumber Parse(String line, ref int pos)
        {
            SFNumber res = null;
            if (line[pos] == '[')
            {
                // Starting a pair. 
                res = new Pair();
                res.ParseThis(line, ref pos);
            }
            else if (Char.IsDigit(line[pos]))
            {
                // A number. 
                res = new Regular();
                res.ParseThis(line, ref pos);
            }
            else 
            {
                throw new Exception("Unexpected token " + line[pos]);
            }
            return res;
        }

        public void Reduce()
        {
            bool doneAnything;

            //Console.WriteLine("\nStarting Reduce");
            //Print();
            //Console.WriteLine();
            do
            {
                doneAnything = false;
                // Explode 
                Regular leftNumber = null;
                int searchForRight = -1;
                int depth = 1;
                TryExplode(depth, ref leftNumber, ref searchForRight, ref doneAnything);

                //Console.WriteLine("After TryExplode");
                //Print();
                //Console.WriteLine();
                
                if (!doneAnything)
                {
                    TrySplit(ref doneAnything);
                    //Console.WriteLine("After TrySplit");
                    //Print();
                    //Console.WriteLine();
                }
            } while (doneAnything);
        }

        protected virtual void ParseThis(String line, ref int pos)
        {  }

        public virtual void Print()
        { }

        public virtual int GetValue() { return 0; } 

        public virtual void TryExplode(int depth, ref Regular leftNumber, ref int searchForRight, ref bool doneAny) { }

        public virtual void TrySplit(ref bool doneAny) { }

        public virtual int GetMagnitude() { return 0; }
    }

    public class Pair : SFNumber
    {
        private SFNumber left;
        private SFNumber right;

        public Pair() { }

        public override SFNumber Clone()
        {
            return new Pair(this.left, this.right);
        }
        public Pair(SFNumber l, SFNumber r)
        {
            left = l.Clone();
            right = r.Clone();
        }

        public override int GetValue()
        {
            return left.GetValue() + right.GetValue();
        }

        public override int GetMagnitude()
        {
            return 3 * left.GetMagnitude() + 2 * right.GetMagnitude();
        }

        protected override void ParseThis(String line, ref int pos)
        {
            // absorb the [
            ++pos;
            left = SFNumber.Parse(line, ref pos);
            if (line[pos++] != ',')
                throw new Exception("Expected comma, got " + line[pos - 1]);
            right = SFNumber.Parse(line, ref pos);
            if (line[pos++] != ']')
                throw new Exception("Expected ], got " + line[pos - 1]);
        }


        public override void TryExplode(int depth, ref Regular leftNumber, ref int searchForRight, ref bool doneAny)
        {
            if (doneAny && searchForRight < 0)
            {
                // nothing to do. 
                return;
            }
            if (depth >= 4 && searchForRight < 0)
            {
                // Any pairs nested in here need to be exploded.
                if (left is Regular)
                {
                    // Just get the reference, as this will be the last one
                    left.TryExplode(depth + 1, ref leftNumber, ref searchForRight, ref doneAny);
                }
                else
                {
                    // Left is pair - it needs to be exploded. 
                    Pair leftPair = left as Pair;
                    if (leftNumber != null)
                    {
                        leftNumber.AddValue(leftPair.left.GetValue());
                    }
                    searchForRight = leftPair.right.GetValue();

                    // Now replace with 0
                    left = new Regular(0);

                    // We are done, don't do any more. 
                    doneAny = true;
                }
                if (right is Regular || doneAny)
                {
                    right.TryExplode(depth + 1, ref leftNumber, ref searchForRight, ref doneAny);
                }
                else
                {
                    // A pair, and we haven't done any, so it needs to be exploded. 
                    Pair rightPair = right as Pair;
                    if (leftNumber != null)
                    {
                        leftNumber.AddValue(rightPair.left.GetValue());
                    }
                    searchForRight = rightPair.right.GetValue();

                    // Now replace with 0
                    right = new Regular(0);

                    // We are done, don't do any more. 
                    doneAny = true;
                }
            }
            else
            {
                // Keep recursing. 
                left.TryExplode(depth + 1, ref leftNumber, ref searchForRight, ref doneAny);
                right.TryExplode(depth + 1, ref leftNumber, ref searchForRight, ref doneAny);
            }
        }

        public override void TrySplit(ref bool doneAny)
        {
            // Only if not done. 
            if (!doneAny)
            {
                if (left is Regular)
                {
                    // Does it need to split? 
                    int val = left.GetValue();
                    if (val >= 10)
                    {
                        // Yes! 
                        Regular newLeft = new Regular(val / 2);
                        Regular newRight = new Regular((val + 1) / 2);
                        Pair newPair = new Pair(newLeft, newRight);
                        left = newPair;
                        doneAny = true;
                    }
                }
                else
                {
                    left.TrySplit(ref doneAny);
                }

                if (!doneAny && right is Regular)
                {
                    // Does it need to split? 
                    int val = right.GetValue();
                    if (val >= 10)
                    {
                        // Yes! 
                        Regular newLeft = new Regular(val / 2);
                        Regular newRight = new Regular((val + 1) / 2);
                        Pair newPair = new Pair(newLeft, newRight);
                        right = newPair;
                        doneAny = true;
                    }
                }
                else
                {
                    right.TrySplit(ref doneAny);
                }

            }
        }

        public override void Print()
        {
            Console.Write('[');
            left.Print();
            Console.Write(',');
            right.Print();
            Console.Write(']');
        }
    }

    public class Regular : SFNumber
    {
        private int number;

        public Regular() { }

        public override SFNumber Clone()
        {
            return new Regular(number);
        }
        public Regular(int n)
        {
            number = n;
        }

        public void AddValue(int v)
        {
            number += v;
        }

        public override int GetValue()
        {
            return number;
        }

        public override int GetMagnitude()
        {
            return number;
        }

        public override void TryExplode(int depth, ref Regular leftNumber, ref int searchForRight, ref bool doneAny)
        {
            // All we need to do is record the nearest node in case there is an explosion later
            leftNumber = this;

            // And if there is a right value to add in, then do this. 
            if (searchForRight >= 0)
            {
                AddValue(searchForRight);
                searchForRight = -1;
            }
        }

        protected override void ParseThis(String line, ref int pos)
        {
            number = 0;
            while (Char.IsDigit(line[pos]))
            {
                number *= 10;
                number += line[pos++] - '0';
            }
        }

        public override void Print()
        {
            Console.Write(number);
        }
    }
}
