using System.Collections;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LandmassGenerator))]
public class LandmassGeneratorEditor : Editor {
    /// <summary>
    /// Draws the inspector for the LandmassGenerator.
    /// </summary>
    public override void OnInspectorGUI() {
        LandmassGenerator landmassGenerator = (LandmassGenerator)target;

        if(DrawDefaultInspector()) {
            if(landmassGenerator.autoUpdate) {
                landmassGenerator.GenerateMap();
            }
        }

        if(GUILayout.Button("Generate")) {
            landmassGenerator.GenerateMap();
        }
    }
}
