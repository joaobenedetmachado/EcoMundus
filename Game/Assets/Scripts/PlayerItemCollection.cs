using UnityEngine;

public class PlayerItemCollection : MonoBehaviour
{
    public Transform holdPosition; // Crie um GameObject filho vazio e arraste aqui
    public float pickupRadius = 2f;

    private GameObject currentItem; // Item atual carregado
    private string currentItemType; // Tipo do item atual
    private Animator animator; // Referência do Animator

    void Start()
    {
        // Obtém o componente Animator
        animator = GetComponent<Animator>();

        if (animator == null)
        {
            Debug.LogError("Animator não encontrado no GameObject!");
        }
    }

    void Update()
    {
        // Se não estiver carregando nada, verifica itens próximos
        if (currentItem == null)
        {
            CheckForItems();
        }
    }

    void CheckForItems()
    {
        // Procura por itens próximos
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, pickupRadius);
        foreach (Collider collider in hitColliders)
        {
            // Verifica se é um item coletável
            CollectableItem item = collider.GetComponent<CollectableItem>();
            if (item != null && !item.isCollected)
            {
                // Pega o item
                PickupItem(collider.gameObject, item);
                break; // Pega apenas um item
            }
        }
    }

    void PickupItem(GameObject item, CollectableItem collectableComponent)
    {
        currentItem = item;
        currentItemType = collectableComponent.itemType;

        // Marca como coletado
        collectableComponent.isCollected = true;

        // Desativa física
        Rigidbody itemRb = item.GetComponent<Rigidbody>();
        if (itemRb != null)
        {
            itemRb.isKinematic = true;
        }

        // Remove colisão temporariamente
        Collider[] colliders = item.GetComponents<Collider>();
        foreach (Collider col in colliders)
        {
            col.enabled = false;
        }

        // Anexa ao jogador
        item.transform.SetParent(holdPosition);
        item.transform.localPosition = Vector3.zero;

        // Muda o estado da animação para 4 quando pega o item
        if (animator != null)
        {
            animator.SetInteger("transition", 4);
        }

        Debug.Log("Item coletado: " + currentItemType);
    }

    // Métodos para serem chamados pelos lixos
    public GameObject GetCurrentItem()
    {
        return currentItem;
    }

    public string GetCurrentItemType()
    {
        return currentItemType;
    }

    public void DropCurrentItem()
    {
        if (currentItem != null)
        {
            Destroy(currentItem);
            currentItem = null;
            currentItemType = "";

            // Volta o estado da animação para 0 (idle) quando solta o item
            if (animator != null)
            {
                animator.SetInteger("transition", 0);
            }
        }
    }

    // Verifica se está carregando um item
    public bool IsCarryingItem()
    {
        return currentItem != null;
    }

    // Mostra raio de coleta no editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, pickupRadius);
    }
}