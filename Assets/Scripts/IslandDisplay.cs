using UnityEngine;
using System.Collections;
public class IslandDisplay : MonoBehaviour
{
    // public Renderer textureRender;
	public MeshFilter meshFilter;
	public Renderer meshRenderer;
	

	// public void DrawTexture(Texture2D texture) {
	// 	textureRender.sharedMaterial.mainTexture = texture;
	// 	textureRender.transform.localScale = new Vector3 (texture.width, 1, texture.height);
	// }

	public void DrawMesh(MeshData meshData, Texture2D texture) {
		// Use mesh not sharedMesh to create a unique instance for each prefab
		meshFilter.mesh = meshData.CreateMesh();
		meshRenderer.material.mainTexture = texture;
	}
}
