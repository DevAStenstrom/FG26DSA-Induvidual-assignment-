using System;
using System.Collections.Generic;

namespace FrontierDispatch.Core
{
    public sealed class DijkstraRouteFinder
    {
        public RouteResult FindRoute(WeightedGraph graph, string start, string goal)
        {
            if (graph == null)
            {
                throw new ArgumentNullException(nameof(graph));
            }

            if (!graph.ContainsNode(start) || !graph.ContainsNode(goal))
            {
                return RouteResult.NotFound(0);
            }

            if (start == goal)
            {
                return RouteResult.Success(new[] { start }, 0, 0);
            }

            var bestCost = new Dictionary<string, int>();
            var cameFrom = new Dictionary<string, string>();
            var frontier = new MinPriorityQueue<string>();

            bestCost[start] = 0;
            frontier.Enqueue(start, 0);

            int expandedNodes = 0;

            while (frontier.Count > 0)
            {
                var entry = frontier.Dequeue();
                string current = entry.Value;

                //if best cost don't have value or the priority is != the currentBest then continue
                if (!bestCost.TryGetValue(current, out int currentBest) || entry.Priority != currentBest)
                {
                    continue;
                }

                expandedNodes++;

                //if the current node is the target make the return list
                if (current == goal)
                {
                    List<string> path = new List<string>();
                    string node = goal;

                    //create list with came from
                    while (node != start)
                    {
                        path.Add(node);
                        node = cameFrom[node];
                    }

                    //reverse it and return
                    path.Add(start);
                    path.Reverse();

                    return RouteResult.Success(path, currentBest, expandedNodes);
                }

                //gets edges & stores & enqueues
                foreach (WeightedEdge WE in graph.GetOutgoing(current))
                {
                    //candidate = current best path cost + new edge cost
                    int candidate = currentBest + WE.Cost;
                    //if bestcost does not have the value of a known path or candidates cost < knowns cost then we update the bestcost,camefrom and enqueues the new node

                    if (!bestCost.TryGetValue(WE.To, out int known) || candidate < known)
                    {
                        bestCost[WE.To] = candidate;
                        cameFrom[WE.To] = current;
                        frontier.Enqueue(WE.To, candidate);
                    }
                }
            }

            return RouteResult.NotFound(expandedNodes);
        }
        
        public IReadOnlyDictionary<string, int> FindReachable(WeightedGraph graph, string start, int budget)
        {
            if (graph == null)
            {
                throw new ArgumentNullException(nameof(graph));
            }

            // budget < 0 or no start node return empty best list
            var bestCost = new Dictionary<string, int>();
            if (budget < 0 || !graph.ContainsNode(start))
            {
                return bestCost;
            }

            var frontier = new MinPriorityQueue<string>();
            bestCost[start] = 0;
            frontier.Enqueue(start, 0);

            while (frontier.Count > 0)
            {
                var entry = frontier.Dequeue();
                string current = entry.Value;

                //if prio is not currently bestcost continue
                if (entry.Priority != bestCost[current])
                {
                    continue;
                }

                foreach (WeightedEdge edge in graph.GetOutgoing(current))
                {
                    int candidate = entry.Priority + edge.Cost;

                    // if new node cost more than budget continue
                    if (candidate > budget)
                    {
                        continue;
                    }

                    //else add
                    if (!bestCost.TryGetValue(edge.To, out int known) || candidate < known)
                    {
                        bestCost[edge.To] = candidate;
                        frontier.Enqueue(edge.To, candidate);
                    }
                }
            }

            return bestCost;
        }
        
    }
}