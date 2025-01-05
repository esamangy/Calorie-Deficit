using System;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerCalorieStages", menuName = "Scriptable Objects/PlayerCalorieStagesSO")]
public class PlayerCalorieStagesSO : ScriptableObject {
    public enum HungerStage {
        Starving,
        Hungry,
        Peckish,
        Normal,
        Fed,
        Well_Fed,
        OverFed,
    }
    [Serializable]
    public struct HungerInfo {
        public HungerStage stage;
        public int numCalories;
        public Sprite sprite;
    }
    [SerializeField] private HungerInfo[] stages;

    private void Awake() {
        GenerateDefaults();
    }

    private void GenerateDefaults() {
        stages = new HungerInfo[Enum.GetNames(typeof(HungerStage)).Length];
        int cntr = 0;
        foreach(string value in Enum.GetNames(typeof(HungerStage))) {
            stages[cntr] = new HungerInfo{
                stage = (HungerStage)Enum.Parse(typeof(HungerStage), value),
                numCalories = 0,
            };
            cntr ++;
        }
    }

    public static int GetNumHungerStages() {
        return Enum.GetNames(typeof(HungerStage)).Length;
    }

    public HungerInfo GetHungerInfoFromStage(HungerStage targetType) {
        for(int i = 0; i < stages.Length; i ++){
            if(stages[i].stage == targetType){
                return stages[i];
            }
        }

        return stages[0];
    }

    public HungerInfo GetHungerInfoFromCalorieCount(int count) {
        if(count < stages[0].numCalories) {
            return stages[0];
        }

        for(int i = 1; i < stages.Length - 1; i ++){
            if(count < stages[i].numCalories){
                return stages[i];
            }
        }

        return stages[stages.Length - 1];
    }
}
