using System;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager Instance { get; private set;}
    public event EventHandler OnScoreChanged;
    private int score;
    private void Awake() {
        if(Instance != null) {
            Destroy(this);
        }
        Instance = this;
    }

    public void AddScore(int value) {
        score += value;
        OnScoreChanged?.Invoke(this, EventArgs.Empty);
    }

    public int GetScore() {
        return score;
    }
}
