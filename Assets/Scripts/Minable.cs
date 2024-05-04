using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Minable : MonoBehaviour {
    public int health = 1;
    public int dropID = 0;
    private Inventory inventory;

    private AudioClip rockHitSound;
    private AudioClip rockBreakSound;

    void Start() {
        inventory = GameObject.Find("Player").GetComponent<Inventory>();
        rockHitSound = Resources.Load<AudioClip>("Sounds/SFX/rock_hit");
        rockBreakSound = Resources.Load<AudioClip>("Sounds/SFX/rock_destroy");
    }
    
    public void TakeHit(int damage) {
        health -= damage;
        if (health <= 0) {
            Die();
        }
        else {
            SoundPlayer.PlayRandomPitched(rockHitSound);
        }
    }

    public void Die() {
        SoundPlayer.Play(rockBreakSound);
        inventory.Add(dropID);
        Cursor.SetCursor(Resources.Load<Texture2D>("cursor_interactable"), Vector2.zero, CursorMode.Auto);
        Destroy(gameObject);
    }
}
