using UnityEngine;

public class SmoothCameraFollow : MonoBehaviour
{
    [Header("Refer�ncias")]
    public Transform player;

    [Header("Configura��es do Mouse")]
    public float mouseSensitivity = 2f;
    public float smoothTime = 0.1f;

    [Header("Limites de Rota��o")]
    public float verticalRotationLimit = 80f;

    [Header("Suaviza��o")]
    public bool smoothRotation = true;

    // Vari�veis privadas
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

        // Valida��o b�sica
        if (player == null)
        {
            Debug.LogError("Player Transform n�o est� atribu�do!");
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

        // Rota��o horizontal - gira o player
        player.Rotate(Vector3.up * currentMouseX);

        // Rota��o vertical - inclina a c�mera
        verticalRotation -= currentMouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -verticalRotationLimit, verticalRotationLimit);

        // Aplica a rota��o vertical na c�mera
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

    // M�todo p�blico para ajustar sensibilidade em runtime
    public void SetSensitivity(float newSensitivity)
    {
        mouseSensitivity = Mathf.Clamp(newSensitivity, 0.1f, 10f);
    }

    // M�todo para resetar a rota��o vertical
    public void ResetVerticalRotation()
    {
        verticalRotation = 0f;
        transform.localEulerAngles = new Vector3(0f, 0f, 0f);
    }
}