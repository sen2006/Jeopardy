using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class SaveSystem {
    public static readonly string savePath = Application.persistentDataPath + "/saves";
    public static readonly string settingsSavePath = Application.persistentDataPath + "/settings.json";
    private static readonly List<string> mediaExtensionTypes = new() {
    ".png",
    ".jpg",
    ".jpeg",
    };

    public static void Save(string saveName, GameSaveData saveData) {
        string path = savePath + "/" + saveName + "/" + saveName + ".boardSave";
        EnsureDirectoryExists(savePath+"/"+saveName);
        File.WriteAllText(path, JsonUtility.ToJson(saveData, true));
        Debug.Log("Saved To: " + path);
    }

    public static GameSaveData Load(string saveName) {
        string path = savePath + "/" + saveName + "/" + saveName + ".boardSave";
        string data = File.ReadAllText(path);
        Debug.Log("Loaded From: " + path);
        return JsonUtility.FromJson<GameSaveData>(data);
    }

    public static byte[] LoadMediaFile(string saveName, string mediaName, string mediaFileType) {
        if (!mediaExtensionTypes.Contains(mediaFileType)) throw new System.Exception("tried to load unsuported file type");
        string path = savePath + "/" + saveName + "/media/" + mediaName + mediaFileType;
        return File.ReadAllBytes(path);
    }

    public static void UploadMediaFile(string saveName, string originalPath, string mediaName) {
        string mediaFileType = originalPath.Split('.').Last();
        if (!mediaExtensionTypes.Contains(mediaFileType)) throw new System.Exception("tried to upload unsuported file type");
        string path = savePath + "/" + saveName + "/media/" + mediaName + mediaFileType;
        
        //TODO: make sure new path is not occupied
        //TODO: copy the file from original path to new path
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

    internal static bool FileExists(string name) {
        //TODO
        return true;
    }
}