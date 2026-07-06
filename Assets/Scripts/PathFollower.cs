using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PathFollower : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float rotationSpeed = 8f;
    [SerializeField] private float arriveDistance = 0.2f;

    private Rigidbody rb;

    // ===== PATH MODE =====
    private List<Node> currentPath = new List<Node>();
    private int currentIndex;

    // ===== DIRECT MODE =====
    private bool directMovement = false;
    private Vector3 directTarget;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void SetPath(List<Node> path)
    {
        directMovement = false;

        currentPath = path;
        currentIndex = 0;
    }

    public void MoveTowards(Vector3 target)
    {
        directMovement = true;
        directTarget = target;
    }

    public void StopDirectMovement()
    {
        directMovement = false;
    }

    // Nuevo: detiene absolutamente cualquier movimiento y limpia el path actual
    public void StopAllMovement()
    {
        directMovement = false;
        if (currentPath != null)
            currentPath.Clear();
        currentIndex = 0;
    }

    public bool HasFinishedPath()
    {
        return currentIndex >= currentPath.Count;
    }

    private void FixedUpdate()
    {
        if (directMovement)
        {
            FollowDirectTarget();
            return;
        }

        if (HasFinishedPath())
            return;

        FollowCurrentPath();
    }

    void FollowCurrentPath()
    {
        Node targetNode = currentPath[currentIndex];

        Vector3 target = targetNode.transform.position;
        target.y = transform.position.y;

        Vector3 dir = target - transform.position;

        if (dir.magnitude <= arriveDistance)
        {
            currentIndex++;
            return;
        }

        Move(dir.normalized);
    }

    void FollowDirectTarget()
    {
        Vector3 target = directTarget;
        target.y = transform.position.y;

        Vector3 dir = target - transform.position;

        if (dir.magnitude <= arriveDistance)
            return;

        Move(dir.normalized);
    }

    void Move(Vector3 direction)
    {
        rb.MovePosition(
            rb.position +
            direction * moveSpeed * Time.fixedDeltaTime
        );

        Quaternion rot = Quaternion.LookRotation(direction);

        rb.MoveRotation(
            Quaternion.Slerp(
                rb.rotation,
                rot,
                rotationSpeed * Time.fixedDeltaTime
            )
        );
    }

    private void OnDrawGizmos()
    {
        if (currentPath == null || currentPath.Count == 0)
            return;

        Gizmos.color = Color.cyan;

        for (int i = 0; i < currentPath.Count - 1; i++)
        {
            if (currentPath[i] != null && currentPath[i + 1] != null)
            {
                Gizmos.DrawLine(
                    currentPath[i].transform.position,
                    currentPath[i + 1].transform.position
                );
            }
        }
    }
}