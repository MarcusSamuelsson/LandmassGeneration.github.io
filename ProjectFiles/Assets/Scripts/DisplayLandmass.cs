using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public class DisplayLandmass : MonoBehaviour {
    public Renderer textureRenderer;
    public MeshRenderer plane, terrain;

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
