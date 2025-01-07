using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour {
    public static PlayerHUD Instance { get; private set;}
    [SerializeField] private TextMeshProUGUI text; 
    [SerializeField] private Image calorieImage;
    [SerializeField] private PlayerCalorieStagesSO playerCalorieStagesSO;
    [SerializeField] private Image progressUI;
    [SerializeField] private GameObject vignette;
    [SerializeField] private float changeSpeed = 1f;
    [SerializeField] private GameObject deathScreen;
    [SerializeField] private TextMeshProUGUI deathMessage;
    [SerializeField] private Button retryButton;
    [SerializeField] private Button menuButton;
    private float lastCaloriesUnrounded;
    private bool isPlayerHolding = false;
    private PlayerInteraction.Handedness holdingHandedness = PlayerInteraction.Handedness.None;
    private void Awake() {
        if(Instance != null) {
            Destroy(this);
            return;
        }
        Instance = this;
    }
    private void Start() {
        PlayerInteraction.Instance.OnHoldStarted += OnHoldStarted;
        PlayerInteraction.Instance.OnHoldStopped += OnHoldStopped;
        lastCaloriesUnrounded = GameManager.Instance.GetPlayerCalories();
        text.text = $"Calories: {GameManager.Instance.GetPlayerCalories()}";
        vignette.SetActive(false);
    }
    private void Update() {
        UpdateVisual();
    }

    private void UpdateVisual() {
        lastCaloriesUnrounded = Mathf.Lerp(lastCaloriesUnrounded, GameManager.Instance.GetPlayerCalories(), Time.deltaTime * changeSpeed);
        text.text = $"Calories: {Mathf.Round(lastCaloriesUnrounded)}";
        calorieImage.sprite = playerCalorieStagesSO.GetHungerInfoFromCalorieCount((int)lastCaloriesUnrounded).sprite;

        if(isPlayerHolding) {
            float startTime = holdingHandedness == PlayerInteraction.Handedness.Right ? PlayerInteraction.Instance.interactAltStartTime : PlayerInteraction.Instance.interactStartTime;
            float value = (Time.time - startTime) / PlayerInteraction.HOLD_TIME;
            progressUI.fillAmount = value;
        } else {
            progressUI.fillAmount = 0;
        }
    }
    private void OnHoldStopped(PlayerInteraction.Handedness arg0) {
        if(holdingHandedness != arg0){
            return;
        }
        holdingHandedness = PlayerInteraction.Handedness.None;
        isPlayerHolding = false;
    }

    private void OnHoldStarted(PlayerInteraction.Handedness arg0) {
        if(holdingHandedness == PlayerInteraction.Handedness.None) {
            holdingHandedness = arg0;
            isPlayerHolding = true;
        }
    }

    public void ToggleVignette(bool onOff) {
        vignette.SetActive(onOff);
    }

    public void KillPlayer(string message) {
        if(deathScreen.activeSelf) return;
        deathScreen.SetActive(true);
        deathMessage.text = message;
        PlayerController.Instance.DisablePlayer();
        Cursor.lockState = CursorLockMode.Confined;
        retryButton.onClick.AddListener(() => {
            retryButton.onClick.RemoveAllListeners();
            SceneChanger.Instance.RestartScene();
        });
        menuButton.onClick.AddListener(() => {
            retryButton.onClick.RemoveAllListeners();
            SceneChanger.Instance.ReturnToMenu();
        });
    }

}
