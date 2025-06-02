using UnityEngine;

public class PlayerItemCollection : MonoBehaviour
{
    public Transform holdPosition; // Crie um GameObject filho vazio e arraste aqui
    public float pickupRadius = 2f;

    private GameObject currentItem; // Item atual carregado
    private string currentItemType; // Tipo do item atual

    void Update()
    {
        // Se n�o estiver carregando nada, verifica itens pr�ximos
        if (currentItem == null)
        {
            CheckForItems();
        }
    }

    void CheckForItems()
    {
        // Procura por itens pr�ximos
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, pickupRadius);

        foreach (Collider collider in hitColliders)
        {
            // Verifica se � um item colet�vel
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

        // Desativa f�sica
        Rigidbody itemRb = item.GetComponent<Rigidbody>();
        if (itemRb != null)
        {
            itemRb.isKinematic = true;
        }

        // Remove colis�o temporariamente
        Collider[] colliders = item.GetComponents<Collider>();
        foreach (Collider col in colliders)
        {
            col.enabled = false;
        }

        // Anexa ao jogador
        item.transform.SetParent(holdPosition);
        item.transform.localPosition = Vector3.zero;

        Debug.Log("Item coletado: " + currentItemType);
    }

    // M�todos para serem chamados pelos lixos
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
        }
    }

    // Mostra raio de coleta no editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, pickupRadius);
    }
}