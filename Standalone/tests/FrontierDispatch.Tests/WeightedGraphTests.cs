using System;
using FrontierDispatch.Core;
using NUnit.Framework;

namespace FrontierDispatch.Tests
{
    public sealed class WeightedGraphTests
    {
        [Test]
        public void Constructor_EdgeToUnknownNode_Throws()
        {
            var nodes = new[] { new WeightedNode("A", 0, 0) };
            var edges = new[] { new WeightedEdge("A", "B", 1) };

            Assert.That(
                () => { _ = new WeightedGraph(nodes, edges); },
                Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void WeightedEdge_NegativeCost_Throws()
        {
            Assert.That(
                () => { _ = new WeightedEdge("A", "B", -1); },
                Throws.TypeOf<ArgumentOutOfRangeException>());
        }
    }
}
