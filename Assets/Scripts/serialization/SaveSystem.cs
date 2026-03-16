using System.IO;
using UnityEngine;

public class SaveSystem {
    public static readonly string savePath = Application.persistentDataPath + "/saves";
    public static readonly string settingsSavePath = Application.persistentDataPath + "/settings.json";

    public static void Save(string saveName, GameSaveData saveData) {

        string path = savePath + "/" + saveName + ".boardSave";
        EnsureDirectoryExists(savePath);
        File.WriteAllText(path, JsonUtility.ToJson(saveData, true));
        Debug.Log("Saved To: " + path);
    }

    public static GameSaveData Load(string saveName) {
        string path = savePath + "/" + saveName + ".boardSave";
        string data = File.ReadAllText(path);
        Debug.Log("Loaded From: " + path);
        return JsonUtility.FromJson<GameSaveData>(data);
    }

    public static void SaveSettings(SettingsSave save) {

        string path = settingsSavePath;
        EnsureDirectoryExists(savePath);
        File.WriteAllText(path, JsonUtility.ToJson(save, true));
        Debug.Log("Saved To: " + path);
    }

    public static SettingsSave LoadSettings() {
        string path = settingsSavePath;
        string data = File.ReadAllText(path);
        Debug.Log("Loaded From: " + path);
        return JsonUtility.FromJson<SettingsSave>(data);
    }

    private static void EnsureDirectoryExists(string path) {
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
    }
}