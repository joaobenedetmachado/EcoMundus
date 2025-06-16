using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float runSpeed = 9f;
    public Transform planet;
    public Transform cameraTransform; // Referência da câmera

    private Rigidbody rb;
    private Animator animator;
    private bool isGrounded;
    private int lastAnimState = -1;
    private Vector3 gravityDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    void FixedUpdate()
    {
        gravityDirection = (planet.position - transform.position).normalized;

        // Aplica a gravidade customizada
        rb.AddForce(gravityDirection * 9.81f, ForceMode.Acceleration);

        // Rotaciona o personagem para ficar "em pé" na superfície do planeta
        Quaternion targetRotation = Quaternion.FromToRotation(transform.up, -gravityDirection) * transform.rotation;
        rb.MoveRotation(Quaternion.Slerp(transform.rotation, targetRotation, 10 * Time.fixedDeltaTime));

        // Input - APENAS VERTICAL (W/S ou setas cima/baixo)
        float v = Input.GetAxis("Vertical"); // Ignora Input.GetAxis("Horizontal")

        // Ajusta forward da câmera para o plano tangente à superfície do planeta
        Vector3 camForward = Vector3.ProjectOnPlane(cameraTransform.forward, gravityDirection).normalized;

        // Calcula direção de movimento APENAS para frente/trás
        Vector3 inputDir = camForward * v; // Remove a parte horizontal (camRight * h)

        if (inputDir.magnitude > 0)
        {
            float speed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;
            Vector3 horizontalVelocity = inputDir * speed;
            Vector3 newVelocity = horizontalVelocity + Vector3.Project(rb.linearVelocity, gravityDirection);
            rb.linearVelocity = newVelocity;

            // Rotaciona o personagem na direção do movimento
            Quaternion lookRotation = Quaternion.LookRotation(inputDir, -gravityDirection);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, lookRotation, 10 * Time.fixedDeltaTime));
        }
        else
        {
            // Para o personagem quando não há input
            Vector3 verticalVelocity = Vector3.Project(rb.linearVelocity, gravityDirection);
            rb.linearVelocity = verticalVelocity;
        }

        UpdateAnimation(inputDir);
    }

    private void UpdateAnimation(Vector3 inputDir)
    {
        int animState = 0;

        if (Input.GetKey(KeyCode.E))
        {
            animState = 4;
        }
        else if (inputDir.magnitude > 0)
        {
            animState = Input.GetKey(KeyCode.LeftShift) ? 2 : 1;
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