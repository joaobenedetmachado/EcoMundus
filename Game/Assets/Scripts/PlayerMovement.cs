using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float runSpeed = 9f;
    public Transform planet;

    private Rigidbody rb;
    private Animator animator;
    private bool isGrounded;
    private int lastAnimState = -1;
    private Vector3 gravityDirection;
    private PlanetCamera planetCamera;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeRotation;

        // Encontra a câmera do planeta
        planetCamera = FindObjectOfType<PlanetCamera>();
    }

    void FixedUpdate()
    {
        gravityDirection = (planet.position - transform.position).normalized;

        // Aplica a gravidade customizada
        rb.AddForce(gravityDirection * 9.81f, ForceMode.Acceleration);

        // Input para movimento e rotação
        float v = Input.GetAxis("Vertical"); // W/S
        bool rotatingLeft = Input.GetKey(KeyCode.A);
        bool rotatingRight = Input.GetKey(KeyCode.D);

        // Usa a direção da câmera para movimento
        Vector3 camForward = Vector3.zero;
        if (planetCamera != null)
        {
            camForward = planetCamera.GetCameraForward();
        }
        else
        {
            camForward = Vector3.ProjectOnPlane(transform.forward, -gravityDirection).normalized;
        }

        // Sempre rotaciona o player para ficar alinhado com a câmera
        // Primeiro, alinha o "up" do player com a direção anti-gravidade
        Quaternion upRotation = Quaternion.FromToRotation(transform.up, -gravityDirection);

        // Depois, obtém a direção forward da câmera e alinha o player
        if (planetCamera != null)
        {
            Vector3 cameraForward = planetCamera.GetCameraForward();
            Quaternion forwardRotation = Quaternion.LookRotation(cameraForward, -gravityDirection);

            // Aplica a rotação final
            rb.MoveRotation(Quaternion.Slerp(transform.rotation, forwardRotation, 12 * Time.fixedDeltaTime));
        }
        else
        {
            // Fallback: apenas alinha com a gravidade
            rb.MoveRotation(Quaternion.Slerp(transform.rotation, upRotation * transform.rotation, 10 * Time.fixedDeltaTime));
        }

        // Movimento apenas com W/S
        Vector3 inputDir = camForward * v;

        if (inputDir.magnitude > 0)
        {
            float speed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;
            Vector3 horizontalVelocity = inputDir * speed;
            Vector3 newVelocity = horizontalVelocity + Vector3.Project(rb.linearVelocity, gravityDirection);
            rb.linearVelocity = newVelocity;
        }
        else
        {
            // Para o personagem quando não há input de movimento
            Vector3 verticalVelocity = Vector3.Project(rb.linearVelocity, gravityDirection);
            rb.linearVelocity = verticalVelocity;
        }

        UpdateAnimation(inputDir, rotatingLeft || rotatingRight);
    }

    private void UpdateAnimation(Vector3 inputDir, bool isRotating)
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
        else if (isRotating)
        {
            // Animação de idle/parado quando só está girando
            animState = 0;
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