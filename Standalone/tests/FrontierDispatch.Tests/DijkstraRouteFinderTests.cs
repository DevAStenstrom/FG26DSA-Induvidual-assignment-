using System;
using FrontierDispatch.Core;
using NUnit.Framework;

namespace FrontierDispatch.Tests
{
    public sealed class DijkstraRouteFinderTests
    {
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
            // D exists in the graph but has no incoming edges.
            RouteResult result = new DijkstraRouteFinder().FindRoute(CreateDemoGraph(), "A", "D");

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

        [Test]
        public void FindReachable_ZeroBudget_ReturnsOnlyStart()
        {
            var reachable = new DijkstraRouteFinder().FindReachable(CreateDemoGraph(), "A", 0);

            Assert.That(reachable.Count, Is.EqualTo(1));
            Assert.That(reachable.ContainsKey("A"), Is.True);
            Assert.That(reachable["A"], Is.EqualTo(0));
        }

        [Test]
        public void FindReachable_BudgetJustBelowPathCost_ExcludesLocation()
        {
            // Cheapest way to C costs 5 (A -> B -> C).
            var reachable = new DijkstraRouteFinder().FindReachable(CreateDemoGraph(), "A", 4);

            Assert.That(reachable.ContainsKey("B"), Is.True);
            Assert.That(reachable.ContainsKey("C"), Is.False);
        }

        [Test]
        public void FindReachable_BudgetExactlyAtPathCost_IncludesLocation()
        {
            var reachable = new DijkstraRouteFinder().FindReachable(CreateDemoGraph(), "A", 5);

            Assert.That(reachable.ContainsKey("C"), Is.True);
        }

        [Test]
        public void FindReachable_UsesCheapestOfSeveralRoutes()
        {
            // C is reachable directly for 10 and through B for 5.
            var reachable = new DijkstraRouteFinder().FindReachable(CreateDemoGraph(), "A", 100);

            Assert.That(reachable["B"], Is.EqualTo(2));
            Assert.That(reachable["C"], Is.EqualTo(5));
        }

        [Test]
        public void FindReachable_DisconnectedLocation_IsNeverIncluded()
        {
            var reachable = new DijkstraRouteFinder().FindReachable(CreateDemoGraph(), "A", 100);

            Assert.That(reachable.ContainsKey("D"), Is.False);
            Assert.That(reachable.Count, Is.EqualTo(3));
        }

        [Test]
        public void FindReachable_UnknownStartOrNegativeBudget_ReturnsEmpty()
        {
            var finder = new DijkstraRouteFinder();
            WeightedGraph graph = CreateDemoGraph();

            Assert.That(finder.FindReachable(graph, "Z", 10), Is.Empty);
            Assert.That(finder.FindReachable(graph, "A", -1), Is.Empty);
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
