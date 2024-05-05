using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public class GenerateTexture : MonoBehaviour {
    public static Texture2D GenerateNoiseMapTexture(float[,] noiseMap) {
        int width = noiseMap.GetLength(0);
        int height = noiseMap.GetLength(1);

        Texture2D texture = new Texture2D(width, height);

        Color[] colorMap = new Color[width * height];
        Parallel.For(0, height, y => {
            for(int x = 0; x < width; x++) {
                colorMap[y * width + x] = Color.Lerp(Color.black, Color.white, noiseMap[x, y]);
            }
        });

        texture.SetPixels(colorMap);
        texture.Apply();

        return texture;
    }

    public static Texture2D GenerateColorMapTexture(float[,] noiseMap, TerrainType[] regions, bool smooth = false) {
        int width = noiseMap.GetLength(0);
        int height = noiseMap.GetLength(1);

        Texture2D texture = new Texture2D(width, height);
        
        if(smooth) {
            texture.filterMode = FilterMode.Bilinear;
        } else {
            texture.filterMode = FilterMode.Point;
        }
        
        texture.wrapMode = TextureWrapMode.Clamp;

        Color[] colorMap = new Color[width * height];
        Parallel.For(0, height, y => {
            for(int x = 0; x < width; x++) {
                float currentHeight = noiseMap[x, y];

                for(int i = 0; i < regions.Length; i++) {
                    if(currentHeight <= regions[i].height) {
                        colorMap[y * width + x] = regions[i].color;
                        break;
                    }
                }
            }
        });

        texture.SetPixels(colorMap);
        texture.Apply();

        return texture;
    }
}
