using System;
using System.Collections.Generic;

namespace FrontierDispatch.Core
{
    // Intentionally straightforward baseline. Correct for small worlds, but some
    // repeated queries scan all data. The assignment asks students to reason about
    // these operations before improving the representation.
    public sealed class WorldQueries
    {
        private readonly WorldData _world;

        public WorldQueries(WorldData world)
        {
            _world = world ?? throw new ArgumentNullException(nameof(world));
        }

        public Location? FindLocationById(int id)
        {
            for (int i = 0; i < _world.Locations.Count; ++i)
            {
                Location location = _world.Locations[i];
                if (location.Id == id)
                {
                    return location;
                }
            }

            return null;
        }

        public IReadOnlyList<Connection> FindOutgoingConnections(int fromLocationId)
        {
            var result = new List<Connection>();
            for (int i = 0; i < _world.Connections.Count; ++i)
            {
                Connection connection = _world.Connections[i];
                if (connection.FromLocationId == fromLocationId)
                {
                    result.Add(connection);
                }
            }

            return result;
        }
    }
}
