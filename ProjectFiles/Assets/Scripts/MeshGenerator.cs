using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGenerator : MonoBehaviour {
    /// <summary>
    /// Generates a terrain mesh based on the provided height map, terrain settings, and mesh object.
    /// </summary>
    /// <param name="heightMap">The 2D array representing the height map of the terrain.</param>
    /// <param name="settings">The settings for generating the terrain mesh.</param>
    /// <param name="meshObject">The GameObject to which the generated mesh will be assigned.</param>
    public static void GenerateTerrainMesh(float[,] heightMap , TerrainSettings settings, GameObject meshObject) {
        int width = heightMap.GetLength(0);
        int height = heightMap.GetLength(1);

        float topLeftX = (width - 1) / -2f;
        float topLeftZ = (height - 1) / 2f;

        int meshSimplificationIncrement = (settings.lod == 0) ? 1 : settings.lod * 2;

        Vector2Int verticesPerLine = new Vector2Int((width - 1) / meshSimplificationIncrement + 1, (height - 1) / meshSimplificationIncrement + 1);

        MeshData meshData = new MeshData(verticesPerLine);

        int vertexIndex = 0;

        /// Generate the vertices for the mesh
        for(int y = 0; y < height; y += meshSimplificationIncrement) {
            for(int x = 0; x < width; x += meshSimplificationIncrement) {
                meshData.vertices[vertexIndex] = new Vector3(topLeftX + x, settings.heightCurve.Evaluate(heightMap[x, y]) * settings.heightMultiplier , topLeftZ - y);
                meshData.uvs[vertexIndex] = new Vector2(x / (float)width, y / (float)height);

                if(x < width - 1 && y < height - 1) {
                    meshData.AddQuad(vertexIndex, vertexIndex + verticesPerLine.x + 1, vertexIndex + verticesPerLine.x, vertexIndex + 1);
                }

                vertexIndex++;
            }
        }

        Mesh mesh = meshData.CreateMesh();
    
        meshObject.GetComponent<MeshFilter>().mesh = mesh;
        meshObject.GetComponent<MeshCollider>().sharedMesh = mesh;
    }
}

public class MeshData {
    public Vector3[] vertices;
    public int[] triangles;
    public Vector2[] uvs;

    private int triangleIndex;

    /// <summary>
    /// Initializes a new MeshData object with the specified number of vertices per line.
    /// </summary>
    /// <param name="verticesPerLine">Gives the vertices per line for the x and y axis</param>
    public MeshData(Vector2Int verticesPerLine) {
        vertices = new Vector3[verticesPerLine.x * verticesPerLine.y];
        uvs = new Vector2[verticesPerLine.x * verticesPerLine.y];
        triangles = new int[(verticesPerLine.x - 1) * (verticesPerLine.y - 1) * 6];
    }

    /// <summary>
    /// Adds a quad to the mesh data with the specified vertex indices.
    /// </summary>
    /// <param name="A">A index to the relevant vertex for the triangle</param>
    /// <param name="B">A index to the relevant vertex for the triangle</param>
    /// <param name="C">A index to the relevant vertex for the triangle</param>
    /// <param name="D">A index to the relevant vertex for the triangle</param>
    public void AddQuad(int A, int B, int C, int D) {
        triangles[triangleIndex] = A;
        triangles[triangleIndex + 1] = B;
        triangles[triangleIndex + 2] = C;
        triangles[triangleIndex + 3] = B;
        triangles[triangleIndex + 4] = A;
        triangles[triangleIndex + 5] = D;
        triangleIndex += 6;
    }

    /// <summary>
    /// Creates a mesh from the mesh data.
    /// </summary>
    /// <returns>The new mesh initilized from the set options given through the mesh data</returns>
    public Mesh CreateMesh() {
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        return mesh;
    }
}