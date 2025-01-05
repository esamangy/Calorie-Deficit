using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI text; 
    [SerializeField] private Image calorieImage;
    [SerializeField] private PlayerCalorieStagesSO playerCalorieStagesSO;
    [SerializeField] private float changeSpeed = 1f;
    private float lastCaloriesUnrounded;
    private void Start() {
        lastCaloriesUnrounded = GameManager.Instance.GetPlayerCalories();
        text.text = $"Calories: {GameManager.Instance.GetPlayerCalories()}";
    }

    private void Update() {
        UpdateVisual();
    }

    private void UpdateVisual() {
        lastCaloriesUnrounded = Mathf.Lerp(lastCaloriesUnrounded, GameManager.Instance.GetPlayerCalories(), Time.deltaTime * changeSpeed);
        text.text = $"Calories: {Mathf.Round(lastCaloriesUnrounded)}";
        calorieImage.sprite = playerCalorieStagesSO.GetHungerInfoFromCalorieCount((int)lastCaloriesUnrounded).sprite;
    }
}
