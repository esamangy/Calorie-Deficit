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
        button.onClick.AddListener(() => SceneChanger.Instance.LoadLevel(desiredLevel));
    }
}
