using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomObjectSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public float spawnInterval = 15f;
    public Transform planet; // Referência ao planeta para a gravidade

    [Header("Spawn Area")]
    public float spawnRadius = 10f;
    public float spawnHeight = 5f;

    private float nextSpawnTime;
    private PrimitiveType[] objectTypes = { PrimitiveType.Cube, PrimitiveType.Sphere, PrimitiveType.Capsule };

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
        // Escolhe um tipo de objeto aleatório
        int randomIndex = Random.Range(0, objectTypes.Length);
        PrimitiveType objectType = objectTypes[randomIndex];

        // Posição aleatória
        Vector3 randomSpawnPosition = new Vector3(
            Random.Range(-spawnRadius, spawnRadius),
            spawnHeight,
            Random.Range(-spawnRadius, spawnRadius)
        );

        // Cria um novo objeto primitivo
        GameObject spawnedObject = GameObject.CreatePrimitive(objectType);
        spawnedObject.transform.position = randomSpawnPosition;

        // Adiciona o componente CollectableItem
        CollectableItem collectable = spawnedObject.AddComponent<CollectableItem>();
        collectable.itemType = objectType.ToString();

        // Adiciona a gravidade personalizada
        if (planet != null)
        {
            TesteGravidade gravityScript = spawnedObject.AddComponent<TesteGravidade>();
            gravityScript.planet = planet;
            gravityScript.gravity = 9.8f;
        }

        // Garante que o objeto tem Rigidbody
        if (spawnedObject.GetComponent<Rigidbody>() == null)
        {
            spawnedObject.AddComponent<Rigidbody>();
        }

        Debug.Log("Objeto criado: " + objectType);
    }

    // Mostra a área de spawn no editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position + Vector3.up * spawnHeight,
                           new Vector3(spawnRadius * 2, 0.1f, spawnRadius * 2));
    }
}