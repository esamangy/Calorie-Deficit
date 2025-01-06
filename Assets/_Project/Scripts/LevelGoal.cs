using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class LevelGoal : MonoBehaviour {
    [SerializeField] private float stayTime = .5f;
    private Collider thisCollider;
    CountdownTimer countdown;
    private void Awake() {
        thisCollider = GetComponent<Collider>();
        thisCollider.isTrigger = true;
    }

    private void Start() {
        countdown = new CountdownTimer(stayTime);
        countdown.OnTimerStop += ()=> FinishLevel();
    }

    private void Update() {
        countdown.Tick(Time.deltaTime);
    }

    private void FinishLevel() {
        Dictionary<string, string> data = new Dictionary<string, string>();
        data.Add(SaveSystem.HIGHEST_CALORIES_KEY, GameManager.Instance.GetPlayerCaloriesRounded().ToString());
        SaveSystem.SaveData(GameManager.Instance.GetLevel().ToString(), data);
        SceneChanger.Instance.ReturnToMenu();
    }

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player")){
            countdown.Start();
        }
    }

    private void OnTriggerExit(Collider other) {
        countdown.Stop();
        countdown.Reset();
    }
}
