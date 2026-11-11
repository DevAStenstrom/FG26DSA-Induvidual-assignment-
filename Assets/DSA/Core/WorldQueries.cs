using System;
using System.Collections.Generic;
using System.Linq;

namespace FrontierDispatch.Core
{
    // Intentionally straightforward baseline. Correct for small worlds, but some
    // repeated queries scan all data. The assignment asks students to reason about
    // these operations before improving the representation.
    
    
    public sealed class WorldQueries
    {
        private readonly WorldData _world;
        private Dictionary<int,Location>  _locationsIdsDictionary = new Dictionary<int,Location>();
        private readonly Dictionary<int, List<Connection>> _connectionsDictionary = new Dictionary<int, List<Connection>>();
        
        public WorldQueries(WorldData world)
        {
            _world = world ?? throw new ArgumentNullException(nameof(world));
           
            foreach (var location in world.Locations)
            {
                _locationsIdsDictionary.Add(location.Id,location);
            }
            
            foreach (var connection in world.Connections)
            {
                //if not find value add empty list
                if (!_connectionsDictionary.TryGetValue(connection.FromLocationId, out var connections))
                {
                    connections = new List<Connection>();
                    _connectionsDictionary.Add(connection.FromLocationId, connections);
                }
                
                connections.Add(connection);
            }
        }

        public Location? FindLocationById(int id)
        {
            if (_locationsIdsDictionary.ContainsKey(id))
            {
                return _locationsIdsDictionary[id];
            }
            return null;
        }

        public IReadOnlyList<Connection> FindOutgoingConnections(int fromLocationId)
        {
            //if we get connection list from id
            if (_connectionsDictionary.TryGetValue(fromLocationId, out var connections))
            {
                return _connectionsDictionary[fromLocationId];
            }
            return null;
        }
    }
}
