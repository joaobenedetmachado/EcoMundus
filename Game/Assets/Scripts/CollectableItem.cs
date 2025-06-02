using UnityEngine;

public class CollectableItem : MonoBehaviour
{
    public string itemType; // "Capsule", "Cube" ou "Sphere"
    public bool isCollected = false;

    // Essa cor será usada quando o objeto for coletado
    private Color originalColor;
    private Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();
        if (rend != null)
        {
            originalColor = rend.material.color;
        }
    }
}