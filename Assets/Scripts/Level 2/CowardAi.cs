using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PathFollower))]
public class CowardAI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private LineOfSight lineOfSight;

    private PathFollower follower;
    private bool escaping = false;


    [SerializeField]
    private float repathTime = 0.5f;


    private void Awake()
    {
        follower = GetComponent<PathFollower>();

        if (lineOfSight == null)
            lineOfSight = GetComponent<LineOfSight>();
    }

    private void Update()
    {
        if (escaping)
        {
            

            if (follower.HasFinishedPath())
            {
                escaping = false;
            }
            else
            {
                return;
            }
        }

        bool canSeePlayer =
            lineOfSight.isInRange(transform, player) &&
            lineOfSight.isInAngle(transform, player) &&
            lineOfSight.hasLineOfSight(transform, player);

        if (canSeePlayer)
        {
            escaping = true;

            EnemyManager.Instance.AlertHunter(player.position);

            EscapeNow();

            return;
        }

        Patrol();
    }

    void Patrol()
    {
        follower.StopDirectMovement();

        if (!follower.HasFinishedPath())
            return;

        Node randomNode = Navigator.Instance.GetRandomNode();

        follower.SetPath(
            Navigator.Instance.FindPath(
                transform.position,
                randomNode.transform.position
            )
        );
    }

    void EscapeNow()
    {
        Node safeNode = Navigator.Instance.GetFarthestNode(
            player.position,
            transform.position
        );

        follower.SetPath(
            Navigator.Instance.FindPath(
                transform.position,
                safeNode.transform.position
            )
        );
    }
}
