using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour {
    public static float musicVolume = 0.5f;
    public static float sfxVolume = 0.5f;

    public static List<AudioSource> sfxSources = new List<AudioSource>();
    public static List<AudioSource> musicSources = new List<AudioSource>();
    
    public static void Play(AudioClip clip, bool isMusic = false) {
        AudioSource src = MakeSource(isMusic);
        src.clip = clip;
        src.Play();
        RemoveReference(isMusic, src);
        Destroy(src.gameObject, src.clip.length);
    }
    
    public static void Play(AudioClip[] clips, bool isMusic = false) {
        AudioSource src = MakeSource(isMusic);
        src.clip = clips[Random.Range(0, clips.Length)];
        src.Play();
        RemoveReference(isMusic, src);
        Destroy(src.gameObject, src.clip.length);
    }

    public static void PlayRandomPitched(AudioClip clip, float variance = 0.1f, bool isMusic = false) {
        AudioSource src = MakeSource(isMusic);
        src.clip = clip;
        src.pitch = Random.Range(1 - variance, 1 + variance);
        src.Play();
        RemoveReference(isMusic, src);
        Destroy(src.gameObject, src.clip.length);
    }
    
    public static void PlayRandomPitched(AudioClip[] clips, float variance = 0.1f, bool isMusic = false) {
        AudioSource src = MakeSource(isMusic);
        src.clip = clips[Random.Range(0, clips.Length)];
        src.pitch = Random.Range(1 - variance, 1 + variance);
        src.Play();
        RemoveReference(isMusic, src);
        Destroy(src.gameObject, src.clip.length);
    }
    
    public static void PlayAtVolume(AudioClip clip, float volume) {
        AudioSource src = MakeSource(false);
        src.clip = clip;
        src.volume = volume;
        src.Play();
        RemoveReference(false, src);
        Destroy(src.gameObject, src.clip.length);
    }

    public static void SetVolume(bool isMusic, float volume) {
        if (isMusic) {
            musicVolume = volume;
        }
        else {
            sfxVolume = volume;
        }
        foreach (AudioSource src in isMusic ? musicSources : sfxSources) {
            src.volume = volume;
        }
    }

    private static AudioSource MakeSource(bool isMusic) {
        AudioSource src = Instantiate(GameObject.Find("Audio Source").GetComponent<AudioSource>());
        if (isMusic) {
            musicSources.Add(src);
        }
        else {
            sfxSources.Add(src);
        }
        src.volume = isMusic ? musicVolume : sfxVolume;
        return src;
    }

    private static void RemoveReference(bool isMusic, AudioSource src) {
        if (isMusic) {
            musicSources.Remove(src);
        }
        else {
            sfxSources.Remove(src);
        }
    }
}
