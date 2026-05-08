using UnityEngine;

public static class PersistentBoardSave
{
    static GameData savedGameData = null;
    static string fromLocation = "";
    public static readonly string savePath = Application.persistentDataPath + "/saves";

    public static void LoadFromFile(string fileName) {
        fromLocation = fileName;
        savedGameData = new();
        savedGameData.Load(SaveSystem.Load(fileName));
    }

    public static GameData GetGameData() {
        return savedGameData;
    }

    public static void ScrapSave() {
        savedGameData = null;
        fromLocation = "";
    }

    internal static bool HasSave() {
        return savedGameData != null;
    }
}
