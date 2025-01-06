using System;
using UnityEngine;

[CreateAssetMenu(fileName = "TerrainInfoSO", menuName = "Scriptable Objects/TerrainInfoSO")]
public class TerrainInfoSO : ScriptableObject {
    
    public enum TerrainType {
        Normal = 0,
        Muck = 1,
    }
    [Serializable]
    public struct TerrainInfo {
        public TerrainType type;
        public float moveSpeed;
        public float moveSpeedCalorieMultiplier;
        public float sprintSpeed;
        public int sprintSpeedCalorieMultiplier;
        public bool sinks;
    }
    [SerializeField] private TerrainInfo[] types;
    public void GenerateDefaults() {
        types = new TerrainInfo[Enum.GetNames(typeof(TerrainType)).Length];
        int cntr = 0;
        foreach(string value in Enum.GetNames(typeof(TerrainType))) {
            types[cntr] = new TerrainInfo{
                type = (TerrainType)Enum.Parse(typeof(TerrainType), value),
                moveSpeed = 200,
                moveSpeedCalorieMultiplier = 1,
                sprintSpeed = 300,
                sprintSpeedCalorieMultiplier = 3,
                sinks = false,
            };
            cntr ++;
        }
    }

    public static int GetNumTerrainTypes() {
        return Enum.GetNames(typeof(TerrainType)).Length;
    }

    public TerrainInfo GetTerrainInfo(TerrainType targetType) {
        for(int i = 0; i < types.Length; i ++){
            if(types[i].type == targetType){
                return types[i];
            }
        }

        return types[0];
    }
}