using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour {
    public Transform destination;
    public Color bgColor;
    private Camera cam;
    public bool setDarkness;
    private SpriteRenderer darkness;
    void Start()
    {
        cam = Camera.main;
        darkness = GameObject.Find("Darkness Overlay").GetComponent<SpriteRenderer>();
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player")) {
            other.gameObject.transform.position = destination.position;
            cam.backgroundColor = bgColor;
            darkness.enabled = setDarkness;
        }
    }
}
