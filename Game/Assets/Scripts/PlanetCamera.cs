using UnityEngine;

public class PlanetCamera : MonoBehaviour
{
    public Transform target;       // Player
    public Transform planet;       // Planeta

    public float distance = 6f;
    public float height = 2f;
    public float smoothTime = 0.1f;

    private Vector3 velocity = Vector3.zero;

    void LateUpdate()
    {
        if (target == null || planet == null) return;

        Vector3 gravityUp = (target.position - planet.position).normalized;
        Vector3 desiredPosition = target.position - target.forward * distance + gravityUp * height;

        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothTime);
        transform.rotation = Quaternion.LookRotation(target.position - transform.position, gravityUp);
    }
}
