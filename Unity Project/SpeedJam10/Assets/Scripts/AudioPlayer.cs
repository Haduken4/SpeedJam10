using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    public bool Music = false;

    float initialVolume = 0.0f;
    AudioSource source = null;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        source = GetComponent<AudioSource>();
        initialVolume = source.volume;
    }

    // Update is called once per frame
    void Update()
    {
        float mult = Music ? GlobalGameData.MusicVolume : GlobalGameData.SFXVolume;
        source.volume = initialVolume * mult;
    }

    public void PlayAudio()
    {
        if(!source.isPlaying)
        {
            source.Play();
        }
    }
}
