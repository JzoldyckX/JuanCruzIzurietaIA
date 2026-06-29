using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class EnemyController : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private Transform player;

    [SerializeField]
    private LineOfSight los;

    [SerializeField]
    private FSM fsm;
    private Animator animator;
    private Rigidbody rb;

    [Header("Movement")]
    [SerializeField]
    private float speed = 3f;

    [SerializeField]
    private float rotationSpeed = 5f;

    [Header("Behavior")]
    public bool shouldFlee = false;

    [Header("Patrol")]
    [SerializeField]
    private float patrolRadius = 5f;

    [SerializeField]
    private float patrolWaitTime = 2f;

    private Vector3 patrolTarget;
    private float patrolTimer;

    [Header("TMP UI")]
    [SerializeField]
    private TextMeshProUGUI resultText;

    private bool gameEnded = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        if (los == null)
            los = GetComponent<LineOfSight>();

        if (fsm == null)
            fsm = GetComponent<FSM>();
    }

    void Start()
    {
        SetNewPatrolPoint();
        animator = GetComponent<Animator>();
        if (resultText != null)
            resultText.gameObject.SetActive(false);

        rb.freezeRotation = true;
    }

    void Update()
    {
        if (gameEnded)
            return;

        bool canSeePlayer =
            los.isInRange(transform, player)
            && los.isInAngle(transform, player)
            && los.hasLineOfSight(transform, player);

        fsm.UpdateState(canSeePlayer, shouldFlee);

        ExecuteState();
    }

    void ExecuteState()
    {
        switch (fsm.currentState)
        {
            case FSM.EnemyState.Patrol:
                Patrol();
                break;

            case FSM.EnemyState.Pursuit:
                PursuePlayer();
                break;

            case FSM.EnemyState.Flee:
                FleeFromPlayer();
                break;
        }
    }

    void Patrol()
    {
        Vector3 dir = patrolTarget - transform.position;
        dir.y = 0;

        if (dir.magnitude < 0.5f)
        {
            patrolTimer += Time.deltaTime;

            if (animator != null)
                animator.SetFloat("Speed", 0f); // Quieto en patrulla

            if (patrolTimer >= patrolWaitTime)
            {
                SetNewPatrolPoint();
                patrolTimer = 0f;
            }
            return;
        }

        Move(dir);
    }

    void SetNewPatrolPoint()
    {
        Vector2 randomCircle = Random.insideUnitCircle * patrolRadius;
        patrolTarget = new Vector3(
            transform.position.x + randomCircle.x,
            transform.position.y,
            transform.position.z + randomCircle.y
        );
    }

    void PursuePlayer()
    {
        Vector3 dir = player.position - transform.position;
        dir.y = 0;

        Move(dir);
    }

    void FleeFromPlayer()
    {
        Vector3 dir = transform.position - player.position;
        dir.y = 0;

        Move(dir);
    }

    void Move(Vector3 dir)
    {
        Vector3 moveDir = dir.normalized;

        rb.MovePosition(rb.position + moveDir * speed * Time.deltaTime);

        transform.forward = Vector3.Lerp(
            transform.forward,
            moveDir,
            Time.deltaTime * rotationSpeed
        );

        if (animator != null)
        {
            if (dir.magnitude > 0.1f)
                animator.SetFloat("Speed", 1f);
            else
                animator.SetFloat("Speed", 0f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (gameEnded)
            return;

        if (other.CompareTag("Player"))
        {
            gameEnded = true;

            if (resultText != null)
                resultText.gameObject.SetActive(true);

            if (fsm.currentState == FSM.EnemyState.Pursuit)
            {
                resultText.text = "Perdiste";
                StartCoroutine(RestartGame(2f));
            }
            else
            {
                resultText.text = "Ganaste";
                SceneManager.LoadScene("Nivel2");
            }
        }
    }


    IEnumerator RestartGame(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
