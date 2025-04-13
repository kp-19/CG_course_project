using UnityEngine;
using System.Collections;
public class IslandDisplay : MonoBehaviour
{
    public Renderer textureRender;
	public MeshFilter meshFilter;
	public Renderer meshRenderer;
	

	public void DrawTexture(Texture2D texture) {
		textureRender.sharedMaterial.mainTexture = texture;
		textureRender.transform.localScale = new Vector3 (texture.width, 1, texture.height);
	}

	public void DrawMesh(MeshData meshData, Texture2D texture) {
		meshFilter.sharedMesh = meshData.CreateMesh ();
		Debug.Log(meshFilter.sharedMesh.vertexCount);
		meshRenderer.sharedMaterial.mainTexture = texture;
	}
}
