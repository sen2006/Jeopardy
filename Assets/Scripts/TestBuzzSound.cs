using UnityEngine;

public class TestBuzzSound : MonoBehaviour
{
    [SerializeField] AudioClip defaultBuzzSound;
    [SerializeField] AudioSource source;
    
    public void PlayBuzzSound() {
        source.clip = defaultBuzzSound;
        source.volume = Settings.GetSettings().volume;
        source.Play();
    }
}
