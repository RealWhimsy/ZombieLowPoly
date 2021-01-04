using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SoundManager
{
    public enum Sound {
        Zombies1,
        Zombies2,
        Zombies3,
        Zombies4,
        Step1,
        Step2,
        Step3,
        Step4,
        Hitmarker1,
        Hitmarker2,
        Hitmarker3,
        Hitmarker4,
        ZombieDead,
        PlayerDead,
        AmmoEmpty,
        AmmoPickup,
        WeaponSwitch
    }

    public static void PlaySound(Sound sound) {
        GameObject soundGameObject = new GameObject("Sound");
        AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
        audioSource.PlayOneShot(GetAudioClip(sound));
    }

    private static AudioClip GetAudioClip(Sound sound) {
        foreach(GameAssets.SoundAudioClip soundAudioClip in GameAssets.i.soundAudioClipArray) {
            if(soundAudioClip.sound == sound) {
                return soundAudioClip.audioClip;
            }
        }
        return null;
    }
    

}
