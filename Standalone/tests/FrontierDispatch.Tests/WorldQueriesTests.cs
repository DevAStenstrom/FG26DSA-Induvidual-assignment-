using NUnit.Framework;
using FrontierDispatch.Core;

namespace FrontierDispatch.Tests
{
    public sealed class WorldQueriesTests
    {
        [Test]
        public void FindLocationById_ReturnsMatchingLocation()
        {
            var queries = new WorldQueries(SampleWorldFactory.CreateSmall());
            Location? location = queries.FindLocationById(734);
            Assert.That(location, Is.Not.Null);
            Assert.That(location!.Name, Is.EqualTo("North Gate"));
        }

        [Test]
        public void FindLocationById_ReturnsNullWhenMissing()
        {
            var queries = new WorldQueries(SampleWorldFactory.CreateSmall());
            Assert.That(queries.FindLocationById(9999), Is.Null);
        }

        [Test]
        public void FindOutgoingConnections_ReturnsOnlyMatchingOrigin()
        {
            var queries = new WorldQueries(SampleWorldFactory.CreateSmall());
            var outgoing = queries.FindOutgoingConnections(734);
            Assert.That(outgoing.Count, Is.EqualTo(3));
            for (int i = 0; i < outgoing.Count; ++i)
            {
                Assert.That(outgoing[i].FromLocationId, Is.EqualTo(734));
            }
        }
        
        [Test]
        public void FindNextPriority()
        {
            var sampleFactory = SampleWorldFactory.CreateSmall();
            var queries = new WorldQueries(sampleFactory);
            var priorityList = new MinPriorityQueue();
            foreach (var item in sampleFactory.Locations)
            {
                priorityList.Enqueue(item); 
            }
            
            Assert.That(priorityList.PeekNext().Priority, Is.EqualTo(3));
            priorityList.DequeueNext();
            Assert.That(priorityList.PeekNext().Priority, Is.EqualTo(5));
            priorityList.DequeueNext();
            Assert.That(priorityList.PeekNext().Priority, Is.EqualTo(5));
            priorityList.DequeueNext();
            Assert.That(priorityList.PeekNext().Priority, Is.EqualTo(8));
            priorityList.DequeueNext();
            Assert.That(priorityList.PeekNext().Priority, Is.EqualTo(21));
            priorityList.DequeueNext();
            Assert.That(priorityList.PeekNext().Priority, Is.EqualTo(39));
            
        }
        
    }
}
