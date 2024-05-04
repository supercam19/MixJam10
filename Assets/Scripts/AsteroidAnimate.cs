using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidAnimate : MonoBehaviour {
    private float frames = 0;
    private Vector2 pos;
    private RectTransform rt;

    private float lastSoundTime;

    void Start() {
        rt = GetComponent<RectTransform>();
        pos = rt.anchoredPosition;
    }
    
    void Update() {
        float yPos = Mathf.Sin(frames / 600);
        rt.anchoredPosition = new Vector2(pos.x, pos.y + yPos * 25);
        frames++;
    }

    public void OnClick() {
        if (Time.time - lastSoundTime > 5.0f) {
            SoundPlayer.PlayRandomPitched(Resources.Load<AudioClip>("Sounds/SFX/deep_growl"), 0.2f);
            lastSoundTime = Time.time;
        }
    }
}
