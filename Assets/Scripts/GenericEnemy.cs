using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class GenericEnemy : MonoBehaviour {
    public int health;
    public float speed;
    public int damage;
    public float attackDelay;
    public float attackRange;
    public int sightRange;
    public bool useNavMesh = true;

    [NonSerialized]
    public Transform playerPos;
    [NonSerialized]
    public PlayerBehavior player;
    [NonSerialized]
    public bool foundPlayer;
    private const int tickDelay = 20;
    private int tickCounter;
    [NonSerialized]
    public NavMeshAgent agent;
    
    [NonSerialized]
    public float lastAttackTime;
    
    public GenericEnemy(int health = 25, float speed = 100f, int damage = 5, float attackDelay = 1.0f, float attackRange = 1.0f, int sightRange = 20, bool useNavMesh = true) {
        this.health = health;
        this.speed = speed;
        this.damage = damage;
        this.attackDelay = attackDelay;
        this.attackRange = attackRange;
        this.sightRange = sightRange;
        this.useNavMesh = useNavMesh;
    }

    void Start() {
        GameObject playerObj = GameObject.Find("Player");
        player = playerObj.GetComponent<PlayerBehavior>();
        playerPos = playerObj.GetComponent<Transform>();

        if (useNavMesh) {
            agent = GetComponent<NavMeshAgent>();
            agent.speed = speed;
        }
    }

    void Update() {
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        transform.rotation = Quaternion.identity;
        if (tickCounter >= tickDelay) {
            Tick();
            tickCounter = 0;
        }
        else {
            tickCounter++;
        }

        OnUpdate();
    }

    public virtual void OnUpdate() {
        
    }
    
    void Tick() {
        if (useNavMesh) {
            if (playerPos == null) {
                playerPos = GameObject.Find("Player").GetComponent<Transform>();
            }

            if (!foundPlayer) {
                if (Chance(20)) {
                    StartWander();
                }

                if (Vector2.Distance(transform.position, playerPos.position) <= sightRange) {
                    foundPlayer = true;
                }
                else {
                    foundPlayer = false;
                }
            }
            else {
                GetComponent<NavMeshAgent>().SetDestination(GameObject.Find("Player").transform.position);
                if (Vector2.Distance(transform.position, playerPos.position) <= attackRange) {
                    Attack();
                }
            }
        }
        else {
            if (playerPos == null) {
                playerPos = GameObject.Find("Player").GetComponent<Transform>();
            }
            if (Vector2.Distance(transform.position, playerPos.position) <= attackRange) {
                Attack();
            }
        }

        TickUpdate();
    }

    public virtual void Attack() {
        if (lastAttackTime + attackDelay <= Time.time) {
            GameObject.Find("Player").GetComponent<PlayerBehavior>().TakeDamage(damage);
            lastAttackTime = Time.time;
        }
    }

    public void TakeDamage(int damageTaken) {
        health -= damageTaken;
        if (health <= 0) {
            Die();
        }
        else {
            SoundPlayer.PlayRandomPitched(Resources.Load<AudioClip>("Sounds/SFX/enemy_hurt"), 0.1f);
        }
    }

    public virtual void TickUpdate() {
        
    }

    private void Die() {
        SoundPlayer.PlayRandomPitched(Resources.Load<AudioClip>("Sounds/SFX/death"), 0.1f);
        Destroy(this.gameObject);
    }

    public void StartWander() {
        if (agent == null) {
            agent = GetComponent<NavMeshAgent>();
        }
        Vector2 destination = new Vector2(transform.position.x + Random.Range(-8, 8), transform.position.y + Random.Range(-8, 8));
        agent.SetDestination(destination);
    }

    private bool Chance(int percent) {
        return Random.Range(1, 101) <= percent;
    }
    
    
}
