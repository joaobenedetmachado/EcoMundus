using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float runSpeed = 9f;
    // public float jumpForce = 8f; // removido

    public Transform planet;

    private Rigidbody rb;
    private Animator animator;
    private bool isGrounded;
    // private bool hasJumped = false; // removido
    private int lastAnimState = -1;

    private Vector3 gravityDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    void Update()
    {
        // Código de pulo removido
        // if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !hasJumped)
        // {
        //     Jump();
        // }
    }

    void FixedUpdate()
    {
        gravityDirection = (planet.position - transform.position).normalized;

        // Aplica a gravidade customizada
        rb.AddForce(gravityDirection * 9.81f, ForceMode.Acceleration);

        // Rotaciona o personagem para ficar "em pé" na superfície do planeta
        Quaternion targetRotation = Quaternion.FromToRotation(transform.up, -gravityDirection) * transform.rotation;
        rb.MoveRotation(Quaternion.Slerp(transform.rotation, targetRotation, 10 * Time.fixedDeltaTime));

        // Movimento horizontal com velocidade
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 inputDir = new Vector3(h, 0f, v).normalized;

        if (inputDir.magnitude > 0)
        {
            Vector3 forward = Vector3.Cross(transform.right, -gravityDirection).normalized;
            Vector3 right = Vector3.Cross(-gravityDirection, forward).normalized;
            Vector3 moveDir = (forward * v + right * h).normalized;

            float speed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;

            // Mantém a componente vertical da velocidade (pulo/gravity)
            Vector3 horizontalVelocity = moveDir * speed;

            Vector3 newVelocity = horizontalVelocity + Vector3.Project(rb.linearVelocity, gravityDirection);
            rb.linearVelocity = newVelocity;

            // Rotaciona na direção do movimento
            Quaternion lookRotation = Quaternion.LookRotation(moveDir, -gravityDirection);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, lookRotation, 10 * Time.fixedDeltaTime));
        }
        else
        {
            // Se não estiver andando, mantém só a velocidade vertical (gravidade/pulo)
            Vector3 verticalVelocity = Vector3.Project(rb.linearVelocity, gravityDirection);
            rb.linearVelocity = verticalVelocity;
        }

        // Reseta flag de pulo quando toca no chão (removido)
        // if (isGrounded && hasJumped)
        // {
        //     hasJumped = false;
        // }

        // Atualiza animação
        UpdateAnimation(inputDir);
    }

    // Método Jump removido
    // private void Jump()
    // {
    //     Vector3 verticalVelocity = Vector3.Project(rb.linearVelocity, gravityDirection);
    //     rb.linearVelocity -= verticalVelocity;
    //     rb.AddForce(-gravityDirection * jumpForce, ForceMode.Impulse);
    //     hasJumped = true;
    //     isGrounded = false;
    //     animator.SetInteger("transition", 3); // estado pulo
    // }

    private void UpdateAnimation(Vector3 inputDir)
    {
        // Como não tem pulo, só animações normais
        int animState = 0; // idle

        if (Input.GetKey(KeyCode.E))
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
            lastAnimState = animState;
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isGrounded = false;
        }
    }
}
