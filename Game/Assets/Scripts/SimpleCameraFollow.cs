using UnityEngine;

public class SimpleCameraFollow : MonoBehaviour
{
    public Transform player;
    public float mouseSensitivity = 5f;
    public float verticalRotationLimit = 80f;

    private float verticalRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // Horizontal - gira o corpo do player
        player.Rotate(Vector3.up * mouseX);

        // Vertical - inclina a câmera (o CameraPivot)
        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -verticalRotationLimit, verticalRotationLimit);
        transform.localEulerAngles = new Vector3(verticalRotation, 0f, 0f);
    }
}
