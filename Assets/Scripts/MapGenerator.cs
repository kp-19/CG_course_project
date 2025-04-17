using UnityEngine;
using System.Collections.Generic;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] private GameObject islandPrefab;
    [SerializeField] private int islandCount = 5;
    
    [Header("Spacing")]
    [SerializeField] private float minSpacingBetweenIslands = 200f;
    [SerializeField] private float maxSpacingBetweenIslands = 400f;
    [SerializeField] private float mapRadius = 1000f;
    
    [Header("Generation")]
    [SerializeField] private int seed = 0;
    [SerializeField] private bool useRandomSeed = true;
    [SerializeField] private Vector2 offsetRange = new Vector2(100f, 100f);
    
    private System.Random prng;
    
    void Start()
    {
        GenerateIslands();
    }
    
    public void GenerateIslands()
    {
        // Initialize seed
        if (useRandomSeed)
        {
            seed = Random.Range(0, 999999);
        }
        ////Debug.Log("Map Seed: " + seed);
        prng = new System.Random(seed);
        
        // Clear existing islands if any
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        
        List<Vector2> islandPositions = new List<Vector2>();
        
        // Generate islands with random positions
        for (int i = 0; i < islandCount; i++)
        {
            // Try to find a valid position
            Vector2 position = GetRandomPosition(islandPositions);
            islandPositions.Add(position);
            
            // Create island
            Vector3 worldPos = new Vector3(position.x, 0, position.y);
            GameObject islandObj = Instantiate(islandPrefab, worldPos, Quaternion.identity, transform);
            islandObj.name = "Island_" + i;
            
            // Set deterministic offset based on seed
            IslandGenerator islandGenerator = islandObj.GetComponent<IslandGenerator>();
            if (islandGenerator != null)
            {
                // Generate deterministic offset based on island index and seed
                Vector2 determinisiticOffset = new Vector2(
                    (float)((prng.NextDouble() * 2 - 1) * offsetRange.x),
                    (float)((prng.NextDouble() * 2 - 1) * offsetRange.y)
                );
                
                islandGenerator.offset = determinisiticOffset;
                islandGenerator.GenerateIsland();
            }
        }
    }
    
    private Vector2 GetRandomPosition(List<Vector2> existingPositions)
    {
        const int MAX_ATTEMPTS = 100;
        
        for (int attempt = 0; attempt < MAX_ATTEMPTS; attempt++)
        {
            // Generate random angle and distance within map radius
            float angle = (float)(prng.NextDouble() * 2 * Mathf.PI);
            float distance = (float)(prng.NextDouble() * mapRadius);
            
            Vector2 newPos = new Vector2(
                Mathf.Cos(angle) * distance,
                Mathf.Sin(angle) * distance
            );
            
            // Check if position is valid (not too close to existing islands)
            bool isValid = true;
            foreach (Vector2 existingPos in existingPositions)
            {
                float distanceToIsland = Vector2.Distance(newPos, existingPos);
                if (distanceToIsland < minSpacingBetweenIslands)
                {
                    isValid = false;
                    break;
                }
            }
            
            if (isValid)
            {
                return newPos;
            }
        }
        
        // Fallback if we can't find a valid position after max attempts
        float fallbackAngle = (float)(prng.NextDouble() * 2 * Mathf.PI);
        float fallbackDistance = (float)(prng.NextDouble() * mapRadius + maxSpacingBetweenIslands);
        return new Vector2(
            Mathf.Cos(fallbackAngle) * fallbackDistance,
            Mathf.Sin(fallbackAngle) * fallbackDistance
        );
    }
} 