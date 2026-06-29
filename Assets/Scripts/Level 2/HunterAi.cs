using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PathFollower))]
public class HunterAI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private LineOfSight lineOfSight;

    private PathFollower follower;

    private bool chasing = false;
    private bool investigating = false;

    private Vector3 alertPosition;

    [SerializeField]
    private float repathTime = 0.5f;

    private float timer;

    private void Awake()
    {
        follower = GetComponent<PathFollower>();

        if (lineOfSight == null)
            lineOfSight = GetComponent<LineOfSight>();
    }

    public void ReceiveAlert(Vector3 playerPosition)
    {
        investigating = true;
        alertPosition = playerPosition;
    }

    private void Update()
    {
        bool canSeePlayer =
            lineOfSight.isInRange(transform, player) &&
            lineOfSight.isInAngle(transform, player) &&
            lineOfSight.hasLineOfSight(transform, player);

        if (canSeePlayer)
        {
            chasing = true;
            investigating = false;
        }
        else if (chasing)
        {
            chasing = false;
            follower.StopDirectMovement();
        }

        if (chasing)
        {
            follower.MoveTowards(player.position);
            return;
        }

        timer += Time.deltaTime;

        if (timer < repathTime)
            return;

        timer = 0;

        if (investigating)
        {
            if (!follower.HasFinishedPath())
                return;

            follower.SetPath(
                Navigator.Instance.FindPath(
                    transform.position,
                    alertPosition
                )
            );

            investigating = false;
        }
        else
        {
            Patrol();
        }
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
}