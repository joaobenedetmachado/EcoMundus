using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 120f;
    public Transform planet;

    private Rigidbody rb;
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    void FixedUpdate()
    {
        // Direção da gravidade
        Vector3 gravityDirection = (planet.position - transform.position).normalized;

        // Aplica gravidade personalizada
        rb.AddForce(gravityDirection * 9.81f, ForceMode.Acceleration);

        // Alinha o 'up' com a gravidade
        Quaternion gravityAlignment = Quaternion.FromToRotation(transform.up, -gravityDirection);
        rb.MoveRotation(gravityAlignment * rb.rotation);

        // Inputs
        float horizontal = Input.GetAxis("Horizontal"); // A/D
        float vertical = Input.GetAxis("Vertical");     // W/S
        bool isRunning = Input.GetKey(KeyCode.LeftShift); // Corrida

        // Rotação com A/D
        if (Mathf.Abs(horizontal) > 0.01f)
        {
            Quaternion turn = Quaternion.Euler(0f, horizontal * rotationSpeed * Time.fixedDeltaTime, 0f);
            rb.MoveRotation(rb.rotation * turn);
        }

        // Movimento com W/S e corrida
        float currentSpeed = isRunning ? moveSpeed * 1.5f : moveSpeed; // 🔥 Aumenta a velocidade ao correr
        Vector3 moveDirection = transform.forward * vertical;
        Vector3 moveVelocity = moveDirection * currentSpeed;

        // Mantém a velocidade de queda
        Vector3 gravityVelocity = Vector3.Project(rb.linearVelocity, gravityDirection);

        // Aplica movimento final
        rb.linearVelocity = moveVelocity + gravityVelocity;

        // Atualiza animação
        UpdateAnimation(horizontal, vertical);
    }

    void UpdateAnimation(float h, float v)
    {
        if (animator == null) return;

        int state = 0;

        if (Input.GetKey(KeyCode.E))
        {
            state = 4;
        }
        else if (Mathf.Abs(v) > 0.1f)
        {
            state = Input.GetKey(KeyCode.LeftShift) ? 2 : 1;
        }
        else if (Mathf.Abs(h) > 0.1f)
        {
            state = 0;
        }

        animator.SetInteger("transition", state);
    }
}
