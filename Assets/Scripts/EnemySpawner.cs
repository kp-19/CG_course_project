using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : ResourceSpawner
{
    [Header("Enemy Settings")]
    public GameObject enemyPrefab;
    public int minEnemies = 3;
    public int maxEnemies = 6;

    [Header("Height Spawning Settings")]
    [SerializeField] private float minNoiseHeight = 0.3f;
    [SerializeField] private float maxNoiseHeight = 0.9f;

    private float heightMultiplier = 1f;
    private AnimationCurve heightCurve;

    void Start()
    {
        if (islandGenerator != null)
        {
            heightMultiplier = islandGenerator.heightMultiplier;
            heightCurve = islandGenerator.heightCurve;
        }
        else
        {
            Debug.LogWarning(gameObject.name + ": IslandGenerator not assigned!");
        }

        base.Start();
    }

    public override void SetupAndSpawnResources(MeshData meshData)
    {
        DeleteResources();

        if (enemyPrefab == null)
        {
            Debug.LogError("Enemy prefab not assigned.");
            return;
        }

        Vector3[] vertices = meshData.vertices;
        if (vertices.Length == 0) return;

        int numberOfEnemies = Random.Range(minEnemies, maxEnemies + 1);
        int spawned = 0, attempts = 0, maxAttempts = numberOfEnemies * 10;

        while (spawned < numberOfEnemies && attempts < maxAttempts)
        {
            attempts++;
            int index = Random.Range(0, vertices.Length);
            Vector3 vertex = vertices[index];

            if (ShouldSpawnAtHeight(vertex.y))
            {
                Vector3 spawnPos = new Vector3(
                    vertex.x * meshScale + meshOffsetX,
                    vertex.y * meshScale + meshOffsetY + 0.5f,
                    vertex.z * meshScale + meshOffsetZ
                );

                GameObject enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity, transform);
                enemy.transform.localScale = Vector3.one;

                if (enemy.GetComponent<CapsuleCollider>() == null)
                    enemy.AddComponent<CapsuleCollider>();

                if (enemy.GetComponent<Rigidbody>() == null)
                {
                    Rigidbody rb = enemy.AddComponent<Rigidbody>();
                    rb.constraints = RigidbodyConstraints.FreezeRotation;
                    rb.isKinematic = true;
                }

                spawned++;
                
            }
        }

        if (spawned < numberOfEnemies)
            Debug.LogWarning($"Only spawned {spawned} out of {numberOfEnemies} enemies after {attempts} attempts.");
    }

    private float GetApproximateNoiseHeightFromMeshHeight(float meshHeight)
    {
        float unscaled = meshHeight / meshScale;
        if (heightCurve == null || heightCurve.keys.Length == 0 || heightMultiplier == 0)
            return unscaled / heightMultiplier;

        float normalized = unscaled / heightMultiplier;

        if (unscaled < 0) return Mathf.Lerp(0.0f, 0.2f, Mathf.InverseLerp(-0.1f, 0.0f, unscaled));
        if (unscaled < heightCurve.Evaluate(0.5f) * heightMultiplier)
            return Mathf.Lerp(0.2f, 0.5f, Mathf.InverseLerp(0.0f, heightCurve.Evaluate(0.5f) * heightMultiplier, unscaled));
        if (unscaled < heightCurve.Evaluate(0.7f) * heightMultiplier)
            return Mathf.Lerp(0.5f, 0.7f, Mathf.InverseLerp(heightCurve.Evaluate(0.5f) * heightMultiplier, heightCurve.Evaluate(0.7f) * heightMultiplier, unscaled));

        return Mathf.Lerp(0.7f, 1.0f, Mathf.InverseLerp(heightCurve.Evaluate(0.7f) * heightMultiplier, heightCurve.Evaluate(1.0f) * heightMultiplier, unscaled));
    }

    protected override bool ShouldSpawnAtHeight(float meshHeight)
    {
        float noiseHeight = GetApproximateNoiseHeightFromMeshHeight(meshHeight);
        return noiseHeight >= minNoiseHeight && noiseHeight <= maxNoiseHeight;
    }
}
