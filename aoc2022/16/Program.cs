using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;

namespace _16
{
    internal class Program
    {
        static List<String> input = new List<String>();
        static Dictionary<String, Valve> valves = new Dictionary<string, Valve>();

        static void Main(string[] args)
        {
            ReadInput();
            Part1();
            Part2();
        }

        static void ReadInput()
        {
            using (StreamReader inputRdr = new StreamReader("d:\\adventofcode\\AOC2022\\16\\input.txt"))
            {
                while (!inputRdr.EndOfStream)
                {
                    String line = inputRdr.ReadLine();
                    input.Add(line);

                    // Parse.
                    // Valve AA has flow rate=0; tunnels lead to valves DD, II, BB
                    String[] parts = line.Split(' ');
                    String name = parts[1];
                    String flowStr = parts[4].Substring(5, parts[4].Length - 6);
                    Valve v = new Valve();
                    v.name = name;
                    v.flowrate = int.Parse(flowStr);
                    for (int i = 9; i< parts.Length; i++)
                    {
                        v.Add(parts[i].Substring(0, 2));
                    }
                    valves[name] = v;
                }
                foreach(Valve v in valves.Values)
                {
                    v.Resolve();
                }
            }
        }
        static int valvesToOpen = 0;
        static List<Valve> closedValves = new List<Valve>();

        static void Part1()
        {
            Console.WriteLine("Part1");
            foreach (Valve v in valves.Values)
            {
                if (v.flowrate > 0)
                {
                    valvesToOpen++;
                    closedValves.Add(v);
                }
            }
            closedValves.Sort(delegate (Valve l, Valve r)
            {
                return r.flowrate - l.flowrate;
            });
            valves["AA"].FindRoutes();
            foreach (Valve v in closedValves)
            {
                v.FindRoutes();
            }

            // OK, now we can find out the best answer.
            int best = valves["AA"].Walk(30, "", 0, 0);
           
            Console.Write($"best was {best}");
        }

        private static void Part2()
        {
            Console.WriteLine("Part2");
            int bestFlow = 0;
            for (long l=3; l<(long)1 << (valvesToOpen-1); l++)
            {
                String doneList1 = "";
                String doneList2 = "";
                int ones = 0;
                for (int i=0; i<valvesToOpen; ++i)
                {
                    if ((((long)1 << i) & l) == 0)
                    {
                        doneList1 = $"{doneList1},{closedValves[i].name}";
                    }
                    else
                    {
                        ones++;
                        doneList2 = $"{doneList2},{closedValves[i].name}";
                    }
                }
                if (ones < valvesToOpen / 4 || ones > 3 * valvesToOpen / 4)
                    continue;

                // Got a mix of the two, find both values. 
                int flow = valves["AA"].Walk(26, doneList1, 0, 0) + valves["AA"].Walk(26, doneList2, 0, 0);
                if (flow > bestFlow)
                    bestFlow = flow;

            }
            Console.WriteLine($"best with Ele {bestFlow}");
        }

        public class Valve
        {
            public String name;
            public int flowrate;
            public List<Valve> tunnels = new List<Valve>();
            public List<String> tunnelNames = new List<string>();
            public List<(Valve, int)> tunnelsAndTimes = new List<(Valve, int)>();

            public int Walk(int tLeft, String visited, int flow, int bestFlow)
            {
                // Check if there is any point
                if (tLeft <= 1)
                {
                    if (flow > bestFlow)
                        bestFlow = flow;
                    return bestFlow;
                }

                //int maxLeft = 0;
                //int maxtLeft = tLeft;
                //foreach (Valve v in closedValves)
                //{
                //    if (!openValves.Contains(v.name))
                //    {
                //        maxLeft += v.flowrate * maxtLeft;
                //        maxtLeft -= 2;
                //        if (maxtLeft < 0)
                //            break;
                //    }
                //}
                //if (maxLeft + flow < bestFlow)
                //    return;
                visited = $"{visited},{name}";
                if (flowrate > 0)
                {
                    flow = flow + flowrate * (--tLeft);
                }
                bool doneAny = false;
                foreach((Valve, int)valveAndTime in tunnelsAndTimes)
                {
                    if (!visited.Contains(valveAndTime.Item1.name))
                    {
                        if (valveAndTime.Item2 < tLeft)
                        {
                            doneAny = true;
                            bestFlow = valveAndTime.Item1.Walk(tLeft - valveAndTime.Item2, visited, flow, bestFlow);
                        }
                    }
                }
                // no more? 
                if (!doneAny)
                {
                    if (flow > bestFlow)
                        bestFlow= flow;
                }
                return bestFlow;
            }

            public void Add(String name)
            {
                tunnelNames.Add(name);
            }

            public void Resolve()
            {
                foreach (String nm in tunnelNames)
                {
                    tunnels.Add(valves[nm]);
                }
                tunnels.Sort(delegate (Valve l, Valve r)
                {
                    return l.flowrate < r.flowrate ? 1 : -1;
                });
            }

            public void FindRoutes()
            {
                // Only applies to valves that we can open
                if (flowrate != 0 || name == "AA")
                {
                    int time = 0;
                    String visited = name;
                    List<Valve> edge = new List<Valve>();
                    edge.Add(this);
                    do
                    {
                        List<Valve> newEdge = new List<Valve>();
                        time++;
                        foreach (Valve v in edge)
                        {
                            foreach (Valve v2 in v.tunnels)
                            {
                                if (!visited.Contains(v2.name))
                                {
                                    newEdge.Add(v2);
                                    if (v2.flowrate != 0)
                                    {
                                        tunnelsAndTimes.Add((v2, time));
                                    }
                                    visited = $"{visited},{v2.name}";
                                }
                            }
                        }
                        edge = newEdge;
                    } while (edge.Count > 0);
                }
            }
        }
    }
}

/* THis didn't work for part 2!! 
static int bestFlow = 0;
static int valvesToOpen = 0;
static List<Valve> closedValves = new List<Valve>();

static void Part1()
{
    Console.WriteLine("Part1");
    foreach(Valve v in valves.Values)
    {
        if (v.flowrate > 0)
        {
            valvesToOpen++;
            closedValves.Add(v);
        }
    }
    closedValves.Sort(delegate (Valve l, Valve r)
    {
        return r.flowrate - l.flowrate;
    });

    WalkTunnel(valves["AA"], 30, "", 0);
    Console.WriteLine($"Best flow is {bestFlow}");
}

private static void WalkTunnel(Valve val, int tLeft, String openValves, int flow)
{
    // try all the combinations, and add them to the running total. No more time to open a valve if only 1 minute left.
    if (--tLeft <= 1)
    {
        if (flow > bestFlow)
            bestFlow = flow;
        return;
    }

    int maxLeft = 0;
    int maxtLeft = tLeft;
    foreach (Valve v in closedValves)
    {
        if (!openValves.Contains(v.name))
        {
            maxLeft += v.flowrate * maxtLeft;
            maxtLeft -= 2;
            if (maxtLeft < 0)
                break;
        }
    }
    if (maxLeft + flow < bestFlow)
        return;
    if (val.flowrate > 0 && !openValves.Contains(val.name))
    {
        // Worth trying this one. Keep this seperate to the tunnel walk, so we consider both.
        String newOpenValves = $"{openValves},{val.name}";
        int newFlow = flow + val.flowrate * tLeft;

        // any more to do? 
        if (newOpenValves.Length >= valvesToOpen * 3)
        {
            if (newFlow > bestFlow)
                bestFlow = newFlow;
            return;
        }

        // Now go down each tunnel.
        foreach (Valve t in val.tunnels)
        {
            WalkTunnel(t, tLeft-1, newOpenValves, newFlow);
        }
    }

    // Now go down each tunnel. This is trying without opening the valve here.
    foreach (Valve t in val.tunnels)
    {
        WalkTunnel(t, tLeft, openValves, flow);
    }
}

static void Part2()
{
    Console.WriteLine("Part2");
    //bestFlow = 0;

    JustAMinute(valves["AA"], valves["AA"], 26, "", 0);
    Console.WriteLine($"Bext flow is {bestFlow}");
}

private static void JustAMinute(Valve val, Valve eleVal, int tLeft, String openValves, int flow)
{
    // try all the combinations, and add them to the running total. No more time to open a valve if only 1 minute left.
    if (--tLeft <= 1)
    {
        if (flow > bestFlow)
            bestFlow = flow;
        return;
    }

    int maxLeft = 0;
    int maxtLeft = tLeft;
    bool both = false;
    foreach (Valve v in closedValves)
    {
        if (!openValves.Contains(v.name))
        {
            maxLeft += v.flowrate * maxtLeft;
            if (!both)
                both = true;
            else
            {
                both = false;
                maxtLeft -= 2;
            }
            if (maxtLeft < 0)
                break;
        }
    }
    if (maxLeft + flow < bestFlow)
        return;

    // Need to do a single action for each (val and eleVal) and then recurse. So try all combinations of both. 
    List<Valve> fullList = new List<Valve>();
    fullList.Add(val);
    fullList.AddRange(val.tunnels);
    List<Valve> eleList = new List<Valve>();
    eleList.Add(eleVal);
    eleList.AddRange(eleVal.tunnels);
    int newFlow = flow;
    String newOpenValves = openValves;
    foreach (Valve v in fullList)
    {
        // Treat the actual chamber as 'open this valve'.
        if (v == val)
        {
            if (val.flowrate > 0 && !newOpenValves.Contains(val.name))
            {
                // Worth trying this one. Keep this seperate to the tunnel walk, so we consider both.
                newOpenValves = $"{newOpenValves},{val.name}";
                newFlow = newFlow + val.flowrate * tLeft;

                // any more to do? 
                if (newOpenValves.Length >= valvesToOpen * 3)
                {
                    if (newFlow > bestFlow)
                        bestFlow = newFlow;
                    return;
                }
            }
            else
            {
                continue;
            }
        }

        // Avoid the same one that we are on!
        if (eleList.Contains(v))
        {
            // Do it last.
            eleList.Remove(v);
            eleList.Add(v);
        }
        foreach(Valve ev in eleList)
        {
            int oldFlow = newFlow;
            String oldOpenValves = newOpenValves;
            if (ev == eleVal)
            {
                if (ev.flowrate > 0 && !newOpenValves.Contains(ev.name))
                {
                    // Worth trying this one. Keep this seperate to the tunnel walk, so we consider both.
                    newOpenValves = $"{newOpenValves},{ev.name}";
                    newFlow = newFlow + ev.flowrate * tLeft;

                    // any more to do? 
                    if (newOpenValves.Length >= valvesToOpen * 3)
                    {
                        if (newFlow > bestFlow)
                            bestFlow = newFlow;
                        return;
                    }
                }
                else
                {
                    continue;
                }
            }

            // Now we can recurse, as we have done a single minute for each of me and ele. 
            JustAMinute(v, ev, tLeft, newOpenValves, newFlow);
            newFlow = oldFlow;
            newOpenValves = oldOpenValves;
        }
        newFlow = flow;
        newOpenValves = openValves;
    }
}
*/

