using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace _11
{
    internal class Program
    {
        static List<String> input = new List<String>();

        static void Main(string[] args)
        {
            ReadInput();
            ParseMonkeys();
            Part1();
            //Part2();
        }

        static void ReadInput()
        {
            using (StreamReader inputRdr = new StreamReader("d:\\adventofcode\\AOC2022\\11\\input_test.txt"))
            {
                while (!inputRdr.EndOfStream)
                {
                    String line = inputRdr.ReadLine();
                    input.Add(line);
                }
            }
        }

        static void ParseMonkeys()
        {
            Monkey currMonkey = null;
            foreach (String line in input)
            {
                string[] bits = line.Trim().Split(' ');
                switch (bits[0])
                {
                    case "Monkey":
                        currMonkey = Monkey.AddMonkey(Int32.Parse(bits[1].Split(':')[0]));
                        break;
                    case "Starting":
                        for (int i = 2; i < bits.Length; ++i)
                        {
                            currMonkey.items.Add(Int32.Parse(bits[i].Split(',')[0]));
                        }
                        break;
                    case "Operation:":
                        char op = bits[4][0];
                        if (op == '*' && bits[5] == "old")
                        {
                            currMonkey.opCode = '^';
                        }
                        else
                        {
                            currMonkey.opCode = op;
                            currMonkey.operand = Int32.Parse(bits[5]);
                        }
                        break;
                    case "Test:":
                        currMonkey.testVal = Int32.Parse(bits[3]);
                        break;
                    case "If":
                        int m = Int32.Parse(bits[5]);
                        if (bits[1] == "true:")
                            currMonkey.trueThrow = m;
                        else
                            currMonkey.falseThrow = m;
                        break;

                }
            }
        }

        static void Part1()
        {
            Console.WriteLine("Part1");
            Monkey.Part1();

        }

        public class Monkey
        {
            static List<Monkey> monkeys = new List<Monkey>();
            public List<int> items = new List<int>();
            public char opCode;
            public int operand;
            public int testVal;
            public int trueThrow;
            public int falseThrow;
            public int thingCount = 0;

            public static Monkey AddMonkey(int pos)
            {
                Monkey m = new Monkey();
                while (monkeys.Count <= pos)
                    monkeys.Add(new Monkey());
                monkeys[pos] = m;
                return m;
            }

            public void ThrowStuff()
            {
                thingCount += items.Count;
                foreach (int item in items)
                {
                    int val = item;
                    switch (opCode)
                    {
                        case '^': val *= item; break;
                        case '+': val += operand; break;
                        case '-': val -= operand; break;
                        case '*': val *= operand; break;
                        default: throw new Exception("Don't understand operator " + opCode);
                    }
                    // Part 1 worry levels
                    //val = val / 3;
                    // Part 2 worry levels
                    val = val / 3 + 52;
                    // Do the test
                    if (val % testVal == 0)
                    {
                        monkeys[trueThrow].Catch(val);
                    }
                    else
                    {
                        monkeys[falseThrow].Catch(val);
                    }
                }
                items.Clear();
            }

            public void Catch(int val)
            {
                items.Add(val);
            }

            public static void Round()
            {
                foreach(Monkey m in monkeys)
                {
                    m.ThrowStuff();
                }
            }

            public static void Part1()
            {
                Monkey.Round();
                for (int m = 0; m < monkeys.Count; ++m)
                {
                    Console.WriteLine($"Monkey {m} inspected {monkeys[m].thingCount} times");
                }
                for (int i = 1; i < 20; ++i)
                    Monkey.Round();

                for (int m = 0; m < monkeys.Count; ++m)
                {
                    Console.WriteLine($"Monkey {m} inspected {monkeys[m].thingCount} times");
                }

                int topCount = 1;
                int top2 = 1;
                foreach(Monkey m in monkeys)
                {
                    if (m.thingCount > topCount)
                    {
                        top2 = topCount;
                        topCount = m.thingCount;
                    }
                    else if (m.thingCount > top2)
                    {
                        top2 = m.thingCount;
                    }
                }

                Console.WriteLine($"Monkey business = {topCount * top2}");
            }
        }
    }
}
