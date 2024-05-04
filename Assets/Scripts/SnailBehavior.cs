using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SnailBehavior : GenericEnemy {
    private SpriteRenderer sr;
    public Sprite snailLeft;
    public Sprite snailRight;
    void Start() {
        health = 100000;
        speed = 1;
        damage = 1;
        attackDelay = 0.05f;
        attackRange = 0.2f;
        sightRange = 10;
        
        sr = GetComponent<SpriteRenderer>();
        GetComponent<NavMeshAgent>().speed = speed;
    }

    public override void TickUpdate() {
        if (GetComponent<NavMeshAgent>().velocity.x > 0) {
            sr.sprite = snailRight;
        }
        else {
            sr.sprite = snailLeft;
        }
    }
}
