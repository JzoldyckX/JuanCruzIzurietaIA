using System.Collections.Generic;
using UnityEngine;

public class Navigator : MonoBehaviour
{
    public static Navigator Instance;

    private Node[] nodes;

    private void Awake()
    {
        Instance = this;
        nodes = FindObjectsOfType<Node>();
    }

    public Node GetClosestNode(Vector3 position)
    {
        Node closest = null;
        float minDistance = Mathf.Infinity;

        foreach (Node node in nodes)
        {
            float distance = Vector3.Distance(position, node.transform.position);

            if (distance < minDistance)
            {
                minDistance = distance;
                closest = node;
            }
        }

        return closest;
    }
    public Node GetRandomNode()
    {
        return nodes[Random.Range(0, nodes.Length)];
    }
    public Node GetFarthestNode(Vector3 playerPosition, Vector3 enemyPosition)
    {
        Node bestNode = null;
        float bestScore = float.MinValue;


        Vector3 escapeDirection = (enemyPosition - playerPosition).normalized;

        foreach (Node node in nodes)
        {
            Vector3 toNode = (node.transform.position - enemyPosition).normalized;


            float directionScore = Vector3.Dot(escapeDirection, toNode);

            float distanceScore = Vector3.Distance(node.transform.position, playerPosition);

            float score = directionScore * 100f + distanceScore;

            if (score > bestScore)
            {
                bestScore = score;
                bestNode = node;
            }
        }

        return bestNode;
    }
    public List<Node> FindPath(Vector3 start, Vector3 end)
    {
        Node startNode = GetClosestNode(start);
        Node endNode = GetClosestNode(end);

        if (startNode == null || endNode == null)
            return new List<Node>();

        return AStar.Run(
            startNode,
            node => node == endNode,
            node => node.neighbours,
            (a, b) => Vector3.Distance(a.transform.position, b.transform.position),
            node => Vector3.Distance(node.transform.position, endNode.transform.position)
        );
    }

}