using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PathFollower))]
public class EnemyAI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private LineOfSight lineOfSight;

    private PathFollower follower;

    private bool chasing = false;

    [SerializeField] private float repathTime = 0.5f;
    private float timer;

    private Node currentTargetNode;

    private void Awake()
    {
        follower = GetComponent<PathFollower>();

        if (lineOfSight == null)
            lineOfSight = GetComponent<LineOfSight>();
    }

    private void Update()
    {
        bool canSeePlayer =
            lineOfSight.isInRange(transform, player) &&
            lineOfSight.isInAngle(transform, player) &&
            lineOfSight.hasLineOfSight(transform, player);

        // Estado simple de persecución visual
        if (canSeePlayer)
        {
            chasing = true;

        }
        else
        {
            chasing = false;
        }


        if (chasing)
        {
            ChasePlayerDirect();
            return;
        }

        timer += Time.deltaTime;

        if (timer < repathTime)
            return;

        timer = 0;

        Patrol();
    }

    void Patrol()
    {
        follower.StopDirectMovement(); 

        if (!follower.HasFinishedPath())
            return;

        Node randomNode = Navigator.Instance.GetRandomNode();

        List<Node> path =
            Navigator.Instance.FindPath(
                transform.position,
                randomNode.transform.position);

        follower.SetPath(path);
    }

    void ChasePlayerDirect()
    {
        follower.MoveTowards(player.position);
    }
}