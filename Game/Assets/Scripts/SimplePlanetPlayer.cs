using UnityEngine;

public class SimplePlanetPlayer : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float jumpForce = 18f;
    public float gravityForce = 120f;
    public Transform planet;

    private Rigidbody rb;
    private bool isGrounded = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    void FixedUpdate()
    {
        // Aplica gravidade pro centro do planeta
        Vector3 gravityDirection = (planet.position - transform.position).normalized;
        rb.AddForce(gravityDirection * gravityForce);

        // Alinha o jogador com o chão
        Quaternion targetRotation = Quaternion.FromToRotation(transform.up, -gravityDirection) * transform.rotation;
        rb.MoveRotation(Quaternion.Slerp(transform.rotation, targetRotation, 10 * Time.fixedDeltaTime));

        // Movimento básico (WSAD)
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 forward = Vector3.Cross(transform.right, -gravityDirection).normalized;
        Vector3 right = Vector3.Cross(-gravityDirection, forward).normalized;
        Vector3 moveDir = (forward * v + right * h).normalized;

        rb.MovePosition(rb.position + moveDir * walkSpeed * Time.fixedDeltaTime);

        // PULO SEM RAIVA
        if (Input.GetKey(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(-gravityDirection * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    // Detecta contato com o chão de forma simples
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

