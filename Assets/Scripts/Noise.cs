using UnityEngine;

public class Noise
{
    public static float[,] GenerateNoiseMap(int islandWidth, int islandHeight, float scale) {
		float[,] noiseMap = new float[islandWidth,islandHeight];

		if (scale <= 0) {
			scale = 0.0001f;
		}

		for (int y = 0; y < islandHeight; y++) {
			for (int x = 0; x < islandWidth; x++) {
				float sampleX = x / scale;
				float sampleY = y / scale;

				float perlinValue = Mathf.PerlinNoise (sampleX, sampleY);
				noiseMap [x, y] = perlinValue;
			}
		}

		return noiseMap;
	}
}
