using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace _19
{
    internal class Program
    {
        static List<Blueprint> blueprints = new List<Blueprint>();

        static void Main(string[] args)
        {
            ReadInput();
            Part1();
            Part2();
        }

        static void ReadInput()
        {
            using (StreamReader inputRdr = new StreamReader("d:\\adventofcode\\AOC2022\\19\\input.txt"))
            {
                while (!inputRdr.EndOfStream)
                {
                    String line = inputRdr.ReadLine();
                    blueprints.Add(new Blueprint(line));
                }
            }
        }

        static void Part1()
        {
            Console.WriteLine("Part1");
            int qualityLevels = 0;
            foreach (Blueprint blueprint in blueprints)
            {
                int geos = blueprint.MakeABot(24, 0, 0, 0, 0, 1, 0, 0, 0, 0);
                qualityLevels += blueprint.number * geos;
            }
            Console.WriteLine($"Total Quality {qualityLevels}");
        }

        static void Part2()
        {
            Console.WriteLine("Part2");
            int qualityLevels = 1;
            for (int bp=0; bp<3; ++bp)
            {
                Blueprint blueprint = blueprints[bp];
                int geos = blueprint.MakeABot(32, 0, 0, 0, 0, 1, 0, 0, 0, 0);
                qualityLevels *= geos;
            }
            Console.WriteLine($"Total Quality {qualityLevels}");
        }

    public class Blueprint
        {
            public int numOreForOre;
            public int numOreForClay;
            public int numOreForObsidian;
            public int numClayForObsidian;
            public int numOreForGeode;
            public int numObsidianForGeode;
            public int number;
            public int maxOreNeeded;

            public int MakeABot(int tLeft, int ore, int clay, int obs, int geo, int oreRob, int clayRob, int obsRob, int geoRob, int maxGeos)
            {
                // Decide which bots we can save up for. 
                int time = 0;
                int newOre = ore;
                int newClay = clay;
                int newObs = obs;
                int newGeo = geo;

                if (tLeft <= 0)
                {
                    if (geo > maxGeos)
                        return geo;
                    return maxGeos;
                }

                // No limit on geode bots.
                if (obsRob > 0)
                {
                    // We can save up for one!
                    int obsTime = 0, oreTime = 0;
                    if (obs < numObsidianForGeode)
                    {
                        obsTime = (numObsidianForGeode - obs + obsRob - 1) / obsRob;
                    }
                    if (ore < numOreForGeode)
                    {
                        oreTime += (numOreForGeode - ore + oreRob - 1) / oreRob;
                    }
                    time = Math.Max(obsTime, oreTime);
                    time++;
                    if (time < tLeft)
                    {
                        newOre += time * oreRob;
                        newClay += time * clayRob;
                        newObs += time * obsRob;
                        newGeo += time * geoRob;

                        // OK, make a geode robot
                        newObs -= numObsidianForGeode;
                        newOre -= numOreForGeode;
                        maxGeos = MakeABot(tLeft - time, newOre, newClay, newObs, newGeo, oreRob, clayRob, obsRob, geoRob + 1, maxGeos);
                    }
                }

                // Or, we could make an obsidian robot.
                time = 0;
                newOre = ore;
                newClay = clay;
                newObs = obs;
                newGeo = geo;
                if (clayRob > 0 && obsRob < numObsidianForGeode)
                {
                    // We can save up for one!
                    int clayTime = 0, oreTime = 0;

                    if (clay < numClayForObsidian)
                    {
                        clayTime = (numClayForObsidian - clay + clayRob - 1) / clayRob;
                    }
                    if (ore < numOreForObsidian)
                    {
                        oreTime += (numOreForObsidian - ore + oreRob - 1) / oreRob;
                    }
                    time = Math.Max(clayTime, oreTime);
                    time++;
                    if (time < tLeft)
                    {
                        newOre += time * oreRob;
                        newClay += time * clayRob;
                        newObs += time * obsRob;
                        newGeo += time * geoRob;

                        // OK, make an obsidian robot
                        newClay -= numClayForObsidian;
                        newOre -= numOreForObsidian;
                        maxGeos = MakeABot(tLeft - time, newOre, newClay, newObs, newGeo, oreRob, clayRob, obsRob + 1, geoRob, maxGeos);
                    }
                }

                // Or, we could make a clay robot.
                time = 0;
                newOre = ore;
                newClay = clay;
                newObs = obs;
                newGeo = geo;
                if (clayRob < numClayForObsidian)
                {
                    // We can save up for one!
                    if (ore < numOreForClay)
                    {
                        time += (numOreForClay - ore + oreRob - 1) / oreRob;
                    }
                    time++;
                    if (time < tLeft)
                    {
                        newOre += time * oreRob;
                        newClay += time * clayRob;
                        newObs += time * obsRob;
                        newGeo += time * geoRob;

                        // OK, make a clay robot
                        newOre -= numOreForClay;
                        maxGeos = MakeABot(tLeft - time, newOre, newClay, newObs, newGeo, oreRob, clayRob + 1, obsRob, geoRob, maxGeos);
                    } 
                }

                // Or, we could make an ore robot
                time = 0;
                newOre = ore;
                newClay = clay;
                newObs = obs;
                newGeo = geo;
                if (oreRob < maxOreNeeded)
                {
                    // Lets save!
                    if (ore < numOreForOre)
                    {
                        time += (numOreForOre - ore + oreRob - 1) / oreRob; 
                    }
                    time++;
                    if (time < tLeft)
                    {
                        newOre += time * oreRob;
                        newClay += time * clayRob;
                        newObs += time * obsRob;
                        newGeo += time * geoRob;

                        // OK, make an ore robot
                        newOre -= numOreForOre;
                        maxGeos = MakeABot(tLeft - time, newOre, newClay, newObs, newGeo, oreRob + 1, clayRob, obsRob, geoRob, maxGeos);
                    }
                }

                // As a last resort, we could just run out of time with the bots we have
                if (geoRob > 0)
                {
                    geo += tLeft * geoRob;
                    if (geo > maxGeos)
                        maxGeos = geo;
                }
                return maxGeos;
            }

            public Blueprint(String line)
            {
                //Blueprint 1: Each ore robot costs 3 ore. Each clay robot costs 3 ore. Each obsidian robot costs 3 ore and 19 clay. Each geode robot costs 3 ore and 17 obsidian.
                String[] bits = line.Split(' ');
                number = int.Parse(bits[1].Substring(0, bits[1].Length - 1));
                numOreForOre = int.Parse(bits[6]);
                numOreForClay = int.Parse(bits[12]);
                numOreForObsidian = int.Parse(bits[18]);
                numClayForObsidian = int.Parse(bits[21]);
                numOreForGeode = int.Parse(bits[27]);
                numObsidianForGeode = int.Parse(bits[30]);
                maxOreNeeded = Math.Max(numOreForOre, Math.Max(numOreForClay, Math.Max(numOreForObsidian, numOreForGeode)));
            }
        }
    }
}
