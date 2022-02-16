using System;
using System.Collections.Generic;
using System.IO;

namespace _24
{
    class Program
    {
        static void Main(string[] args)
        {
            ReadInput();
            Part1();
            //Part2();
        }

        static void ReadInput()
        {
            using (StreamReader inputRdr = new StreamReader("d:\\adventofcode\\AOC2021\\24\\input.txt"))
            {
                while (!inputRdr.EndOfStream)
                {
                    String line = inputRdr.ReadLine();
                    OpCode.Parse(line);
                }
            }
        }

        static void Part1()
        {
            for (int i0 = 9; i0 > 0; --i0)
            {
                OpCode.input[0].v = i0;
                ALU.inputs[0] = i0;
                for (int i1 = 9; i1 > 0; --i1)
                {
                    OpCode.input[1].v = i1;
                    ALU.inputs[1] = i1;
                    for (int i2 = 9; i2 > 0; --i2)
                    {
                        OpCode.input[2].v = i2;
                        ALU.inputs[2] = i2;
                        for (int i3 = 9; i3 > 0; --i3)
                        {
                            OpCode.input[3].v = i3;
                            ALU.inputs[3] = i3;
                            for (int i4 = 9; i4 > 0; --i4)
                            {
                                OpCode.input[4].v = i4;
                                ALU.inputs[4] = i4;
                                for (int i5 = 9; i5 > 0; --i5)
                                {
                                    OpCode.input[5].v = i5;
                                    ALU.inputs[5] = i5;
                                    for (int i6 = 9; i6 > 0; --i6)
                                    {
                                        OpCode.input[6].v = i6;
                                        ALU.inputs[6] = i6;
                                        for (int i7 = 9; i7 > 0; --i7)
                                        {
                                            OpCode.input[7].v = i7;
                                            ALU.inputs[7] = i7;
                                            for (int i8 = 9; i8 > 0; --i8)
                                            {
                                                OpCode.input[8].v = i8;
                                                ALU.inputs[8] = i8;
                                                for (int i9 = 9; i9 > 0; --i9)
                                                {
                                                    OpCode.input[9].v = i9;
                                                    ALU.inputs[9] = i9;
                                                    for (int i10 = 9; i10 > 0; --i10)
                                                    {
                                                        OpCode.input[10].v = i10;
                                                        ALU.inputs[10] = i10;
                                                        for (int i11 = 9; i11 > 0; --i11)
                                                        {
                                                            OpCode.input[11].v = i11;
                                                            ALU.inputs[11] = i11;
                                                            for (int i12 = 9; i12 > 0; --i12)
                                                            {
                                                                OpCode.input[12].v = i12;
                                                                ALU.inputs[12] = i12; 
                                                                for (int i13 = 9; i13 > 0; --i13)
                                                                {
                                                                    OpCode.input[13].v = i13;
                                                                    ALU.inputs[13] = i13;
                                                                    //int z1 = OpCode.Execute();
                                                                    int z2 = ALU.Run();
                                                                    //if (z2 != z1)
                                                                      //  throw new Exception("Gone wrong");
                                                                    if (z2 == 0)
                                                                    {
                                                                        // Done it. 
                                                                        Console.WriteLine($"Found Lowest at {i0}{i1}{i2}{i3}{i4}{i5}{i6}{i7}{i8}{i9}{i10}{i11}{i12}{i13}");
                                                                        return;
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    // Loop through this: 
        //inp w
        //mul x 0
        //add x z
        //mod x 26
        //div z 1
        //add x 15
        //eql x w
        //eql x 0
        //mul y 0
        //add y 25
        //mul y x
        //add y 1
        //mul z y
        //mul y 0
        //add y w
        //add y 15
        //mul y x
        //add z y

    // ALU
    public class ALU
    {
        public static int w;
        public static int x;
        public static int y;
        public static int z;
        public static int[] inputs = new int[14];

        public static int[] firstParam =    { 0,   0,  0,   1,  0,  1,  0,  0,  0,  1,  1,  1,  1, 1 };
        public static int[] secondParam =   { 15, 12, 13, -14, 15, -7, 14, 15, 15, -7, -8, -7, -5, -10 };
        public static int[] thirdParam =    { 15,  5,  6,   7,  9,  6, 14,  3,  1,  3,  4,  6,  7, 1 };

        public ALU() { }

        public static int Run()
        {
            w = x = y = z = 0;
            for (int loop = 0; loop < firstParam.Length; ++loop)
            {
                //inp w
                w = inputs[loop];
                //mul x 0
                //add x z
                //mod x 26
                x = z % 26;
                //div z 1 or 26
                if (firstParam[loop] > 0)
                    z /= 26;
                //add x 15
                x += secondParam[loop];
                //eql x w
                //eql x 0
                //mul y 0
                //add y 25
                //mul y x
                //add y 1
                if (x != w)
                {
                    y = 26;
                    //mul z y
                    z *= y;
                    //mul y 0
                    //add y w
                    //add y 15
                    y = w + thirdParam[loop];
                    //mul y x
                    //add z y
                    z += y;
                }
            }
            return z;
        }

    }

    // Operands.
    public class Operand
    {
        public int v { get; set; }
        public Operand() { }
    }

    // Instructions
    public class OpCode
    {
        protected String origText;
        protected Operand op1, op2;

        // Registers
        static public Operand w = new Operand();
        static public Operand x = new Operand();
        static public Operand y = new Operand();
        static public Operand z = new Operand();

        // Inputs
        static public Operand[] input = new Operand[14];
        static private int inputCount = 0;

        // Constants
        static private List<Operand> constants = new List<Operand>();

        // Actual codes
        static private List<OpCode> opCodes = new List<OpCode>();

        public OpCode(String line, Operand o1, Operand o2)
        {
            origText = line;
            op1 = o1;
            op2 = o2;
        }

        // Run. Assume the input has been set.
        public static int Execute()
        {
            // Zero. 
            w.v = x.v = y.v = z.v = 0;
            foreach(OpCode oc in opCodes)
            {
                oc.Do();
            }
            return z.v;
        }

        // Virtual constructor
        public static void Parse(String line)
        {
            // Work out the operands. 
            String[] parts = line.Split(' ');

            Operand op1 = null, op2 = null;
            if (parts.Length >= 2)
            {
                op1 = GetOperandForString(parts[1]);
            }
            if (parts.Length == 3)
            {
                op2 = GetOperandForString(parts[2]);
            }

            OpCode oc;
            // Decide what to create based on the first bit.
            switch (parts[0].Trim())
            {
                case "inp":
                    input[inputCount] = new Operand();
                    oc = new OpInp(line, op1, input[inputCount++]); break;
                case "add": oc = new OpAdd(line, op1, op2); break;
                case "mul": oc = new OpMul(line, op1, op2); break;
                case "div": oc = new OpDiv(line, op1, op2); break;
                case "mod": oc = new OpMod(line, op1, op2); break;
                case "eql": oc = new OpEql(line, op1, op2); break;

                default: throw new Exception("Don't know about " + line);
            }

            opCodes.Add(oc);
        }

        private static Operand GetOperandForString(String s)
        {
            switch (s)
            {
                case "w": return w;
                case "x": return x;
                case "y": return y;
                case "z": return z;
            }

            // If its a constant, make us a new one. 
            int c;
            if (Int32.TryParse(s, out c))
            {
                Operand constOp = new Operand();
                constOp.v = c;
                return constOp;
            }
            return null;
        }

        public virtual void Do() { }
    }

    public class OpInp : OpCode
    {
        public OpInp(String l, Operand op1, Operand op2) : base(l, op1, op2) { }

        public override void Do()
        {
            op1.v = op2.v;
        }
    }

    public class OpAdd : OpCode
    {
        public OpAdd(String l, Operand op1, Operand op2) : base(l, op1, op2) { }

        public override void Do()
        {
            op1.v = op1.v + op2.v;
        }
    }

    public class OpMul : OpCode
    {
        public OpMul(String l, Operand op1, Operand op2) : base(l, op1, op2) { }

        public override void Do()
        {
            op1.v = op1.v * op2.v;
        }
    }
    public class OpDiv : OpCode
    {
        public OpDiv(String l, Operand op1, Operand op2) : base(l, op1, op2) { }

        public override void Do()
        {
            op1.v = op1.v / op2.v;
        }
    }
    public class OpMod : OpCode
    {
        public OpMod(String l, Operand op1, Operand op2) : base(l, op1, op2) { }

        public override void Do()
        {
            op1.v = op1.v % op2.v;
        }
    }

    public class OpEql : OpCode
    {
        public OpEql(String l, Operand op1, Operand op2) : base(l, op1, op2) { }

        public override void Do()
        {
            op1.v = (op1.v == op2.v) ? 1 : 0;
        }
    }



}
