using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomObjectSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject[] myObjects; // COLOQUE PREFABS AQUI, NÃO OBJETOS DA CENA
    public float spawnInterval = 15f;
    public Transform planet; // Referência ao planeta para a gravidade

    [Header("Spawn Area")]
    public float spawnRadius = 10f;
    public float spawnHeight = 5f;

    private float nextSpawnTime;

    void Start()
    {
        // Define o primeiro spawn
        nextSpawnTime = Time.time + spawnInterval;
    }

    void Update()
    {
        // Verifica se é hora de spawnar
        if (Time.time >= nextSpawnTime)
        {
            SpawnRandomObject();
            nextSpawnTime = Time.time + spawnInterval;
        }
    }

    void SpawnRandomObject()
    {
        if (myObjects.Length == 0)
        {
            Debug.LogWarning("Nenhum prefab configurado para spawnar!");
            return;
        }

        // Escolhe um prefab aleatório
        int randomIndex = Random.Range(0, myObjects.Length);
        GameObject prefab = myObjects[randomIndex];

        if (prefab == null)
        {
            Debug.LogError("Prefab no índice " + randomIndex + " está null!");
            return;
        }

        // Posição aleatória
        Vector3 randomSpawnPosition = new Vector3(
            Random.Range(-spawnRadius, spawnRadius),
            spawnHeight,
            Random.Range(-spawnRadius, spawnRadius)
        );

        // Instancia o prefab (cria uma cópia)
        GameObject spawnedObject = Instantiate(prefab, randomSpawnPosition, Quaternion.identity);

        // Adiciona a gravidade personalizada automaticamente
        if (planet != null)
        {
            TesteGravidade gravityScript = spawnedObject.GetComponent<TesteGravidade>();
            if (gravityScript == null)
            {
                gravityScript = spawnedObject.AddComponent<TesteGravidade>();
            }

            // Configura a referência do planeta
            gravityScript.planet = planet;
            gravityScript.gravity = 9.8f;
        }

        // Garante que o objeto tem Rigidbody
        if (spawnedObject.GetComponent<Rigidbody>() == null)
        {
            spawnedObject.AddComponent<Rigidbody>();
        }

        Debug.Log("Prefab spawnado: " + prefab.name);
    }

    // Mostra a área de spawn no editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position + Vector3.up * spawnHeight,
                           new Vector3(spawnRadius * 2, 0.1f, spawnRadius * 2));
    }
}