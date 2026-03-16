using System;
using System.Collections.Generic;
using UnityEngine;

public class BuzzerSounds : MonoBehaviour {
    [SerializeField] AudioClip defaultBuzzSound;
    [SerializeField] List<SteamIDSound> steamIDBoundSounds;

    private Dictionary<ulong, AudioClip> steamSounds = new();
    private Dictionary<ulong, AudioSource> steamLinkedSources = new();
    private void Start() {
        foreach (SteamIDSound sound in steamIDBoundSounds) {
            steamSounds.Add(sound.steamID, sound.clip);
        }
    }

    public void PlayBuzzSound(ulong steamID) {
        AudioSource audioSource = GetOrCreateSource(steamID);
        audioSource.volume = Settings.GetSettings().volume;
        audioSource.Play();
    }

    private AudioSource GetOrCreateSource(ulong steamID) {
        if (!steamLinkedSources.ContainsKey(steamID)) {
            steamLinkedSources.Add(steamID, gameObject.AddComponent<AudioSource>());
            if (steamSounds.ContainsKey(steamID)) {
                steamLinkedSources[steamID].clip = steamSounds[steamID];
            } else {
                steamLinkedSources[steamID].clip = defaultBuzzSound;
            }
        }
        return steamLinkedSources[steamID];
    }
}

[Serializable]
public class SteamIDSound {
    public ulong steamID;
    public AudioClip clip;
}
