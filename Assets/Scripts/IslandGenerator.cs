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
    
    public int islandWidth;
	public int islandHeight;
	public float noiseScale;

	public int octaves;
	[Range(0,1)]
	public float persistance;
	public float lacunarity;

	public int seed;
	public Vector2 offset;

	public bool autoUpdate;

    public TerrainType[] regions;

	public void GenerateIsland() {
		float[,] noiseMap = Noise.GenerateNoiseMap (islandWidth, islandHeight, seed, noiseScale, octaves, persistance, lacunarity, offset);
        
        Color[] colourMap = new Color[islandWidth * islandHeight];
		for (int y = 0; y < islandHeight; y++) {
			for (int x = 0; x < islandWidth; x++) {
				float currentHeight = noiseMap [x, y];
				for (int i = 0; i < regions.Length; i++) {
					if (currentHeight <= regions [i].height) {
						colourMap [y * islandWidth + x] = regions [i].colour;
						break;
					}
				}
			}
		}

		IslandDisplay display = FindAnyObjectByType<IslandDisplay> ();
		if (drawMode == DrawMode.NoiseMap) {
			display.DrawTexture (TextureGenerator.TextureFromHeightMap(noiseMap));
		} else if (drawMode == DrawMode.ColourMap) {
			display.DrawTexture (TextureGenerator.TextureFromColourMap(colourMap, islandWidth, islandHeight));
		} else if (drawMode == DrawMode.Mesh) {
			display.DrawMesh(MeshGenerator.GenerateTerrainMesh(noiseMap), TextureGenerator.TextureFromColourMap(colourMap, islandWidth, islandHeight));
		} 
	}

	void OnValidate() {
		if (islandWidth < 1) {
			islandWidth = 1;
		}
		if (islandHeight < 1) {
			islandHeight = 1;
		}
		if (lacunarity < 1) {
			lacunarity = 1;
		}
		if (octaves < 0) {
			octaves = 0;
		}
	}
}

[System.Serializable]
public struct TerrainType {
	public string name;
	public float height;
	public Color colour;
}
