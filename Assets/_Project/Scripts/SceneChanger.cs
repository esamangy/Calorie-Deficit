using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour {
    public static SceneChanger Instance { get; private set;}
    private const string LEVEL_STRING_BASE = "Test Level ";
    private void Awake() {
        if(Instance != null) {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    private void Start() {
        DontDestroyOnLoad(gameObject);
    }

    public void ChangeScene(string sceneName) {
        SceneManager.LoadScene(sceneName);
    }

    public void LoadLevel(int level) {
        ChangeScene(LEVEL_STRING_BASE + level.ToString());
    }

    public void RestartScene() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ReturnToMenu() {
        print("return to menu");
        SceneManager.LoadScene(0);
    }
}
