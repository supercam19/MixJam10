using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TheForgottenBehavior : GenericEnemy {
    private Animator animator;
    void Start() {
        speed = 20;
        damage = 15;
        attackDelay = 20.0f;
        attackRange = 1.2f;
        sightRange = 50;

        animator = GetComponent<Animator>();
        GetComponent<NavMeshAgent>().speed = speed;
    }

    public override void TickUpdate() {
        if (GetComponent<NavMeshAgent>().velocity.magnitude > 2f) {
            animator.SetBool("walking", true);
        }
        else {
            animator.SetBool("walking", false);
        }
    }

    public override void Attack() {
        if (lastAttackTime + attackDelay < Time.time) {
            SoundPlayer.PlayRandomPitched(Resources.Load<AudioClip>("Sounds/SFX/forgotten_screech"));
            base.Attack();
        }
        else {
            sightRange = 0;
            foundPlayer = false;
            Invoke(nameof(ReTarget), 20.0f);
        }
    }

    public void ReTarget() {
        sightRange = 50;
    }
}
