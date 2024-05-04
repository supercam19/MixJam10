using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SlimeBehavior : GenericEnemy {
    private SpriteRenderer sr;
    private Rigidbody2D rb;

    public Sprite slimeUp;
    public Sprite slimeDown;
    public Sprite slimeUpAttack;
    public Sprite slimeDownAttack;
    void Start() {
        health = 10;
        speed = 4f;
        damage = 6;
        attackDelay = 3f;
        attackRange = 3;
        
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        GetComponent<NavMeshAgent>().speed = speed;
    }

    public override void Attack() {
        if (playerPos.position.y > transform.position.y) {
            sr.sprite = slimeUpAttack;
        }
        else {
            sr.sprite = slimeDownAttack;
        }
        agent.isStopped = true;
        Invoke(nameof(Leap), 1.5f);
    }

    public void Leap() {
        SoundPlayer.PlayRandomPitched(Resources.Load<AudioClip>("Sounds/SFX/slime"), 0.1f);
        Vector3 direction = playerPos.position - transform.position;
        rb.AddForce(direction.normalized * 100);
        agent.isStopped = false;
        sightRange = 0;
        StartWander();
        Invoke(nameof(ReTarget), 5.0f);
        foundPlayer = false;
    }
    
    public void ReTarget() {
        sightRange = 20;
    }

    // Update sprite every tick.
    public override void TickUpdate() {
        if (GetComponent<NavMeshAgent>().velocity.y > 0) {
            sr.sprite = slimeUp;
        }
        else {
            sr.sprite = slimeDown;
        }
    }

    public void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player")) {
            GameObject.Find("Player").GetComponent<PlayerBehavior>().TakeDamage(damage);
        }
    }
}
