using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreUI : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI text; 
    [SerializeField] private Image calorieImage;
    [SerializeField] private Sprite[] calorieSprites;
    [SerializeField] private float changeSpeed = 1f;
    private float lastCaloriesUnRounded;
    private void Start() {
        lastCaloriesUnRounded = GameManager.Instance.GetPlayerCalories();
        text.text = $"Calories: {GameManager.Instance.GetPlayerCalories()}";
        UpdateImage();
    }

    private void Update() {
        UpdateVisual();
    }

    private void UpdateVisual() {
        lastCaloriesUnRounded = Mathf.Lerp(lastCaloriesUnRounded, GameManager.Instance.GetPlayerCalories(), Time.deltaTime * changeSpeed);
        text.text = $"Calories: {Mathf.Round(lastCaloriesUnRounded)}";
        UpdateImage();
    }
    private void UpdateImage()
    {
        if (calorieImage == null || calorieSprites.Length != 5) return;

        // Determine which image to use based on calorie ranges
        float calories = GameManager.Instance.GetPlayerCalories();
        int spriteIndex = GetCalorieSpriteIndex(calories);
        calorieImage.sprite = calorieSprites[spriteIndex];
    }
    private int GetCalorieSpriteIndex(float calories)
    {
        // Define calorie thresholds for 5 ranges
        if (calories <= 20) return 0;      // Low calories
        if (calories <= 40) return 1;      // Moderately low
        if (calories <= 60) return 2;      // Medium
        if (calories <= 80) return 3;      // Moderately high
        return 4;                          // High calories
    }
}
