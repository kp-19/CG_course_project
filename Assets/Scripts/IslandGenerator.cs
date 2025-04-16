using UnityEngine;
using System.Collections;
public class IslandGenerator : MonoBehaviour
{
    public enum DrawMode {
        NoiseMap,
        ColourMap,
        Mesh,
        FalloffMap
    }

    public DrawMode drawMode;
    
	const int islandChunkSize = 241;
    [Range(0,6)]
	public int levelOfDetail;
	public float noiseScale;

	public float heightMultiplier;
	public AnimationCurve heightCurve;

	public bool useFalloff;
	float[,] falloffMap;

	public int octaves;
	[Range(0,1)]
	public float persistance;
	public float lacunarity;

	public int seed;
	public Vector2 offset;

	public bool autoUpdate;

    public TerrainType[] regions;

	void Awake() {
		Debug.Log("IslandGenerator Awake - Generating falloff map");
		falloffMap = FalloffGenerator.GenerateFalloffMap(islandChunkSize);
		GenerateIsland();
	}

	public void GenerateIsland() {
		Debug.Log("GenerateIsland called - Creating noise map of size " + islandChunkSize);
		float[,] noiseMap = Noise.GenerateNoiseMap (islandChunkSize, islandChunkSize, seed, noiseScale, octaves, persistance, lacunarity, offset);
        
        Color[] colourMap = new Color[islandChunkSize * islandChunkSize];
		for (int y = 0; y < islandChunkSize; y++) {
			for (int x = 0; x < islandChunkSize; x++) {
				if (useFalloff) {
					noiseMap[x, y] = Mathf.Clamp01(noiseMap[x, y] - falloffMap[x, y]);
				}
				float currentHeight = noiseMap [x, y];
				for (int i = 0; i < regions.Length; i++) {
					if (currentHeight <= regions [i].height) {
						colourMap [y * islandChunkSize + x] = regions [i].colour;
						break;
					}
				}
			}
		}

		IslandDisplay display = FindAnyObjectByType<IslandDisplay> ();
		if (display == null) {
			Debug.LogError("No IslandDisplay found in the scene!");
			return;
		}
		
		Debug.Log("Drawing mesh with " + drawMode + " mode");
		if (drawMode == DrawMode.NoiseMap) {
			//display.DrawTexture (TextureGenerator.TextureFromHeightMap(noiseMap));
			Debug.LogWarning("NoiseMap mode is commented out");
		} else if (drawMode == DrawMode.ColourMap) {
			//display.DrawTexture (TextureGenerator.TextureFromColourMap(colourMap, islandChunkSize, islandChunkSize));
			Debug.LogWarning("ColourMap mode is commented out");
		} else if (drawMode == DrawMode.Mesh) {
			Debug.Log("Generating terrain mesh and texture for mesh display");
			MeshData meshData = MeshGenerator.GenerateTerrainMesh(noiseMap, heightMultiplier, heightCurve, levelOfDetail);
			Texture2D texture = TextureGenerator.TextureFromColourMap(colourMap, islandChunkSize, islandChunkSize);
			display.DrawMesh(meshData, texture);
			Debug.Log("Mesh drawing completed successfully");
		} else if (drawMode == DrawMode.FalloffMap) {
			//display.DrawTexture(TextureGenerator.TextureFromHeightMap(FalloffGenerator.GenerateFalloffMap(islandChunkSize)));
			Debug.LogWarning("FalloffMap mode is commented out");
		}
	}

	void OnValidate() {
		
		if (lacunarity < 1) {
			lacunarity = 1;
		}
		if (octaves < 0) {
			octaves = 0;
		}

		if (useFalloff) {
			falloffMap = FalloffGenerator.GenerateFalloffMap(islandChunkSize);
		}
	}
}

[System.Serializable]
public struct TerrainType {
	public string name;
	public float height;
	public Color colour;
}
