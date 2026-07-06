using UnityEngine;

[RequireComponent(typeof(PathFollower))]
[RequireComponent(typeof(CowardFSM))]
public class CowardAI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private LineOfSight lineOfSight;

    private PathFollower follower;
    private CowardFSM fsm;

    [Header("Pathfinding")]
    [SerializeField] private float repathTime = 0.5f;
    [SerializeField] private bool useThetaStar = false;

    private float timer;


    private bool escaping = false;

    private void Awake()
    {
        follower = GetComponent<PathFollower>();
        fsm = GetComponent<CowardFSM>();

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

            return;
        }

        bool canSeePlayer =
            lineOfSight.isInRange(transform, player) &&
            lineOfSight.isInAngle(transform, player) &&
            lineOfSight.hasLineOfSight(transform, player);


        fsm.UpdateState(canSeePlayer);

        timer += Time.deltaTime;

        if (timer < repathTime)
            return;

        timer = 0;

        if (fsm.IsEscaping())
        {
            EscapeNow();
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
                useThetaStar
            )
        );
    }

    void EscapeNow()
    {
        escaping = true;

        EnemyManager.Instance.AlertHunter(player.position);

        Node safeNode = Navigator.Instance.GetFarthestNode(
            player.position,
            transform.position
        );

        follower.SetPath(
            Navigator.Instance.FindPath(
                transform.position,
                safeNode.transform.position,
                useThetaStar
            )
        );
    }
}