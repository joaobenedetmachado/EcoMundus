using UnityEngine;
using UnityEngine.SceneManagement;

public class TrashBin : MonoBehaviour
{
    public string acceptedItemType; // Configure como "Capsule", "Cube" ou "Sphere"
    public float detectionRadius = 2f;

    private ScoreManager scoreManager;

    void Start()
    {
        // Encontra o gerenciador de pontua��o
        scoreManager = FindObjectOfType<ScoreManager>();
        if (scoreManager == null)
        {
            Debug.LogError("ScoreManager n�o encontrado! Crie um objeto com esse script.");
        }
    }

    void Update()
    {
        // Procura por jogador pr�ximo
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius);

        foreach (Collider collider in hitColliders)
        {
            // Verifica se � o player
            PlayerItemCollection player = collider.GetComponent<PlayerItemCollection>();

            if (player != null)
            {
                // Verifica se o player est� carregando algo
                GameObject currentItem = player.GetCurrentItem();
                string currentItemType = player.GetCurrentItemType();

                if (currentItem != null)
                {
                    // Verifica se o item � do tipo correto
                    if (currentItemType == acceptedItemType)
                    {
                        // Deposita o item correto: +200 pontos
                        scoreManager.AddPoints(200);
                        player.DropCurrentItem();
                        Debug.Log("Item correto depositado! +200 pontos");
                    }
                    else
                    {
                        // Item incorreto: sem pontos
                        player.DropCurrentItem();
                        Debug.Log("Item incorreto depositado!");
                    }
                }
            }
        }
    }

    // Mostra raio de detec��o no editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}