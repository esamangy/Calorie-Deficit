using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TerrainInfoSO))]
public class TerrainInfoSOEditor : Editor {
    public override void OnInspectorGUI() {
       base.OnInspectorGUI();
       if(GUILayout.Button("Generate Defaults")){
            (target as TerrainInfoSO).GenerateDefaults();
       }
    }
}
