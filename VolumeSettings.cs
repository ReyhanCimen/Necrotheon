using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer myMixer;
    [SerializeField ] private Slider musicSlider;


    public void SetMusicVolume()
    {
        float volume = musicSlider.value;
        myMixer.SetFloat("volume", volume);

        musicSlider.value = volume;
    }

    void Start()
    {
        float currentVolume;
        if (myMixer.GetFloat("volume", out currentVolume))
        {
            musicSlider.value = currentVolume;
        }
    }

}
