using UnityEngine;

public class SmoothCameraFollow : MonoBehaviour
{
    [Header("Referências")]
    public Transform player;

    [Header("Configurações da Câmera")]
    public float rotationSpeed = 120f;
    public float cameraDistance = 5f;
    public float cameraHeight = 2f;
    public float smoothSpeed = 10f;

    [Header("Controle do Cursor")]
    public bool lockCursor = true;

    private float currentYRotation = 0f;

    void Start()
    {
        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        if (player == null)
        {
            Debug.LogError("Player Transform não está atribuído!");
        }

        if (player != null)
        {
            currentYRotation = player.eulerAngles.y;
        }
    }

    void Update()
    {
        HandleInput();
        HandleCursorToggle();
    }

    void LateUpdate()
    {
        if (player != null)
        {
            FollowPlayer();
        }
    }

    void HandleInput()
    {
        float horizontalInput = 0f;

        if (Input.GetKey(KeyCode.A))
            horizontalInput = -1f;
        if (Input.GetKey(KeyCode.D))
            horizontalInput = 1f;

        if (horizontalInput != 0f)
        {
            float rotationAmount = horizontalInput * rotationSpeed * Time.deltaTime;
            currentYRotation += rotationAmount;

            // Gira o jogador no eixo Y
            player.rotation = Quaternion.Euler(0, currentYRotation, 0);
        }
    }

    void FollowPlayer()
    {
        // Calcula a posição desejada da câmera
        Vector3 offset = new Vector3(0, cameraHeight, -cameraDistance);
        Vector3 rotatedOffset = Quaternion.Euler(0, currentYRotation, 0) * offset;
        Vector3 targetPosition = player.position + rotatedOffset;

        // Move a câmera suavemente para a posição desejada
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);

        // Faz a câmera olhar para o ponto correto do player
        transform.LookAt(player.position + Vector3.up * cameraHeight);
    }

    void HandleCursorToggle()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        if (Input.GetMouseButtonDown(0) && Cursor.lockState == CursorLockMode.None)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
