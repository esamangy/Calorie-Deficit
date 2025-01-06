using System.Collections.Generic;
using Leguar.TotalJSON;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(TextMeshProUGUI))]
public class MenuLevelStatsUI : MonoBehaviour {
    [SerializeField] private int levelNumber;
    private TextMeshProUGUI textbox;

    private void Awake() {
        textbox = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable() {
        if(SaveSystem.LoadData(levelNumber.ToString(), out JSON data) && data.ContainsKey(SaveSystem.HIGHEST_CALORIES_KEY)){
            string goodText = data.GetString(SaveSystem.HIGHEST_CALORIES_KEY);
            textbox.text = SaveSystem.HIGHEST_CALORIES_KEY + ":\n" + goodText;
        } else {
            gameObject.SetActive(false);
        }
    }
}
