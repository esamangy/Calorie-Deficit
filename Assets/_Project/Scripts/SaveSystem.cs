using UnityEngine;

public class SaveSystem {
    private string persistantDataPath;
    private const string DATA_FILE_BASE = "Save_"; 
    private void Awake() {
        persistantDataPath = Application.persistentDataPath;
    }

    public void SaveData(string level, string data) {

    }
}
