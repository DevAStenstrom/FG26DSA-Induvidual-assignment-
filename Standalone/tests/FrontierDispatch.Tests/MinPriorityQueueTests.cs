using System;
using FrontierDispatch.Core;
using NUnit.Framework;

namespace FrontierDispatch.Tests
{
    public sealed class MinPriorityQueueTests
    {
        [Test]
        public void NewQueue_IsEmpty()
        {
            var queue = new MinPriorityQueue<string>();

            Assert.That(queue.Count, Is.EqualTo(0));
        }

        [Test]
        public void Enqueue_IncreasesCount()
        {
            var queue = new MinPriorityQueue<string>();

            queue.Enqueue("a", 3);
            queue.Enqueue("b", 1);

            Assert.That(queue.Count, Is.EqualTo(2));
        }

        [Test]
        public void Dequeue_ReturnsItemsInPriorityOrder()
        {
            var queue = new MinPriorityQueue<string>();
            queue.Enqueue("c", 5);
            queue.Enqueue("a", 1);
            queue.Enqueue("d", 8);
            queue.Enqueue("b", 3);

            Assert.That(queue.Dequeue().Value, Is.EqualTo("a"));
            Assert.That(queue.Dequeue().Value, Is.EqualTo("b"));
            Assert.That(queue.Dequeue().Value, Is.EqualTo("c"));
            Assert.That(queue.Dequeue().Value, Is.EqualTo("d"));
            Assert.That(queue.Count, Is.EqualTo(0));
        }

        [Test]
        public void Peek_ReturnsMinimumWithoutRemovingIt()
        {
            var queue = new MinPriorityQueue<string>();
            queue.Enqueue("x", 2);
            queue.Enqueue("y", 1);

            Assert.That(queue.Peek().Value, Is.EqualTo("y"));
            Assert.That(queue.Count, Is.EqualTo(2));
        }

        [Test]
        public void Dequeue_OnEmptyQueue_Throws()
        {
            var queue = new MinPriorityQueue<int>();

            Assert.That(() => { queue.Dequeue(); }, Throws.TypeOf<InvalidOperationException>());
        }

        [Test]
        public void Peek_OnEmptyQueue_Throws()
        {
            var queue = new MinPriorityQueue<int>();

            Assert.That(() => { queue.Peek(); }, Throws.TypeOf<InvalidOperationException>());
        }

        [Test]
        public void Dequeue_WithDuplicatePriorities_ReturnsEveryItem()
        {
            var queue = new MinPriorityQueue<string>();
            queue.Enqueue("a", 1);
            queue.Enqueue("b", 1);
            queue.Enqueue("c", 1);

            var values = new[] { queue.Dequeue().Value, queue.Dequeue().Value, queue.Dequeue().Value };

            Assert.That(values, Is.EquivalentTo(new[] { "a", "b", "c" }));
        }

        [Test]
        public void Dequeue_ManyRandomPriorities_NeverGoesBackwards()
        {
            var queue = new MinPriorityQueue<int>();
            var random = new Random(42);
            for (int i = 0; i < 500; ++i)
            {
                queue.Enqueue(i, random.Next(0, 100));
            }

            int previous = int.MinValue;
            int dequeued = 0;
            while (queue.Count > 0)
            {
                int priority = queue.Dequeue().Priority;
                Assert.That(priority, Is.GreaterThanOrEqualTo(previous));
                previous = priority;
                ++dequeued;
            }
        }
    }
}
