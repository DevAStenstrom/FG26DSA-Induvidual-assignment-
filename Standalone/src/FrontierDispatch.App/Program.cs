using System;
using FrontierDispatch.Core;

WorldData world = SampleWorldFactory.CreateSmall();
var queries = new WorldQueries(world);

Console.WriteLine("Frontier Dispatch - headless baseline");
Console.WriteLine($"Locations: {world.Locations.Count}");
Console.WriteLine($"Connections: {world.Connections.Count}");

Location? northGate = queries.FindLocationById(734);
Console.WriteLine(northGate == null ? "734 not found" : $"Found 734: {northGate.Name}");
Console.WriteLine($"Outgoing from 734: {queries.FindOutgoingConnections(734).Count}");
