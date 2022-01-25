using System;
using System.Collections.Generic;

namespace _16
{
    class Program
    {
        //// Actual input.
        const String input = "420D5A802122FD25C8CD7CC010B00564D0E4B76C7D5A59C8C014E007325F116C958F2C7D31EB4EDF90A9803B2EB5340924CA002761803317E2B4793006E28C2286440087C5682312D0024B9EF464DF37EFA0CD031802FA00B4B7ED2D6BD2109485E3F3791FDEB3AF0D8802A899E49370012A926A9F8193801531C84F5F573004F803571006A2C46B8280008645C8B91924AD3753002E512400CC170038400A002BCD80A445002440082021DD807C0201C510066670035C00940125D803E170030400B7003C0018660034E6F1801201042575880A5004D9372A520E735C876FD2C3008274D24CDE614A68626D94804D4929693F003531006A1A47C85000084C4586B10D802F5977E88D2DD2898D6F17A614CC0109E9CE97D02D006EC00086C648591740010C8AF14E0E180253673400AA48D15E468A2000ADCCED1A174218D6C017DCFAA4EB2C8C5FA7F21D3F9152012F6C01797FF3B4AE38C32FFE7695C719A6AB5E25080250EE7BB7FEF72E13980553CE932EB26C72A2D26372D69759CC014F005E7E9F4E9FA7D3653FCC879803E200CC678470EC0010E82B11E34080330D211C663004F00101911791179296E7F869F9C017998EF11A1BCA52989F5EA778866008D8023255DFBB7BD2A552B65A98ECFEC51D540209DFF2FF2B9C1B9FE5D6A469F81590079160094CD73D85FD2699C5C9DCF21F0700094A1AC9EDA64AE3D37D34200B7B401596D678A73AFB2D0B1B88057230A42B2BD88E7F9F0C94F1ECB7B0DD393489182F9802D3F875C00DC40010F8911C61F8002111BA1FC2E400BEA5AA0334F9359EA741892D81100B83337BD2DDB4E43B401A800021F19A09C1F1006229C3F8726009E002A12D71B96B8E49BB180273AA722468002CC7B818C01B04F77B39EFDF53973D95ADB5CD921802980199CF4ADAA7B67B3D9ACFBEC4F82D19A4F75DE78002007CD6D1A24455200A0E5C47801559BF58665D80";
        //// Sample 1 - nested operator packets.
        //const String input = "8A004A801A8002F478";
        //// Sample 2 - nested with 2 literals in each 
        //const String input = "620080001611562C8802118E34";
        // Sample 3 - simple packet
        //const String input = "D2FE28";

        static byte[] data;

        static void Main(string[] args)
        {
            ParseInput(input);
            Packet pkt = new Packet();
            pkt.Parse(data, 0);
            Console.WriteLine($"Total versions is {pkt.SumVersions()}");
            Console.WriteLine($"Value of packet is {pkt.GetValue()}");
        }

        static void ParseInput(String str)
        {
            if ((str.Length & 1) == 1)
                str = str + '0';
            data = new byte[str.Length / 2];
            for (int x=0; x<str.Length; x += 2)
            {
                data[x / 2] = Byte.Parse(str.Substring(x, 2), System.Globalization.NumberStyles.HexNumber);
            }
        }
    }

    public class Packet
    {
        private int version;
        private int type;
        private long value;
        private List<Packet> subPackets = new List<Packet>();

        public Packet() { } 
        public int Parse(Byte[] data, int pos)
        {
            int maxBits = int.MaxValue;
            int maxPackets = int.MaxValue;
            int subPacketStart = 0;
            version = ReadInt32(data, pos, 3);
            pos += 3;
            type = ReadInt32(data, pos, 3);
            pos += 3;

            // Do the right thing with the type. 
            if (type == 4)
            {
                value = 0;
                bool cont;
                // Read a constant. 
                do
                {
                    cont = ReadBit(data, pos++) == 1;
                    value <<= 4;
                    value += (long)ReadInt32(data, pos, 4);
                    pos += 4;
                } while (cont);
            }
            else
            {
                // All other packets have sub-packets. 
                int lengthType = ReadBit(data, pos++);
                if (lengthType == 0)
                {
                    // Next 15 bits is the length in bits
                    maxBits = ReadInt32(data, pos, 15);
                    pos += 15;
                    subPacketStart = pos;                   
                }
                else
                {
                    // Next 11 bits is number of sub-packets.
                    maxPackets = ReadInt32(data, pos, 11);
                    pos += 11;
                }

                // OK, parse the remainder out into sub packets. 
                do
                {
                    Packet subPkt = new Packet();
                    pos = subPkt.Parse(data, pos);
                    subPackets.Add(subPkt);
                } while (subPackets.Count < maxPackets && (pos - subPacketStart) < maxBits);
            }
            return pos;
        }
        public int SumVersions()
        {
            int res = version;
            foreach (Packet pkt in subPackets)
                res += pkt.SumVersions();
            return res;
        }

        public long GetValue()
        {
            // Use the type to determine what to do. 
            switch (type)
            {
                case 0: // Sum
                    value = 0;
                    foreach (Packet p in subPackets)
                        value += p.GetValue();
                    break;
                case 1: // Product
                    value = 1;
                    foreach (Packet p in subPackets)
                        value *= p.GetValue();
                    break;
                case 2: // Minimum
                    value = int.MaxValue;
                    foreach (Packet p in subPackets)
                    {
                        long pVal = p.GetValue();
                        if (pVal < value)
                            value = pVal;
                    }
                    break;
                case 3: // Maximum
                    value = int.MinValue;
                    foreach (Packet p in subPackets)
                    {
                        long pVal = p.GetValue();
                        if (pVal > value)
                            value = pVal;
                    }
                    break;
                case 4: // Literal - value is set
                    break;
                case 5: // GT
                    Packet p1 = subPackets[0];
                    Packet p2 = subPackets[1];
                    value = (p1.GetValue() > p2.GetValue()) ? 1 : 0;
                    break;
                case 6: // LT
                    p1 = subPackets[0];
                    p2 = subPackets[1];
                    value = (p1.GetValue() < p2.GetValue()) ? 1 : 0;
                    break;
                case 7: // EQ
                    p1 = subPackets[0];
                    p2 = subPackets[1];
                    value = (p1.GetValue() == p2.GetValue()) ? 1 : 0;
                    break;
            }
            return value;
        }

        private static Int32 ReadInt32(Byte[] data, int pos, int size)
        {
            Int32 result = 0;
            for (int i = 0; i < size; ++i)
            {
                result <<= 1;
                result = result + ReadBit(data, pos++);
            }
            return result;
        }

        private static int ReadBit(Byte[] data, int pos)
        {
            int offset = 7 - (pos % 8);
            Byte mask = 1;
            mask <<= offset;
            return ((data[pos / 8] & mask) > 0) ? 1: 0;
        }
    }
}
