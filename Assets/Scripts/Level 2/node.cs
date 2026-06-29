using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    [Header("Connections")]
    public List<Node> neighbours = new List<Node>();

    [Header("Settings")]
    [SerializeField] private float connectionDistance = 2.2f;
    [SerializeField] private LayerMask obstacleMask;

    [ContextMenu("Generate Connections")]
    public void GenerateConnections()
    {
        neighbours.Clear();

        Node[] allNodes = FindObjectsOfType<Node>();

        foreach (Node node in allNodes)
        {
            if (node == this)
                continue;

            float distance = Vector3.Distance(transform.position, node.transform.position);

            if (distance > connectionDistance)
                continue;

 
            Vector3 start = transform.position + Vector3.up * 0.2f;
            Vector3 end = node.transform.position + Vector3.up * 0.2f;
            Vector3 dir = end - start;

            if (!Physics.Raycast(start, dir.normalized, dir.magnitude, obstacleMask))
            {
                neighbours.Add(node);
            }
        }
    }

    private void OnDrawGizmos()
    {

        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, 0.15f);


        Gizmos.color = Color.green;

        foreach (Node node in neighbours)
        {
            if (node != null)
            {
                Gizmos.DrawLine(transform.position, node.transform.position);
            }
        }
    }
}