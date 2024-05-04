using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeLevels : MonoBehaviour {
    private float musicVolume = 0.5f;
    private float sfxVolume = 0.5f;

    public float GetVolume(bool isMusic) {
        return isMusic ? musicVolume : sfxVolume;
    }
}
