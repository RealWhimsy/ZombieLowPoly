using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SetVolume : MonoBehaviour {

    public AudioMixer mixer;
    private Slider slider;
    [SerializeField] private string mixerTag;

    private void Start()
    {
        slider = gameObject.GetComponent<Slider>();
        SetCurrentSliderValue();
    }

    private void SetCurrentSliderValue()
    {
        switch (mixerTag)
        {
            case "MasterVol":
                slider.value = SoundManagerRework._masterVolFloat;
                SetMasterVolume(slider.value);
                break;
            
            case "EffectsVol":
                slider.value = SoundManagerRework._effectsVolFloat;
                SetEffectsVolume(slider.value);
                break;
            
            case "MusicVol":
                slider.value = SoundManagerRework._musicVolFloat;
                SetMusicVolume(slider.value);
                break;
        }
    }

    public void SetMusicVolume (float sliderValue)
    {
        mixer.SetFloat("MusicVol", Mathf.Log10(sliderValue) * 20);
        SoundManagerRework._musicVolFloat = sliderValue;
    }
    
    public void SetEffectsVolume (float sliderValue)
    {
        mixer.SetFloat("EffectsVol", Mathf.Log10(sliderValue) * 20);
        SoundManagerRework._effectsVolFloat = sliderValue;
    }
    
    public void SetMasterVolume (float sliderValue)
    {
        mixer.SetFloat("MasterVol", Mathf.Log10(sliderValue) * 20);
        SoundManagerRework._masterVolFloat = sliderValue;
    }
}