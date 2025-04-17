using UnityEngine;

public class OffsetChanger : MonoBehaviour
{
    public int seed = -1; // -1 indicates no seed provided
    private IslandGenerator islandGenerator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Get the IslandGenerator component
        islandGenerator = GetComponent<IslandGenerator>();
        
        if (islandGenerator == null)
        {
            Debug.LogError("IslandGenerator component not found on this GameObject!");
            return;
        }
        
        // Use provided seed or generate a random one
        int usedSeed = seed;
        if (seed == -1)
        {
            usedSeed = Random.Range(0, 100000);
            //Debug.Log("Using random seed: " + usedSeed);
        }
        
        // Set random state using seed
        Random.InitState(usedSeed);
        
        // Set random offset values
        float offsetX = Random.Range(-100f, 100f);
        float offsetY = Random.Range(-100f, 100f);
        
        // Apply to island generator
        islandGenerator.offset = new Vector2(offsetX, offsetY);
        
        // Generate island with new offset
        islandGenerator.GenerateIsland();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
