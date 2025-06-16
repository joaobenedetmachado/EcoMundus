using UnityEngine;

public class PlanetCamera : MonoBehaviour
{
    [Header("Referências")]
    public Transform target; // O player
    public Transform planet; // O planeta (mesmo do player)

    [Header("Configurações da Câmera")]
    public float distance = 10f; // Distância da câmera ao player
    public float height = 2f; // Altura da câmera em relação ao player
    public float rotationSpeed = 2f; // Velocidade de rotação com o mouse
    public float smoothTime = 0.1f; // Suavidade do movimento

    [Header("Limites da Câmera")]
    public float minVerticalAngle = -30f; // Limite inferior
    public float maxVerticalAngle = 60f; // Limite superior

    private float currentX = 0f;
    private float currentY = 0f;
    private Vector3 velocity = Vector3.zero;
    private Vector3 lastTargetPosition;

    void Start()
    {
        // Inicializa a posição da câmera
        if (target != null)
        {
            lastTargetPosition = target.position;

            // Posiciona a câmera atrás do player inicialmente
            Vector3 gravityUp = (target.position - planet.position).normalized;
            Vector3 offset = -target.forward * distance + gravityUp * height;
            transform.position = target.position + offset;
            transform.LookAt(target.position);
        }

        // Não trava o cursor mais
        Cursor.lockState = CursorLockMode.None;
    }

    void LateUpdate()
    {
        if (target == null || planet == null) return;

        // Input das teclas A/D para rotação da câmera
        float horizontalInput = 0f;
        if (Input.GetKey(KeyCode.A))
            horizontalInput = -1f;
        else if (Input.GetKey(KeyCode.D))
            horizontalInput = 1f;

        currentX += horizontalInput * rotationSpeed * 2f * Time.deltaTime;

        // Calcula a direção "para cima" baseada na gravidade do planeta
        Vector3 gravityUp = (target.position - planet.position).normalized;

        // Calcula a posição desejada da câmera
        Vector3 desiredPosition = CalculateCameraPosition(gravityUp);

        // Suaviza o movimento da câmera
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothTime);

        // Faz a câmera olhar para o target usando o up do player
        Vector3 lookDirection = target.position - transform.position;
        Vector3 playerUp = target.up;

        // Calcula a rotação da câmera baseada na orientação do player
        Quaternion lookRotation = Quaternion.LookRotation(lookDirection, playerUp);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 5f * Time.deltaTime);
    }

    private Vector3 CalculateCameraPosition(Vector3 gravityUp)
    {
        // Usa o "up" do player como referência para manter orientação correta
        Vector3 playerUp = target.up;
        Vector3 playerForward = target.forward;
        Vector3 playerRight = target.right;

        // Aplica apenas a rotação horizontal baseada no sistema de coordenadas do player
        Quaternion horizontalRotation = Quaternion.AngleAxis(currentX, playerUp);

        // Calcula a direção da câmera usando o forward do player como base
        Vector3 direction = horizontalRotation * (-playerForward);

        // Posição final da câmera
        Vector3 cameraPosition = target.position + direction * distance + playerUp * height;

        return cameraPosition;
    }

    // Método público para o player obter a direção da câmera
    public Vector3 GetCameraForward()
    {
        Vector3 playerUp = target.up;
        Vector3 playerForward = target.forward;

        Quaternion horizontalRotation = Quaternion.AngleAxis(currentX, playerUp);
        return horizontalRotation * playerForward;
    }

    // Método para ajustar a distância da câmera com scroll do mouse
    void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0f)
        {
            distance -= scroll * 2f;
            distance = Mathf.Clamp(distance, 3f, 20f);
        }
    }
}