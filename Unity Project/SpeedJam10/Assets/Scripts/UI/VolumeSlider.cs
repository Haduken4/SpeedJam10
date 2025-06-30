using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class VolumeSlider : MonoBehaviour
{
    public Slider MusicSlider = null;
    public Slider SFXSlider = null;

    private void Start()
    {
        MusicSlider.value = GlobalGameData.MusicVolume;
        SFXSlider.value = GlobalGameData.SFXVolume;
    }

    public void OnMusicSliderValueChanged()
    {
        GlobalGameData.MusicVolume = MusicSlider.value;
    }

    public void OnSFXSliderValueChanged()
    {
        GlobalGameData.SFXVolume = SFXSlider.value;
    }
}
