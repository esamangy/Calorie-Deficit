using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlayerCalorieStagesSO))]
public class PlayerCalorieStagesSOEditor : Editor {
    public override void OnInspectorGUI() {
       base.OnInspectorGUI();
       if(GUILayout.Button("Generate Defaults")){
            (target as PlayerCalorieStagesSO).GenerateDefaults();
       }
    }
}
