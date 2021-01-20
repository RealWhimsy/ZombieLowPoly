using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManagerRework : MonoBehaviour
{
    
    // Audio players components.
    public AudioSource EffectsSource;
    public AudioSource MusicSource;

    // Random pitch adjustment range.
    public float LowPitchRange = .95f;
    public float HighPitchRange = 1.05f;
    
    public static SoundManagerRework Instance = null;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    // Play a single clip through the sound effects source.
    public void PlayEffect(AudioClip clip)
    {
        float randomPitch = Random.Range(LowPitchRange, HighPitchRange);
        EffectsSource.pitch = randomPitch;
        
        EffectsSource.clip = clip;
        EffectsSource.Play();
    }

    public void PlayEffectDelayed(AudioClip clip, float delay)
    {
        float randomPitch = Random.Range(LowPitchRange, HighPitchRange);
        EffectsSource.pitch = randomPitch;
        
        EffectsSource.clip = clip;
        EffectsSource.PlayDelayed(delay);
    }
    
    public void PlayEffectOneShot(AudioClip clip)
    {
        float randomPitch = Random.Range(LowPitchRange, HighPitchRange);
        EffectsSource.pitch = randomPitch;
        
        EffectsSource.clip = clip;
        EffectsSource.PlayOneShot(clip);
    }

    // Play a single clip through the music source.
    public void PlayMusic(AudioClip clip)
    {
        MusicSource.clip = clip;
        MusicSource.Play();
    }

    // Play a random clip from an array, and randomize the pitch slightly.
    public void RandomSoundEffect(params AudioClip[] clips)
    {
        int randomIndex = Random.Range(0, clips.Length);
        float randomPitch = Random.Range(LowPitchRange, HighPitchRange);

        EffectsSource.pitch = randomPitch;
        EffectsSource.clip = clips[randomIndex];
        EffectsSource.Play();
    }

    public bool IsEffectPlaying()
    {
        if(EffectsSource.isPlaying)
        {
            return true;
        }
        return false;
    }
}