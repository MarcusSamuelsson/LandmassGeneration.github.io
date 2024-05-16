using System.Collections;
using UnityEngine;

public class LandmassGenerator : MonoBehaviour {
    public enum DrawMode {NoiseMap, ColorMap, Mesh};

    public DrawMode drawMode;
    //public const int size = 241;
    
    public TerrainSettings settings;

    public bool autoUpdate;

    public void Start () {
        GenerateMap();
    }

    /// <summary>
    /// Generates a new map based on the settings provided.
    /// </summary>
    public void GenerateMap() {
        float[,] noiseMap = PerlinNoise.GenerateNoiseMap(settings);

        DisplayLandmass display = FindObjectOfType<DisplayLandmass>();

        display.DrawNoiseMap(noiseMap, drawMode, settings);
    }

    /// <summary>
    /// Validates the settings to make sure they are within the correct range.
    /// </summary>
    void OnValidate() {
        if(settings.lacunarity < 1) {
            settings.lacunarity = 1;
        }

        if(settings.octaves < 0) {
            settings.octaves = 0;
        }

        settings.maxLOD = 0;

        for(int i = 1; i <= 6; i++) {
            if((settings.width-1) % (i*2) == 0 && (settings.height-1) % (i*2) == 0) {
                settings.maxLOD++;
            } else {
                break;
            }
        }

        if(settings.lod < 0) {
            settings.lod = 0;
        } else if(settings.lod > settings.maxLOD) {
            settings.lod = settings.maxLOD;
        }

        while(settings.height * settings.width > 65536) {
            if(settings.height > settings.width && settings.height > 10) {
                settings.height--;
            } else {
                settings.width--;
            }
        }

        if(settings.height < 10) {
            settings.height = 10;
        }

        if(settings.width < 10) {
            settings.width = 10;
        }
    }
}

/// <summary>
/// A struct that holds the name and prefab of a nature object.
/// </summary>
[System.Serializable]
public struct NatureObject {
    public string name;
    public GameObject prefab;
    public float minHeight, maxHeight;
}

/// <summary>
/// A struct that holds the name, height, and color of a terrain type.
/// </summary>
[System.Serializable]
public struct TerrainType {
    public string name;
    [Range(0, 1)]
    public float height;
    public Color color;
}

/// <summary>
/// A struct that holds the settings for the terrain generation.
/// </summary>
[System.Serializable]
public class TerrainSettings {
    public int width = 241;
    public int height = 241;
    public float scale;

    public float heightMultiplier;
    public AnimationCurve heightCurve;
    
    public int octaves;
    [Range(0, 1)]
    public float persistance;
    public float lacunarity;

    public int seed;
    public Vector2 offset;

    public TerrainType[] regions;
    public int lod = 0;
    public int maxLOD;
}