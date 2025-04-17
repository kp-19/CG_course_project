using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceSpawner : MonoBehaviour
{
    [Header("Spawn settings")]
    public GameObject[] resourcePrefabArray;
    [Range(0, 100)]
    public float spawnChance = 10f;
    
    [Header("Island Settings")]
    public IslandGenerator islandGenerator;
    public float meshScale = 10f;  // Mesh is scaled by 10 in Unity
    public float meshOffsetX = 0f, meshOffsetZ = 0f, meshOffsetY = 0f;

    [Header("Raycast setup")]
    public float distanceBetweenCheck = 5f;
    public float heightOfCheck = 100f, rangeOfCheck = 200f;
    public LayerMask layerMask;
    protected Vector2 positivePosition, negativePosition;
    
    // This method will be called by IslandDisplay after mesh generation
    public virtual void SetupAndSpawnResources(MeshData meshData)
    {
        ////Debug.Log(gameObject.name + ": SetupAndSpawnResources called with " + meshData.vertices.Length + " vertices");
        DeleteResources();
        
        // Calculate bounds based on vertices
        Vector3[] vertices = meshData.vertices;
        Vector2[] uvs = meshData.uvs;
        
        int resourcesSpawned = 0;
        
        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 vertex = vertices[i];
            
            // Only check a subset of vertices to avoid too many resources
            if (Random.Range(0f, 100f) <= spawnChance && ShouldSpawnAtHeight(vertex.y))
            {
                // Add some randomness to position to avoid grid pattern
                float offsetX = Random.Range(-distanceBetweenCheck/2, distanceBetweenCheck/2);
                float offsetZ = Random.Range(-distanceBetweenCheck/2, distanceBetweenCheck/2);
                
                // Account for mesh scale (mesh is scaled by 10 in Unity)
                Vector3 spawnPos = new Vector3(
                    (vertex.x + offsetX) * meshScale + meshOffsetX, 
                    (vertex.y * meshScale) + meshOffsetY, 
                    (vertex.z + offsetZ) * meshScale + meshOffsetZ
                );
                
                // Only spawn if we're not too close to another vertex
                if (i % (int)(distanceBetweenCheck) == 0)
                {
                    GameObject resource = Instantiate(resourcePrefabArray[Random.Range(0, resourcePrefabArray.Length)], spawnPos, Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0)), transform);
                    resource.transform.localScale = new Vector3(3.5f, 3.5f, 3.5f);
                    
                    // Add capsule collider
                    CapsuleCollider capsuleCollider = resource.AddComponent<CapsuleCollider>();
                    
                    // Add rigidbody with constraints
                    Rigidbody rb = resource.AddComponent<Rigidbody>();
                    rb.constraints = RigidbodyConstraints.FreezePositionX | 
                                    RigidbodyConstraints.FreezePositionZ | 
                                    RigidbodyConstraints.FreezeRotationX | 
                                    RigidbodyConstraints.FreezeRotationY | 
                                    RigidbodyConstraints.FreezeRotationZ;
                    
                    resourcesSpawned++;
                }
            }
        }
        
        ////Debug.Log(gameObject.name + ": Spawned " + resourcesSpawned + " resources");
    }
    
    // Override this in child classes to filter by height
    protected virtual bool ShouldSpawnAtHeight(float height)
    {
        return true; // Base class spawns everywhere
    }

    public void Start()
    {
        if (islandGenerator == null)
        {
            islandGenerator = FindObjectOfType<IslandGenerator>();
        }
        
        // Calculate island boundaries based on island chunk size
        float islandSize = 241; // Default to 241 if not found
        float halfSize = islandSize / 2f;
        negativePosition = new Vector2(-5 * 241, -5 * 241);
        positivePosition = new Vector2(5 * 241, 5 * 241);
    }

    public void Update()
    {
        // if(Input.GetKeyDown(KeyCode.R))
        // {
        //     DeleteResources();
        //     SpawnResources();
        // }
    }

    protected void SpawnResources()
    {
        for(float x = negativePosition.x; x < positivePosition.x; x += distanceBetweenCheck)
        {
            for(float z = negativePosition.y; z < positivePosition.y; z += distanceBetweenCheck)
            {
                RaycastHit hit;
                if(Physics.Raycast(new Vector3((x * meshScale) + meshOffsetX, heightOfCheck, (z * meshScale) + meshOffsetZ), Vector3.down, out hit, rangeOfCheck, layerMask))
                {
                    // Only spawn if it's land (check the height threshold from island regions)
                    if(spawnChance > Random.Range(0f, 100f) && hit.point.y > 0.1f * meshScale)
                    {
                        GameObject resource = Instantiate(resourcePrefabArray[Random.Range(0, resourcePrefabArray.Length)], hit.point, Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0)), transform);
                        resource.transform.localScale = new Vector3(3.5f, 3.5f, 3.5f);
                    }
                }
            }
        }
    }

    protected void DeleteResources()
    {
        foreach(Transform child in transform)
        {
            if (child.gameObject.name != "Mesh" && child.gameObject.name != "TreasureSpawner" && child.gameObject.name != "EnemySpawner")
            {
                Destroy(child.gameObject);
            }
        }
    }
}
