using System;
using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private float changeSpeed = 1f;
    private float lastCaloriesUnRounded;
    private void Start() {
        lastCaloriesUnRounded = GameManager.Instance.GetPlayerCalories();
        text.text = $"Calories: {GameManager.Instance.GetPlayerCalories()}";
    }

    private void Update() {
        UpdateVisual();
    }

    private void UpdateVisual() {
        lastCaloriesUnRounded = Mathf.Lerp(lastCaloriesUnRounded, GameManager.Instance.GetPlayerCalories(), Time.deltaTime * changeSpeed);
        text.text = $"Calories: {Mathf.Round(lastCaloriesUnRounded)}";
    }
}
