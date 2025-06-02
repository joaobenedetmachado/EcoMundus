using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float runSpeed = 9f;
    public float jumpForce = 8f;
    public Transform planet;
    public LayerMask groundMask;
    public float groundCheckDistance = 1.2f;

    private Rigidbody rb;
    private Animator animator;
    private bool isGrounded;
    private int lastAnimState = -1; // Guarda o último estado de animação
    private bool hasJumped = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    void FixedUpdate()
    {
        // Gravidade personalizada
        Vector3 gravityDirection = (planet.position - transform.position).normalized;
        rb.AddForce(gravityDirection * 9.81f, ForceMode.Acceleration);

        // Alinha o jogador com a superfície
        Quaternion targetRotation = Quaternion.FromToRotation(transform.up, -gravityDirection) * transform.rotation;
        rb.MoveRotation(Quaternion.Slerp(transform.rotation, targetRotation, 10 * Time.fixedDeltaTime));

        // Checa se está no chão
        isGrounded = Physics.Raycast(transform.position, -transform.up, groundCheckDistance, groundMask);

        // Entrada de movimento
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 inputDir = new Vector3(h, 0f, v).normalized;

        // Direção tangente ao planeta
        Vector3 forward = Vector3.Cross(transform.right, -gravityDirection).normalized;
        Vector3 right = Vector3.Cross(-gravityDirection, forward).normalized;
        Vector3 moveDir = (forward * v + right * h).normalized;

        float speed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;

        if (inputDir.magnitude > 0)
        {
            rb.MovePosition(rb.position + moveDir * speed * Time.fixedDeltaTime);

            // Rotaciona o corpo do jogador na direção do movimento
            Quaternion lookRotation = Quaternion.LookRotation(moveDir, -gravityDirection);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, lookRotation, 10 * Time.fixedDeltaTime));
        }

        // Pulo (dispara trigger só no momento do pulo)
        if (Input.GetKey(KeyCode.Space) && isGrounded && !hasJumped)
        {
            rb.AddForce(-gravityDirection * jumpForce, ForceMode.Impulse);
            animator.SetTrigger("Jump");
            hasJumped = true;
        }

        // Reseta flag quando voltar ao chão
        if (isGrounded && hasJumped)
        {
            hasJumped = false;
        }

        // === ANIMAÇÃO ===
        // Só roda transições normais se não estiver pulando
        if (!hasJumped)
        {
            int animState = 0; // idle

            if (Input.GetMouseButton(0))
            {
                animState = 5; // punch
            }
            else if (Input.GetKey(KeyCode.E))
            {
                animState = 4; // pick up
            }
            else if (inputDir.magnitude > 0)
            {
                animState = Input.GetKey(KeyCode.LeftShift) ? 2 : 1; // run or walk
            }

            if (animState != lastAnimState)
            {
                animator.SetInteger("transition", animState);
                Debug.Log("AnimState: " + animState);
                lastAnimState = animState;
            }
        }
    }
}
