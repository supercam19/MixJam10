using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderSound : MonoBehaviour {
    private AudioClip tick;
    private float delayTime = 0.1f;
    private float lastSoundTime = 0;
    public bool isMusicSlider;
    
    void Start() {
        tick = Resources.Load<AudioClip>("Sounds/SFX/ui_tick");
    }
    
    public void OnValueChange() {
        if (Time.time - lastSoundTime < delayTime) {
            return;
        }
        float volume = GetComponent<UnityEngine.UI.Slider>().value;
        SoundPlayer.SetVolume(isMusicSlider, volume);
        SoundPlayer.Play(tick, isMusicSlider);
        lastSoundTime = Time.time;
    }
}
