using Unity.VisualScripting;
using UnityEngine;

public class ThornPlant : MonoBehaviour {
    private const string PLAYER = "Player";
    [SerializeField] private HazardSO hazardSO;
    [SerializeField] private float cooldownTimerMax = 1f;
    private float cooldownTimer;
    private void Start() {
        ResetTimer();
    }

    private void Update() {
        if (cooldownTimer > 0) {
            cooldownTimer -= Time.deltaTime;
        }
    }
    private void OnTriggerEnter(Collider other) {
        if(other.transform.CompareTag(PLAYER) && cooldownTimer <= 0){
            GameManager.Instance.SpendCalories(hazardSO.calorieDamage);
            ResetTimer();
        }
    }

    private void OnTriggerStay(Collider other) {
        if(other.transform.CompareTag(PLAYER) && cooldownTimer <= 0){
            GameManager.Instance.SpendCalories(hazardSO.calorieDamage);
            ResetTimer();
        }
    }

    private void ResetTimer() {
        cooldownTimer = cooldownTimerMax;
    }
}
