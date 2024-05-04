using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class WeepingSpiritBehavior : GenericEnemy
{
    private SpriteRenderer sr;
    private float frames = 0;
    void Start() {
        health = 1;
        speed = 3;
        damage = 10;
        attackDelay = 1.0f;
        attackRange = 0.5f;
        sightRange = 15;
        useNavMesh = false;
        
        sr = GetComponent<SpriteRenderer>();
    }
    
    public override void OnUpdate() {
        frames++;
        Vector2 movement = transform.position;
        if (playerPos == null) {
            playerPos = GameObject.Find("Player").transform;
        }
        if (Vector2.Distance(playerPos.position, transform.position) <= sightRange) {
            movement = Vector2.MoveTowards(transform.position, playerPos.position, 0.005f);
            
        }
        movement.y += Mathf.Sin(frames / 400) / 300;
        transform.position = movement;
        if (movement.x - playerPos.position.x < 0) {
            sr.flipX = false;
        }
        else {
            sr.flipX = true;
        }
    }

    public override void Attack() {
        if (lastAttackTime + attackDelay <= Time.time) {
            base.Attack();
            Vector3 teleportPosition = new Vector3(Random.Range(-1, 1), Random.Range(-15, -15), 0);
            transform.position += teleportPosition;
        }
    }
}
