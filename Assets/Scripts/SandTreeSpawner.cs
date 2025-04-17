using UnityEngine;

public class SandTreeSpawner : ResourceSpawner
{
    [Header("Sand Tree Specific Settings")]
    [SerializeField] private float minNoiseHeight = 0.4f;
    [SerializeField] private float maxNoiseHeight = 0.48f;
    
    private float heightMultiplier = 1f;
    private AnimationCurve heightCurve;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ////Debug.Log(gameObject.name + ": SandTreeSpawner Start - Using noise height range: " + minNoiseHeight + " to " + maxNoiseHeight);
        
        // Get the heightMultiplier and heightCurve from the IslandGenerator
        if (islandGenerator != null)
        {
            heightMultiplier = islandGenerator.heightMultiplier;
            heightCurve = islandGenerator.heightCurve;
            ////Debug.Log(gameObject.name + ": Using heightMultiplier: " + heightMultiplier + " and mesh scale: " + meshScale);
        }
        
        base.Start();
    }

    // Find the approximate noise height from the mesh height
    private float GetApproximateNoiseHeightFromMeshHeight(float meshHeight)
    {
        // Account for the mesh scale factor
        float unscaledMeshHeight = meshHeight / meshScale;
        
        // Since we can't directly invert the curve evaluation, we'll use a lookup approach
        // If we don't have a valid curve, just do simple division
        if (heightCurve == null || heightCurve.keys.Length == 0)
        {
            return unscaledMeshHeight / heightMultiplier;
        }
        
        // Use the curves from the image for approximation
        // x-axis (noise) values 0.0-0.5 map to negative/low mesh heights
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

    // Override the height check method
    protected override bool ShouldSpawnAtHeight(float meshHeight)
    {
        // Convert mesh height to approximate noise height
        float approximateNoiseHeight = GetApproximateNoiseHeightFromMeshHeight(meshHeight);
        
        bool shouldSpawn = approximateNoiseHeight >= minNoiseHeight && approximateNoiseHeight <= maxNoiseHeight;
        
       
        
        return shouldSpawn;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
