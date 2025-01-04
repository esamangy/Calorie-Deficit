using System;
using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI text;
    private void Start() {
        GameManager.Instance.OnScoreChanged += GameManager_OnScoreChanged;
        UpdateVisual();
    }

    private void GameManager_OnScoreChanged(object sender, EventArgs e) {
        UpdateVisual();
    }

    private void UpdateVisual() {
        text.text = $"Score: {GameManager.Instance.GetScore()}";
    }
}
