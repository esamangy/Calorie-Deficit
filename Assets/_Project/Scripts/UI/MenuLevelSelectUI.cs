using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class MenuLevelSelectUI : MonoBehaviour {
    [SerializeField] private int desiredLevel;
    private Button button;

    private void Awake() {
        button = GetComponent<Button>();
    }
    private void Start() {
        button.onClick.AddListener(() => {
            if(desiredLevel == -1) return;
            SceneChanger.Instance.LoadLevel(desiredLevel);
        });
    }

    public void Quit() {
        Application.Quit();
    }
}
