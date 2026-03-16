using System;
using UnityEngine;

public class Settings : MonoBehaviour {
    private static bool loaded = false;
    [SerializeField] private static SettingsSave settings;

    public static SettingsSave GetSettings() {
        if (!loaded) {
            Load();
        }
        return settings;
    }


    public void SetVolume(float v) {
        settings.volume = v;
    }

    public static void Save() {
        SaveSystem.SaveSettings(settings);
    }

    public static void Load() {
        try {
            settings = SaveSystem.LoadSettings();
            loaded = true;
        } catch {
            CreateNewSettings();
            loaded = true;
        }
    }

    private static void CreateNewSettings() {
        settings = new SettingsSave();
        settings.volume = 0.5f;
    }
}

[Serializable]
public struct SettingsSave {
    public float volume;
}