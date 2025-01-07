using System;
using UnityEngine;

public class GameManager : MonoBehaviour {
    [SerializeField] private InputReader input;
    [SerializeField] private CalorieUsageStatsSO calorieUsageStatsSO;
    [SerializeField] private int level;
    public static GameManager Instance { get; private set;}
    public event EventHandler OnCaloriesChanged;
    private float playerCalories;
    private void Awake() {
        if(Instance != null) {
            Destroy(this);
        }
        Instance = this;
    }
    private void Start() {
        AddCalories(calorieUsageStatsSO.startingCalories);
    }
    private void Update() {
        SpendCalories(calorieUsageStatsSO.timeCalorieCost * Time.deltaTime);
        HandleMovement();
    }

    public void AddCalories(float value) {
        playerCalories += value;
        OnCaloriesChanged?.Invoke(this, EventArgs.Empty);
    }

    public void SpendCalories(float value) {
        if(playerCalories < 0) return; //player is already dead
        playerCalories -= value;
        if(playerCalories < 0) {
            PlayerHUD.Instance.KillPlayer("You Starved");
        } else {
            OnCaloriesChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public float GetPlayerCalories() {
        return playerCalories;
    }

    public int GetPlayerCaloriesRounded() {
        return Mathf.RoundToInt(playerCalories);
    }

    public void RegisterJump() {
        SpendCalories(calorieUsageStatsSO.jumpCalorieCost);
    }

    private void HandleMovement(){
        if(input.Direction == Vector3.zero) return;
        int sprintMult = input.IsSprinting ? PlayerTerrainController.Instance.GetCurrentSprintSpeedCalorieMultiplier() : 1;
        SpendCalories(PlayerTerrainController.Instance.GetCurrentMoveSpeedCalorieMultiplier() * Time.deltaTime * sprintMult);
    }

    public int GetLevel() {
        return level;
    }
}
