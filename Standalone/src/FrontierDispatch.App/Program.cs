using System;
using System.Diagnostics;
using FrontierDispatch.Core;

WorldData world = SampleWorldFactory.CreateSmall();
var queries = new WorldQueries(world);

int amountOfTimes = 1000000;

var stopWatch = Stopwatch.StartNew();
for (int i = 0; i < amountOfTimes; i++)
{
    queries.FindLocationById(734);
}
stopWatch.Stop();
Console.WriteLine($"Test for FindLocationById took {stopWatch.Elapsed.TotalMilliseconds} ms");

stopWatch = Stopwatch.StartNew();
for (int i = 0; i < amountOfTimes; i++)
{
    queries.FindOutgoingConnections(734);
}
stopWatch.Stop();
Console.WriteLine($"Test for FindOutgoingConnections took {stopWatch.Elapsed.TotalMilliseconds} ms");

Console.WriteLine("Frontier Dispatch - headless baseline");
Console.WriteLine($"Locations: {world.Locations.Count}");
Console.WriteLine($"Connections: {world.Connections.Count}");

Location? northGate = queries.FindLocationById(734);
Console.WriteLine(northGate == null ? "734 not found" : $"Found 734: {northGate.Name}");
Console.WriteLine($"Outgoing from 734: {queries.FindOutgoingConnections(734).Count}");
