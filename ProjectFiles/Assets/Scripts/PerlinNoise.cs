using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public class PerlinNoise {
    public static float[,] GenerateNoiseMap(TerrainSettings settings) {
        float[,] noiseMap = new float[settings.width, settings.height];

        System.Random prng = new System.Random(settings.seed);
        Vector2[] octaveOffsets = new Vector2[settings.octaves];

        for(int i = 0; i < settings.octaves; i++) {
            float offsetX = prng.Next(-100000, 100000) + settings.offset.x;
            float offsetY = prng.Next(-100000, 100000) + settings.offset.y;
            octaveOffsets[i] = new Vector2(offsetX, offsetY);
        }

        if(settings.scale <= 0) {
            settings.scale = 0.0001f;
        }

        float maxNoiseHeight = float.MinValue;
        float minNoiseHeight = float.MaxValue;
        
        Parallel.For(0, settings.height, y => {
            for(int x = 0; x < settings.width; x++) {
                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;

                for(int i = 0; i < settings.octaves; i++) {
                    float sampleX = (x - (settings.width/2)) / settings.scale * frequency + octaveOffsets[i].x;
                    float sampleY = (y - (settings.height/2)) / settings.scale * frequency + octaveOffsets[i].y;
                    noiseMap[x, y] = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;

                    noiseHeight += noiseMap[x, y] * amplitude;

                    amplitude *= settings.persistance;
                    frequency *= settings.lacunarity;
                }

                if(noiseHeight > maxNoiseHeight) {
                    maxNoiseHeight = noiseHeight;
                } else if(noiseHeight < minNoiseHeight) {
                    minNoiseHeight = noiseHeight;
                }

                noiseMap[x, y] = noiseHeight;
            }
        });

        return NormalizeNoiseMap(noiseMap, minNoiseHeight, maxNoiseHeight);
    }

    public static float[,] NormalizeNoiseMap(float[,] noiseMap, float minNoiseHeight, float maxNoiseHeight) {
        int width = noiseMap.GetLength(0);
        int height = noiseMap.GetLength(1);

        // Normalize the noise map in parallel
        Parallel.For(0, height, y => {
            for(int x = 0; x < width; x++) {
                // Normalize the height value to be between 0 and 1
                noiseMap[x, y] = (noiseMap[x, y] - minNoiseHeight) / (maxNoiseHeight - minNoiseHeight);
            }
        });

        return noiseMap;
    }
}