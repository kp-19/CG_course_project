using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureChestSpawner : ResourceSpawner
{
    [Header("Treasure Chest Settings")]
    public GameObject treasureChestPrefab;
    public int minChests = 2;
    public int maxChests = 4;

    [Header("Height Spawning Settings")]
    [SerializeField] private float minNoiseHeight = 0.0f; // Default: Spawn anywhere
    [SerializeField] private float maxNoiseHeight = 1.0f; // Default: Spawn anywhere

    private float heightMultiplier = 1f;
    private AnimationCurve heightCurve;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Debug.Log(gameObject.name + ": TreasureChestSpawner Start - Using noise height range: " + minNoiseHeight + " to " + maxNoiseHeight);

        // Get the heightMultiplier and heightCurve from the IslandGenerator
        if (islandGenerator != null)
        {
            heightMultiplier = islandGenerator.heightMultiplier;
            heightCurve = islandGenerator.heightCurve;
            //Debug.Log(gameObject.name + ": Using heightMultiplier: " + heightMultiplier + " and mesh scale: " + meshScale);
        }
        else
        {
             Debug.LogWarning(gameObject.name + ": IslandGenerator not found. Cannot get height curve/multiplier. Using defaults.");
        }

        // Ensure base Start logic is also called if needed (e.g., setting up bounds)
        base.Start();
    }

    // Override the base method to spawn treasure chests
    public override void SetupAndSpawnResources(MeshData meshData)
    {
        //Debug.Log(gameObject.name + ": SetupAndSpawnResources called for Treasure Chests");
        DeleteResources(); // Reuse the DeleteResources method from the base class

        if (treasureChestPrefab == null)
        {
            Debug.LogError("TreasureChestPrefab is not assigned!");
            return;
        }

        Vector3[] vertices = meshData.vertices;
        if (vertices.Length == 0)
        {
            Debug.LogWarning("No vertices found in mesh data to spawn chests.");
            return;
        }

        int numberOfChestsToSpawn = Random.Range(minChests, maxChests + 1); // +1 because Random.Range for int is exclusive for max
        int chestsSpawned = 0;
        int attempts = 0;
        int maxAttempts = numberOfChestsToSpawn * 10; // Limit attempts to avoid infinite loops

        //Debug.Log(gameObject.name + ": Attempting to spawn " + numberOfChestsToSpawn + " treasure chests.");

        while (chestsSpawned < numberOfChestsToSpawn && attempts < maxAttempts)
        {
            attempts++;
            
            // Pick a random vertex
            int randomIndex = Random.Range(0, vertices.Length);
            Vector3 vertex = vertices[randomIndex];

            // Check if this height is suitable for spawning
            if (ShouldSpawnAtHeight(vertex.y))
            {
                // Add some randomness to position if desired, or keep it precise
                // float offsetX = Random.Range(-distanceBetweenCheck / 4, distanceBetweenCheck / 4);
                // float offsetZ = Random.Range(-distanceBetweenCheck / 4, distanceBetweenCheck / 4);
                float offsetX = 0;
                float offsetZ = 0;


                // Account for mesh scale and offsets defined in the base class
                Vector3 spawnPos = new Vector3(
                    (vertex.x + offsetX) * meshScale + meshOffsetX,
                    (vertex.y * meshScale) + meshOffsetY + 0.5f, // Add a small offset to prevent clipping
                    (vertex.z + offsetZ) * meshScale + meshOffsetZ
                );
                Debug.Log(gameObject.name + ": Spawning chest at " + spawnPos);

                GameObject chest = Instantiate(treasureChestPrefab, spawnPos, Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0)), transform);
                chest.transform.localScale = new Vector3(1f, 1f, 1f); // Adjust scale as needed for the chest prefab

                // Add necessary components if they don't exist on the prefab
                if (chest.GetComponent<CapsuleCollider>() == null)
                {
                    CapsuleCollider capsuleCollider = chest.AddComponent<CapsuleCollider>();
                    // Configure collider if needed (e.g., radius, height)
                }
                if (chest.GetComponent<Rigidbody>() == null)
                {
                     Rigidbody rb = chest.AddComponent<Rigidbody>();
                    rb.constraints = RigidbodyConstraints.FreezePositionX |
                                     RigidbodyConstraints.FreezePositionZ |
                                     RigidbodyConstraints.FreezeRotationX |
                                     RigidbodyConstraints.FreezeRotationY |
                                     RigidbodyConstraints.FreezeRotationZ;
                    rb.isKinematic = true; // Make kinematic initially to prevent falling through floor before placement stabilizes
                }


                chestsSpawned++;
            }
        }
        
        if (chestsSpawned < numberOfChestsToSpawn) {
            Debug.LogWarning(gameObject.name + ": Could only spawn " + chestsSpawned + " out of " + numberOfChestsToSpawn + " requested chests after " + attempts + " attempts.");
        } else {
             //Debug.Log(gameObject.name + ": Successfully spawned " + chestsSpawned + " treasure chests.");
        }
    }

    // Find the approximate noise height from the mesh height
    private float GetApproximateNoiseHeightFromMeshHeight(float meshHeight)
    {
        // Account for the mesh scale factor
        float unscaledMeshHeight = meshHeight / meshScale;

        // Since we can't directly invert the curve evaluation, we'll use a lookup approach
        // If we don't have a valid curve or multiplier, return a value that likely allows spawning
        if (heightCurve == null || heightCurve.keys.Length == 0 || heightMultiplier == 0)
        {
            // Maybe return a mid-range value or the unscaled height directly?
            // Returning 0.5 as a guess, adjust if needed.
            return unscaledMeshHeight / heightMultiplier;
        }

        // Normalize the unscaled height relative to the multiplier
        float normalizedHeight = unscaledMeshHeight / heightMultiplier;

        // Find the input time (noise value) that corresponds to the normalizedHeight
        // This requires iterating through the curve keys or using a precomputed lookup table
        // for efficiency. For simplicity here, we do a basic linear search/approximation.
        
        // Handle edge cases
        if (unscaledMeshHeight < 0)
        {
            return Mathf.Lerp(0.0f, 0.2f, Mathf.InverseLerp(-0.1f, 0.0f, unscaledMeshHeight));
        }
        
        if (unscaledMeshHeight < heightCurve.Evaluate(0.5f) * heightMultiplier)
        {
            return Mathf.Lerp(0.2f, 0.5f, Mathf.InverseLerp(0.0f, heightCurve.Evaluate(0.5f) * heightMultiplier, unscaledMeshHeight));
        }
        
        if (unscaledMeshHeight < heightCurve.Evaluate(0.7f) * heightMultiplier)
        {
            return Mathf.Lerp(0.5f, 0.7f, Mathf.InverseLerp(heightCurve.Evaluate(0.5f) * heightMultiplier, 
                                                          heightCurve.Evaluate(0.7f) * heightMultiplier, unscaledMeshHeight));
        }
        
        return Mathf.Lerp(0.7f, 1.0f, Mathf.InverseLerp(heightCurve.Evaluate(0.7f) * heightMultiplier, 
                                                      heightCurve.Evaluate(1.0f) * heightMultiplier, unscaledMeshHeight));
    }

    // Override this if chests should only spawn at specific heights (e.g., beaches)
    protected override bool ShouldSpawnAtHeight(float meshHeight)
    {
        // Convert mesh height to approximate noise height
        float approximateNoiseHeight = GetApproximateNoiseHeightFromMeshHeight(meshHeight);

        bool shouldSpawn = approximateNoiseHeight >= minNoiseHeight && approximateNoiseHeight <= maxNoiseHeight;

        // Optional: Add logging for debugging
        // if (Random.Range(0, 100) < 5)
        // {
        //     //Debug.Log(gameObject.name + ": Mesh height: " + meshHeight +
        //               " → Unscaled: " + (meshHeight / meshScale) +
        //               " → Approx noise height: " + approximateNoiseHeight +
        //               " → In range [" + minNoiseHeight + "-" + maxNoiseHeight + "]: " + shouldSpawn);
        // }

        return shouldSpawn;
    }

    // No need for Update() unless specific logic is needed for chests
} 