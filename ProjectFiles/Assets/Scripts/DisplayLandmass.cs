using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public class DisplayLandmass : MonoBehaviour {
    public Renderer textureRenderer;
    public MeshRenderer plane, terrain;

    /// <summary>
    /// Draws the noise map based on the provided noise map, draw mode, settings, and mesh object.
    /// </summary>
    /// <param name="noiseMap">The noise map generated from the perlin noise and FBM</param>
    /// <param name="drawMode">An enum that determince how the terrain should be generated</param>
    /// <param name="settings">The terrain settings used for creating the terrain(Used for generating color and mesh)</param>
    /// <param name="meshObject">This gives if the mesh should be attached to a specific GameObject in th scene or if it is null it will chose gameobject with this script on it.</param>
    public void DrawNoiseMap(float[,] noiseMap, LandmassGenerator.DrawMode drawMode, TerrainSettings settings, GameObject meshObject = null) {
        int size = noiseMap.GetLength(0);

        Texture2D texture;

        if(drawMode == LandmassGenerator.DrawMode.NoiseMap) {
            texture = GenerateTexture.GenerateNoiseMapTexture(noiseMap);
        } else if (drawMode == LandmassGenerator.DrawMode.ColorMap){
            texture = GenerateTexture.GenerateColorMapTexture(noiseMap, settings.regions);
        } else {
            texture = GenerateTexture.GenerateColorMapTexture(noiseMap, settings.regions, true);
        }

        if(meshObject == null) {
            textureRenderer.sharedMaterial.mainTexture = texture;
            textureRenderer.transform.localScale = new Vector3(size, 1, size);
        } else {
            Material material = new Material(Shader.Find("Standard"));
            material.mainTexture = texture;
            meshObject.GetComponent<MeshRenderer>().sharedMaterial = material;
        }
        

        if(drawMode == LandmassGenerator.DrawMode.Mesh) {
            plane.enabled = false;
            
            if(meshObject == null) {
                MeshGenerator.GenerateTerrainMesh(noiseMap, settings, terrain.gameObject);
                terrain.enabled = true;
            } else {
                MeshGenerator.GenerateTerrainMesh(noiseMap, settings, meshObject);
            }
            
        } else {
            plane.enabled = true;
            terrain.enabled = false;
        }
    }
}
