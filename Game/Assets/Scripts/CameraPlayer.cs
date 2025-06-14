using UnityEngine;

public class CameraPlayer : MonoBehaviour
{
    [Header("Referência")]
    public Transform player;

    [Header("Offset no Eixo X")]
    public float offsetX = 0f;

    [Header("Suavização")]
    public float smoothSpeed = 5f;

    // Posição fixa da câmera nos outros eixos
    private float initialY;
    private float initialZ;

    void Start()
    {
        if (player == null)
        {
            Debug.LogError("Player não atribuído!");
            enabled = false;
            return;
        }

        // Guarda os eixos que não devem mudar
        initialY = transform.position.y;
        initialZ = transform.position.z;
    }

    void LateUpdate()
    {
        if (player == null) return;

        // Calcula nova posição no eixo X
        float targetX = player.position.x + offsetX;
        float smoothedX = Mathf.Lerp(transform.position.x, targetX, smoothSpeed * Time.deltaTime);

        // Aplica nova posição
        transform.position = new Vector3(smoothedX, initialY, initialZ);

        // Faz a câmera olhar para o player
        transform.LookAt(player);
    }
}
