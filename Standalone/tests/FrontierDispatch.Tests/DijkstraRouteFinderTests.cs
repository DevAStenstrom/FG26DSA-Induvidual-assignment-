using FrontierDispatch.Core;
using NUnit.Framework;

namespace FrontierDispatch.Tests
{
    public sealed class DijkstraRouteFinderTests
    {
        [Test]
        public void FindRoute_ReturnsLowestCostPath()
        {
            var graph = new WeightedGraph(
                new[]
                {
                    new WeightedNode("A", 0, 0),
                    new WeightedNode("B", 1, 0),
                    new WeightedNode("C", 0, 1),
                    new WeightedNode("D", 1, 1)
                },
                new[]
                {
                    new WeightedEdge("A", "B", 8),
                    new WeightedEdge("A", "C", 2),
                    new WeightedEdge("C", "B", 1),
                    new WeightedEdge("B", "D", 2),
                    new WeightedEdge("C", "D", 10)
                });

            RouteResult result = new DijkstraRouteFinder().FindRoute(graph, "A", "D");

            Assert.That(result.Found, Is.True);
            Assert.That(result.Path, Is.EqualTo(new[] { "A", "C", "B", "D" }));
            Assert.That(result.TotalCost, Is.EqualTo(5));
        }
    }
}
