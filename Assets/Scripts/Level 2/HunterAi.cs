using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PathFollower))]
public class HunterAI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private LineOfSight lineOfSight;
    [SerializeField] private bool Theta;
    private PathFollower follower;

    private bool investigating = false;

    private Vector3 alertPosition;

    [SerializeField]
    private float repathTime = 0.5f;

    private float timer;

    [SerializeField] private bool useDecisionTree;

    private HunterDecisionTree tree;

    private enum HunterState
    {
        Patrol,
        Chase,
        Recalculate
    }

    private HunterState currentState;

    private void Awake()
    {
        tree = GetComponent<HunterDecisionTree>();  
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

        if (useDecisionTree)
        {
            tree.UpdateTree(canSeePlayer);

            currentState = tree.currentState == HunterDecisionTree.HunterState.Chase
                ? HunterState.Chase
                : (tree.currentState == HunterDecisionTree.HunterState.Recalculate ? HunterState.Recalculate : HunterState.Patrol);
        }
        else
        {
            if (canSeePlayer)
                currentState = HunterState.Chase;
            else
                currentState = HunterState.Patrol;
        }


        if (currentState == HunterState.Recalculate)
        {
            follower.StopDirectMovement();
            return;
        }


        if (currentState == HunterState.Chase)
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
                    alertPosition,
                    Theta
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
                randomNode.transform.position,
                Theta
            )
        );
    }
}