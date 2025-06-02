using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Transform planet;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // entrada do jogador
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 inputDir = new Vector3(h, 0f, v).normalized;

        if (inputDir.magnitude == 0)
            return;

        // direção da gravidade (centro do planeta)
        Vector3 gravityUp = (transform.position - planet.position).normalized;

        // cria um "plano tangente" na superfície do planeta
        Vector3 forward = Vector3.Cross(transform.right, gravityUp).normalized;
        Vector3 right = Vector3.Cross(gravityUp, forward).normalized;

        // calcula direção final no plano tangente
        Vector3 moveDir = (forward * v + right * h).normalized;

        // move o jogador aplicando velocidade
        rb.MovePosition(rb.position + moveDir * moveSpeed * Time.fixedDeltaTime);
    }
}