using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Leguar.TotalJSON;

public static class SaveSystem {
    private const string DATA_FILE_BASE = "Save_"; 
    public const string HIGHEST_CALORIES_KEY = "Highest Calories";
    public static void SaveData(string level, Dictionary<string, string> data) {
        string filePath = Path.Combine(Application.persistentDataPath, DATA_FILE_BASE + level + ".json");
        JSON origin = new JSON();
        foreach (KeyValuePair<string, string> keyValuePair in data){
            origin.Add(keyValuePair.Key, keyValuePair.Value);
        }

        File.WriteAllText(filePath, origin.CreatePrettyString());
    }

    public static bool LoadData(string level, out JSON data){
        string filePath = Path.Combine(Application.persistentDataPath, DATA_FILE_BASE + level + ".json");
        if(File.Exists(filePath)) {
            data = JSON.ParseString(File.ReadAllText(filePath));
            return true;
        } else {
            data = new();
            return false;
        }
    }
}
