using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IslandDisplay : MonoBehaviour
{
    // public Renderer textureRender;
	public MeshFilter meshFilter;
	public Renderer meshRenderer;
	public MeshCollider meshCollider;
	
	[Header("Resource Spawners")]
	public List<ResourceSpawner> resourceSpawners;

	// public void DrawTexture(Texture2D texture) {
	// 	textureRender.sharedMaterial.mainTexture = texture;
	// 	textureRender.transform.localScale = new Vector3 (texture.width, 1, texture.height);
	// }

	public void DrawMesh(MeshData meshData, Texture2D texture) {
		Debug.Log("DrawMesh called - Creating mesh from meshData");
		if (meshData == null) {
			Debug.LogError("MeshData is null!");
			return;
		}
		
		Debug.Log("MeshData has " + meshData.vertices.Length + " vertices");
		
		// Use mesh not sharedMesh to create a unique instance for each prefab
		Mesh generatedMesh = meshData.CreateMesh();
		Debug.Log("Mesh created with " + generatedMesh.vertexCount + " vertices");
		
		meshFilter.mesh = generatedMesh;
		meshCollider.sharedMesh = meshFilter.mesh;
		meshRenderer.material.mainTexture = texture;
		
		Debug.Log("Mesh assigned to meshFilter and meshCollider");
		
		// Spawn resources using mesh data
		if (resourceSpawners != null && resourceSpawners.Count > 0) {
			Debug.Log("Spawning resources with " + resourceSpawners.Count + " spawners");
			foreach (ResourceSpawner spawner in resourceSpawners) {
				if (spawner != null) {
					spawner.SetupAndSpawnResources(meshData);
				}
			}
		} else {
			Debug.LogWarning("No resource spawners assigned!");
		}
	}
}
