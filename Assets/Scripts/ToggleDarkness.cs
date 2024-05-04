using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleDarkness : MonoBehaviour {
    public SpriteRenderer darkness;
    public void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player")) {
            darkness.enabled = !darkness.enabled;
        }
    }
}
