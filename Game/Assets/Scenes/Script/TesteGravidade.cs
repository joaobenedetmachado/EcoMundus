using UnityEngine;

public class TesteGravidade : MonoBehaviour
{
    public Transform planet; // planeta no centro

    public float gravity = 9.8f;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false; // desativa a gravidade padrão
        rb.constraints = RigidbodyConstraints.FreezeRotation; // opcional pra não girar louco
    }

    void FixedUpdate()
    {
        Vector3 direction = (planet.position - transform.position).normalized;

        // aplica força em direção ao centro do planeta
        rb.AddForce(direction * gravity);

        // rotaciona o personagem pra "ficar em pé" na superfície
        Quaternion targetRotation = Quaternion.FromToRotation(transform.up, -direction) * transform.rotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10 * Time.fixedDeltaTime);
    }
}
