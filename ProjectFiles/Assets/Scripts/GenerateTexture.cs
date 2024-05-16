using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public class GenerateTexture : MonoBehaviour {
    /// <summary>
    /// Generates a noise map texture based on the provided noise map.
    /// </summary>
    /// <param name="noiseMap">The noise map generated from the perlin noise and FBM</param>
    /// <returns>The generated texture with the colors between black and white.</returns>
    public static Texture2D GenerateNoiseMapTexture(float[,] noiseMap) {
        int width = noiseMap.GetLength(0);
        int height = noiseMap.GetLength(1);

        Texture2D texture = new Texture2D(width, height);

        Color[] colorMap = new Color[width * height];
        
        /// Generate the color map in parallel
        Parallel.For(0, height, y => {
            for(int x = 0; x < width; x++) {
                colorMap[y * width + x] = Color.Lerp(Color.black, Color.white, noiseMap[x, y]);
            }
        });

        texture.SetPixels(colorMap);
        texture.Apply();

        return texture;
    }

    /// <summary>
    /// Generates a color map texture based on the provided noise map and regions.
    /// </summary>
    /// <param name="noiseMap">The noise map generated from the perlin noise and FBM</param>
    /// <param name="regions">The TerrainType objects that ties a color to a value in the noise map</param>
    /// <param name="smooth">Determines if the texture should be sharp or if it should blend with the next color</param>
    /// <returns></returns>
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
        
        /// Generate the color map in parallel
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
