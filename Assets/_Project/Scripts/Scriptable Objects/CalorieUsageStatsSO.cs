using UnityEngine;

[CreateAssetMenu(fileName = "CalorieUsageStatsSO", menuName = "Scriptable Objects/Calorie Usage Stats")]
public class CalorieUsageStatsSO : ScriptableObject{
    public int startingCalories;
    public int jumpCalorieCost;
    public int timeCalorieCost;
}
