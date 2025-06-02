using UnityEngine;

public class SmoothCameraFollow : MonoBehaviour
{
    [Header("Referências")]
    public Transform player;

    [Header("Configurações do Mouse")]
    public float mouseSensitivity = 2f;
    public float smoothTime = 0.1f;

    [Header("Limites de Rotação")]
    public float verticalRotationLimit = 80f;

    [Header("Suavização")]
    public bool smoothRotation = true;

    // Variáveis privadas
    private float verticalRotation = 0f;
    private float currentMouseX = 0f;
    private float currentMouseY = 0f;
    private float mouseXVelocity = 0f;
    private float mouseYVelocity = 0f;

    void Start()
    {
        // Esconde e trava o cursor no centro da tela
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Validação básica
        if (player == null)
        {
            Debug.LogError("Player Transform não está atribuído!");
        }
    }

    void Update()
    {
        HandleMouseInput();
        HandleCursorToggle();
    }

    void HandleMouseInput()
    {
        if (player == null) return;

        // Captura input do mouse
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        if (smoothRotation)
        {
            // Suaviza o movimento do mouse
            currentMouseX = Mathf.SmoothDamp(currentMouseX, mouseX, ref mouseXVelocity, smoothTime);
            currentMouseY = Mathf.SmoothDamp(currentMouseY, mouseY, ref mouseYVelocity, smoothTime);
        }
        else
        {
            currentMouseX = mouseX;
            currentMouseY = mouseY;
        }

        // Rotação horizontal - gira o player
        player.Rotate(Vector3.up * currentMouseX);

        // Rotação vertical - inclina a câmera
        verticalRotation -= currentMouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -verticalRotationLimit, verticalRotationLimit);

        // Aplica a rotação vertical na câmera
        transform.localEulerAngles = new Vector3(verticalRotation, 0f, 0f);
    }

    void HandleCursorToggle()
    {
        // Pressiona ESC para mostrar/esconder cursor
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }

        // Clica na tela para travar o cursor novamente
        if (Input.GetMouseButtonDown(0) && Cursor.lockState == CursorLockMode.None)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    // Método público para ajustar sensibilidade em runtime
    public void SetSensitivity(float newSensitivity)
    {
        mouseSensitivity = Mathf.Clamp(newSensitivity, 0.1f, 10f);
    }

    // Método para resetar a rotação vertical
    public void ResetVerticalRotation()
    {
        verticalRotation = 0f;
        transform.localEulerAngles = new Vector3(0f, 0f, 0f);
    }
}