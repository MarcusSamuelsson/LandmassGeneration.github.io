using System.Collections;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LandmassGenerator))]
public class LandmassGeneratorEditor : Editor {
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
