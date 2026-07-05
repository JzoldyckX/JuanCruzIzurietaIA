using System;
using System.Collections.Generic;
using UnityEngine;

public class ThetaStar : MonoBehaviour
{
    public static List<Node> Run(
        Node initialNode,
        Func<Node, bool> isSatisfied,
        Func<Node, List<Node>> getConnections,
        Func<Node, Node, float> getCosts,
        Func<Node, float> heuristic,
        LayerMask obstacleMask,
        int watchDog = 1000)
    {
        PriorityQueue<Node> pending = new PriorityQueue<Node>();
        Dictionary<Node, Node> parents = new Dictionary<Node, Node>();
        Dictionary<Node, float> costs = new Dictionary<Node, float>();
        HashSet<Node> closed = new HashSet<Node>();

        costs[initialNode] = 0;
        parents[initialNode] = initialNode;

        pending.Enqueue(initialNode, heuristic(initialNode));

        while (!pending.IsEmpty)
        {
            Node current = pending.Dequeue();

            if (closed.Contains(current))
                continue;

            closed.Add(current);

            if (isSatisfied(current))
            {
                List<Node> path = new List<Node>();

                Node node = current;

                while (node != parents[node])
                {
                    path.Add(node);
                    node = parents[node];
                }

                path.Add(initialNode);
                path.Reverse();

                return path;
            }

            foreach (Node neighbour in getConnections(current))
            {
                if (neighbour == null)
                    continue;

                Node parent = parents[current];

                float newCost;

                // .
                if (parent != current &&
                    parent.HasLineOfSight(neighbour, obstacleMask))
                {
                    newCost = costs[parent] + getCosts(parent, neighbour);

                    if (!costs.ContainsKey(neighbour) || newCost < costs[neighbour])
                    {
                        costs[neighbour] = newCost;
                        parents[neighbour] = parent;

                        pending.Enqueue(
                            neighbour,
                            newCost + heuristic(neighbour));
                    }
                }
                else
                {
                    newCost = costs[current] + getCosts(current, neighbour);

                    if (!costs.ContainsKey(neighbour) || newCost < costs[neighbour])
                    {
                        costs[neighbour] = newCost;
                        parents[neighbour] = current;

                        pending.Enqueue(
                            neighbour,
                            newCost + heuristic(neighbour));
                    }
                }
            }
        }

        return new List<Node>();
    }
}