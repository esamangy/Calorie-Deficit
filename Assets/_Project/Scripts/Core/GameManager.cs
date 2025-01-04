using System;
using UnityEngine;

public class GameManager : MonoBehaviour {
    [SerializeField] private int jumpCalorieCost = 5;
    [SerializeField] private float timeCalorieCost = 1f;
    public static GameManager Instance { get; private set;}
    public event EventHandler OnCaloriesChanged;
    private float playerCalories;
    private bool sprint = false;
    private bool move = false;
    private void Awake() {
        if(Instance != null) {
            Destroy(this);
        }
        Instance = this;
    }

    private void Update() {
        SpendCalories(timeCalorieCost * Time.deltaTime);
        HandleMovement();
    }

    public void AddCalories(float value) {
        playerCalories += value;
        OnCaloriesChanged?.Invoke(this, EventArgs.Empty);
    }

    public void SpendCalories(float value) {
        playerCalories -= value;
        OnCaloriesChanged?.Invoke(this, EventArgs.Empty);
    }

    public float GetPlayerCalories() {
        return playerCalories;
    }

    public void RegisterJump() {
        SpendCalories(jumpCalorieCost);
    }

    private void HandleMovement(){
        if(!move) return;
        int sprintMult = sprint ? PlayerTrerrainController.Instance.GetCurrentSprintSpeedCalorieMultiplier() : 1;
        SpendCalories(PlayerTrerrainController.Instance.GetCurrentMoveSpeedCalorieMultiplier() * Time.deltaTime * sprintMult);
    }

    public void RegisterMovement(bool move) {
        this.move = move;
    }

    public void RegisterSprint(bool sprint) {
        this.sprint = sprint;
    }
}
