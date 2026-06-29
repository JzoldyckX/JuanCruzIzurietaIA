using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movimiento")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;

    [Header("Salto")]
    public float gravity = -9.81f;
    public float jumpHeight = 1.5f;

    [Header("Ayudas al salto")]
    public float coyoteTime = 0.15f;
    public float jumpBufferTime = 0.15f;

    public Transform cameraTransform;

    private CharacterController controller;
    private Animator animator;

    private Vector3 velocity;

    private float coyoteCounter;
    private float jumpBufferCounter;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Guardar el input del salto
        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        // Detectar suelo
        if (controller.isGrounded)
        {
            coyoteCounter = coyoteTime;

            if (velocity.y < 0)
                velocity.y = -2f;
        }
        else
        {
            coyoteCounter -= Time.deltaTime;
        }

        // Movimiento
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude > 0.1f)
        {
            float targetAngle =
                Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg +
                cameraTransform.eulerAngles.y;

            float angle = Mathf.LerpAngle(
                transform.eulerAngles.y,
                targetAngle,
                rotationSpeed * Time.deltaTime);

            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            controller.Move(moveDir.normalized * moveSpeed * Time.deltaTime);

            animator.SetFloat("Speed", 1f);
        }
        else
        {
            animator.SetFloat("Speed", 0f);
        }

        // Salto
        if (jumpBufferCounter > 0f && coyoteCounter > 0f)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

            jumpBufferCounter = 0f;
            coyoteCounter = 0f;

            animator.SetTrigger("Jump");
        }

        // Gravedad
        velocity.y += gravity * Time.deltaTime;

        // Movimiento vertical
        controller.Move(velocity * Time.deltaTime);
    }
}