using UnityEngine;

public class IslandGenerator : MonoBehaviour
{
    public int islandWidth;
	public int islandHeight;
	public float noiseScale;

	public bool autoUpdate;

	public void GenerateIsland() {
		float[,] noiseMap = Noise.GenerateNoiseMap (islandWidth, islandHeight, noiseScale);


		IslandDisplay display = FindObjectOfType<IslandDisplay> ();
		display.DrawNoiseMap (noiseMap);
	}
}
