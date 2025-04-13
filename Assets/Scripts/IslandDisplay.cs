using UnityEngine;
using System.Collections;
public class IslandDisplay : MonoBehaviour
{
    // public Renderer textureRender;
	public MeshFilter meshFilter;
	public Renderer meshRenderer;
	private bool meshGenerationInProgress = false;

	// public void DrawTexture(Texture2D texture) {
	// 	textureRender.sharedMaterial.mainTexture = texture;
	// 	textureRender.transform.localScale = new Vector3 (texture.width, 1, texture.height);
	// }

	public void DrawMesh(MeshData meshData, Texture2D texture) {
		// Use mesh not sharedMesh to create a unique instance for each prefab
		if (meshFilter == null || meshRenderer == null) {
			Debug.LogError("MeshFilter or MeshRenderer is null on " + gameObject.name);
			return;
		}

		meshGenerationInProgress = true;
		
		// Create mesh from data
		Mesh generatedMesh = meshData.CreateMesh();
		
		// Force garbage collection to free memory
		System.GC.Collect();
		
		// Assign mesh to filter
		meshFilter.mesh = generatedMesh;
		
		// Verify mesh was assigned correctly
		if (meshFilter.mesh == null || meshFilter.mesh.vertexCount == 0) {
			Debug.LogWarning("Mesh generation failed for " + gameObject.name + ". Retrying...");
			StartCoroutine(RetryMeshGeneration(meshData, texture));
			return;
		}
		
		// Set texture
		meshRenderer.material.mainTexture = texture;
		meshGenerationInProgress = false;
	}
	
	private IEnumerator RetryMeshGeneration(MeshData meshData, Texture2D texture) {
		// Wait a bit longer before retrying
		yield return new WaitForSeconds(0.5f);
		
		// Create and assign mesh again
		Mesh generatedMesh = meshData.CreateMesh();
		meshFilter.mesh = generatedMesh;
		
		// Verify mesh was assigned this time
		if (meshFilter.mesh == null || meshFilter.mesh.vertexCount == 0) {
			Debug.LogError("Mesh generation failed after retry for " + gameObject.name);
		} else {
			// Set texture
			meshRenderer.material.mainTexture = texture;
			Debug.Log("Mesh generation succeeded on retry for " + gameObject.name);
		}
		
		meshGenerationInProgress = false;
	}
}
