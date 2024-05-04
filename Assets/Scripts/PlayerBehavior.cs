using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBehavior : MonoBehaviour {
    [DoNotSerialize]
    public int health = 100;

    [DoNotSerialize] public int maxHealth = 100;

    private Slider healthBar;
    private BedInteraction bed;
    public GameObject HUD;

    void Start() {
        healthBar = GameObject.Find("Health Bar").GetComponent<Slider>();
        bed = GameObject.Find("Bed").GetComponent<BedInteraction>();
    }
    
    public void TakeDamage(int damage) {
        health -= damage;
        if (health <= 0) {
            Die();
        }
        else {
            SoundPlayer.Play(Resources.Load<AudioClip>("Sounds/SFX/hurt"));
        }
        healthBar.value = health;
    }

    public void SetHealth(int newHealth) {
        health = newHealth;
        healthBar.value = health;
        Debug.Log("Set health to " + health);
    }

    public void SetMaxHealth(int newMax) {
        int diff = newMax - maxHealth;
        maxHealth = newMax;
        healthBar.maxValue = maxHealth;
        SetHealth(health + diff);
        Debug.Log("Max health set to " + maxHealth);
    }

    void Die() {
        GameState gs = GameObject.Find("GameManager").GetComponent<GameState>();
        gs.SetBalance(0);
        HUD.SetActive(false);
        SetHealth(maxHealth);
        bed.Sleep();
    }
}
