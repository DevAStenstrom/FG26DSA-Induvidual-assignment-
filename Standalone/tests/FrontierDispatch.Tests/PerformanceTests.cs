using System;
using System.Collections.Generic;
using System.Diagnostics;
using FrontierDispatch.Core;
using NUnit.Framework;

namespace FrontierDispatch.Tests
{
    public sealed class PerformanceTests
    {
        private const int LocationCount = 20000;
        private const int QueryCount = 2000;
        
        [Test]
        public void Measure_Prepare()
        {
            WorldData world = CreateLargeWorld();
            Stopwatch stopwatch = Stopwatch.StartNew();
            WorldQueries queries = new WorldQueries(world);
            stopwatch.Stop();

            Console.WriteLine($"Prepare WorldQueries for {LocationCount} locations: {stopwatch.Elapsed.TotalMilliseconds} ms");

            Assert.That(queries.FindLocationById(0), Is.Not.Null);
        }

        //FindLocationById old implementation vs mine
        [Test]
        public void Measure_FindLocationById()
        {
            WorldData world = CreateLargeWorld();
            WorldQueries queries = new WorldQueries(world);
            
            int foundBefore = 0;
            Stopwatch stopwatch = Stopwatch.StartNew();
            for (int id = 0; id < QueryCount; ++id)
            {
                for (int i = 0; i < world.Locations.Count; ++i)
                {
                    if (world.Locations[i].Id == id)
                    {
                        ++foundBefore;
                        break;
                    }
                }
            }
            stopwatch.Stop();
            Console.WriteLine($"FindLocationById before: {stopwatch.Elapsed.TotalMilliseconds} ms");

            // After: dictionary lookup.
            int foundAfter = 0;
            stopwatch = Stopwatch.StartNew();
            for (int id = 0; id < QueryCount; ++id)
            {
                if (queries.FindLocationById(id) != null)
                {
                    ++foundAfter;
                }
            }
            stopwatch.Stop();
            Console.WriteLine($"FindLocationById after:  {stopwatch.Elapsed.TotalMilliseconds} ms");

            //check to make sure both have the same count
            Assert.That(foundAfter, Is.EqualTo(foundBefore));
        }

        
        [Test]
        public void Measure_FindOutgoingConnections()
        {
            WorldData world = CreateLargeWorld();
            WorldQueries queries = new WorldQueries(world);
            
            int connectionsBefore = 0;
            long bytesAtStart = GC.GetAllocatedBytesForCurrentThread(); //get memory currently allocated
            Stopwatch stopwatch = Stopwatch.StartNew();
            for (int id = 0; id < QueryCount; ++id)
            {
                List<Connection> result = new List<Connection>();
                for (int i = 0; i < world.Connections.Count; ++i)
                {
                    if (world.Connections[i].FromLocationId == id)
                    {
                        result.Add(world.Connections[i]);
                    }
                }
                connectionsBefore += result.Count;
            }
            stopwatch.Stop();
            long bytesBefore = GC.GetAllocatedBytesForCurrentThread() - bytesAtStart;
            Console.WriteLine($"FindOutgoingConnections before: {stopwatch.Elapsed.TotalMilliseconds} ms with {bytesBefore} bytes allocated");
            
            
            
            int connectionsAfter = 0;
            bytesAtStart = GC.GetAllocatedBytesForCurrentThread();
            stopwatch = Stopwatch.StartNew();
            for (int id = 0; id < QueryCount; ++id)
            {
                connectionsAfter += queries.FindOutgoingConnections(id).Count;
            }
            stopwatch.Stop();
            long bytesAfter = GC.GetAllocatedBytesForCurrentThread() - bytesAtStart;
            Console.WriteLine($"FindOutgoingConnections after: {stopwatch.Elapsed.TotalMilliseconds} ms with {bytesAfter} bytes allocated");

            Assert.That(connectionsAfter, Is.EqualTo(connectionsBefore));
        }
        
        [Test]
        public void Measure_Routes()
        {
            WeightedGraph graph = CreateLargeGraph();
            DijkstraRouteFinder finder = new DijkstraRouteFinder();
            int routeCount = 20;

            int found = 0;
            int expandedNodes = 0;
            Stopwatch stopwatch = Stopwatch.StartNew();
            for (int i = 0; i < routeCount; ++i)
            {
                string start = i.ToString();
                string goal = (i + LocationCount / 2).ToString();

                RouteResult result = finder.FindRoute(graph, start, goal);
                if (result.Found)
                {
                    ++found;
                }
                expandedNodes += result.ExpandedNodes;
            }
            stopwatch.Stop();

            Console.WriteLine($"FindRoute {stopwatch.Elapsed.TotalMilliseconds} ms with {expandedNodes} nodes");

            Assert.That(found, Is.EqualTo(routeCount));
        }


        private static WorldData CreateLargeWorld()
        {
            Random random = new Random();
            List<Location> locations = new List<Location>();
            List<Connection> connections = new List<Connection>();

            //iterates by LocationCount and adding random Locations
            for (int id = 0; id < LocationCount; ++id)
            {
                locations.Add(new Location(id, "Location " + id, new WorldPosition(0, 0), "RandomNOde"));
                //using cjs modulo-operator function to set each connection
                connections.Add(new Connection(id, (id + 1) % LocationCount, random.Next(1, 10)));
                connections.Add(new Connection(id, random.Next(0, LocationCount), random.Next(1, 10)));
            }

            return new WorldData(locations, connections);
        }
        
        private static WeightedGraph CreateLargeGraph()
        {
            Random random = new Random();
            List<WeightedNode> nodes = new List<WeightedNode>();
            List<WeightedEdge> edges = new List<WeightedEdge>();

            
            //same  as LargeWorld but for Nodes instead
            for (int id = 0; id < LocationCount; ++id)
            {
                nodes.Add(new WeightedNode(id.ToString(), 0, 0));
                edges.Add(new WeightedEdge(id.ToString(), ((id + 1) % LocationCount).ToString(), random.Next(1, 10)));
                edges.Add(new WeightedEdge(id.ToString(), random.Next(0, LocationCount).ToString(), random.Next(1, 10)));
            }

            return new WeightedGraph(nodes, edges);
        }
    }
}
