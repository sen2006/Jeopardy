using System.IO;
using UnityEngine;

public static class PersistentBoardSave
{
    static GameData savedGameData = null;
    public static readonly string savePath = Application.persistentDataPath + "/saves";

    public static void LoadFromFile(string fileName) {
        string path = savePath + "/" + fileName + ".gameBoard";

        Packet packet = new Packet(File.ReadAllBytes(path));
        savedGameData = new GameData();
        savedGameData.Deserialize(packet);
        Debug.Log("Loaded From: " + savePath + "/" + fileName + ".gameBoard");
    }

    public static GameData GetGameData() {
        return savedGameData;
    }

    public static void ScrapSave() {
        savedGameData = null;
    }

    internal static bool HasSave() {
        return savedGameData != null;
    }
}
