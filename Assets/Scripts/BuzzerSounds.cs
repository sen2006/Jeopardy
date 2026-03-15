using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BuzzerSounds : MonoBehaviour {
    [SerializeField] AudioClip defaultBuzzSound;
    [SerializeField] List<SteamIDSound> steamIDBoundSounds;

    AudioSource audioSource;
    Dictionary<ulong, AudioClip> steamSounds = new();
    private void Start() {
        audioSource = GetComponent<AudioSource>();

        foreach (SteamIDSound sound in steamIDBoundSounds) {
            steamSounds.Add(sound.steamID, sound.clip);
        }
    }

    public void PlayBuzzSound(ulong SteamID) {
        if (steamSounds.ContainsKey(SteamID)) {
            audioSource.clip = steamSounds[SteamID];
            audioSource.Play();
        } else {
            audioSource.clip = defaultBuzzSound;
            audioSource.Play();
        }
    }
}

[System.Serializable]
public class SteamIDSound {
    public ulong steamID;
    public AudioClip clip;
}
