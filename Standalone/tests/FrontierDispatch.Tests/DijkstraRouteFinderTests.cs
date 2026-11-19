using System;
using FrontierDispatch.Core;
using NUnit.Framework;

namespace FrontierDispatch.Tests
{
    public sealed class DijkstraRouteFinderTests
    {
        [Test]
        public void FindRoute_ReturnsLowestCostPath()
        {
            var graph = CreateDemoGraph();

            RouteResult result = new DijkstraRouteFinder().FindRoute(graph, "A", "C");

            Assert.That(result.Found, Is.True);
            Assert.That(result.Path, Is.EqualTo(new[] { "A", "B", "C" }));
            Assert.That(result.TotalCost, Is.EqualTo(5));
        }

        [Test]
        public void FindRoute_StartEqualsGoal_ReturnsSingleNodePathWithZeroCost()
        {
            RouteResult result = new DijkstraRouteFinder().FindRoute(CreateDemoGraph(), "A", "A");

            Assert.That(result.Found, Is.True);
            Assert.That(result.Path, Is.EqualTo(new[] { "A" }));
            Assert.That(result.TotalCost, Is.EqualTo(0));
        }

        [Test]
        public void FindRoute_PrefersCheaperPathOverFewerHops()
        {
            RouteResult result = new DijkstraRouteFinder().FindRoute(CreateDemoGraph(), "A", "C");

            Assert.That(result.Found, Is.True);
            Assert.That(result.Path, Is.EqualTo(new[] { "A", "B", "C" }));
            Assert.That(result.TotalCost, Is.EqualTo(5));
        }

        [Test]
        public void FindRoute_UnreachableGoal_ReturnsNotFound()
        {
            RouteResult result = new DijkstraRouteFinder().FindRoute(CreateDemoGraph(), "A", "Z");

            Assert.That(result.Found, Is.False);
            Assert.That(result.Path, Is.Empty);
        }
        
        [Test]
        public void FindRoute_UnknownStartOrGoal_ReturnsNotFound()
        {
            var finder = new DijkstraRouteFinder();
            WeightedGraph graph = CreateDemoGraph();

            Assert.That(finder.FindRoute(graph, "A", "Z").Found, Is.False);
            Assert.That(finder.FindRoute(graph, "Z", "A").Found, Is.False);
        }



        private static WeightedGraph CreateDemoGraph()
        {
            return new WeightedGraph(
                new[]
                {
                    new WeightedNode("A", 0, 0),
                    new WeightedNode("B", 1, 0),
                    new WeightedNode("C", 2, 0),
                    new WeightedNode("D", 3, 0)
                },
                new[]
                {
                    new WeightedEdge("A", "C", 10),
                    new WeightedEdge("A", "B", 2),
                    new WeightedEdge("B", "C", 3)
                });
        }
    }
}
