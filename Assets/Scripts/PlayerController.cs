using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float speed = 5.0f;
    private Rigidbody2D rb;
    public float stamina = 100.0f;
    public float maxStamina = 100.0f;
    public float staminaRegenRate = 0.25f;
    private float activeSpeed;
    public bool busy = false;
    private bool isMoving = false;
    public int miningPower = 1;

    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private Slider staminaBar;

    private SoundPlayer src;
    private AudioClip swingSound;
    private AudioClip[] dirtFootsteps = new AudioClip[5];
    private float nextFootstepTime;

    public TileBase dirtTile;
    public Tilemap map;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        activeSpeed = speed;

        animator.SetFloat("horizontal", 0);
        animator.SetFloat("vertical", -1);
        
        staminaBar = GameObject.Find("Stamina Bar").GetComponent<Slider>();
        if (staminaBar == null) {
            Debug.LogError("Stamina Bar not found");
        }

        swingSound = Resources.Load<AudioClip>("Sounds/SFX/tool_swing");
        for (int i = 1; i < 6; i++) {
            dirtFootsteps[i - 1] = Resources.Load<AudioClip>("Sounds/SFX/Footsteps/dirt_" + i);
        }
    }

    void Update() {
        rb.velocity = new Vector2(0, 0);
        transform.rotation = Quaternion.Euler(0, 0, 0);
        
        if (Input.GetMouseButtonDown(0)) {
            if (!busy && !isMoving) {
                SoundPlayer.Play(swingSound);
                LookTowardsMouse();
                animator.SetBool("mining", true);
                busy = true;
                Invoke(nameof(EndHit), 0.4f);
            }
        }
        else {
            animator.SetBool("mining", false);
        }
    }

    private void EndHit() {
        busy = false;
        // Raycast to find the object to hit
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if (hit.collider != null) {
            if (hit.collider.gameObject.CompareTag("Rock")) {
                hit.collider.gameObject.GetComponent<Minable>().TakeHit(miningPower);
            }
            else if (hit.collider.gameObject.CompareTag("Enemy")) {
                hit.collider.gameObject.GetComponent<GenericEnemy>().TakeDamage(miningPower);
            }
            else if (hit.collider.gameObject == this.gameObject) {
                RaycastHit2D behind = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0, 0, 1), Vector2.zero);
                if (behind.collider != null) {
                    if (behind.collider.gameObject.CompareTag("Rock")) {
                        behind.collider.gameObject.GetComponent<Minable>().TakeHit(miningPower);
                    }
                    else if (behind.collider.gameObject.CompareTag("Enemy")) {
                        behind.collider.gameObject.GetComponent<GenericEnemy>().TakeDamage(miningPower);
                    }
                }
            }
        }
        
    }
    
    void FixedUpdate() {
        if (!busy) {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            if (horizontal == 0 && vertical == 0) {
                isMoving = false;
            }
            else {
                isMoving = true;
                if (Time.time > nextFootstepTime) {
                    SoundPlayer.PlayRandomPitched(FootstepSoundsFromMaterial());
                    nextFootstepTime = Time.time + 0.5f;
                }
            }
            int lookDirection = DirectionPriority(horizontal, vertical);
            animator.SetFloat("horizontal", Mathf.Abs(horizontal));

            Vector2 movement = new Vector2(horizontal, vertical).normalized;
            rb.velocity = movement * (activeSpeed * Time.deltaTime);

            if (lookDirection == 2) {
                if (horizontal > 0) {
                    spriteRenderer.flipX = false;
                }
                else if (horizontal < 0) {
                    spriteRenderer.flipX = true;
                }
            }

            if (horizontal != 0) {
                vertical = 0;
            }

            animator.SetFloat("vertical", vertical);
            if (horizontal != 0 || vertical != 0) {
                animator.SetInteger("last_dir", lookDirection);
            }


            if (Input.GetKey(KeyCode.LeftShift) && stamina > 5) {
                activeSpeed = speed * 1.5f;
                stamina -= 0.75f;
                animator.SetFloat("animation_speed", 1.5f);
            }
            else {
                activeSpeed = speed;
                animator.SetFloat("animation_speed", 1.0f);
            }
            stamina = Mathf.Min(stamina + staminaRegenRate, maxStamina);
            staminaBar.value = (int)stamina;
        }
    }

    public void SetMaxStamina(float newMax) {
        maxStamina = newMax;
        stamina = newMax;
        staminaBar.maxValue = newMax;
        staminaBar.value = newMax;
    }

    private void LookTowardsMouse() {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (mousePos.x - transform.position.x > 0.5f) {
            spriteRenderer.flipX = false;
            animator.SetInteger("last_dir", 2);
            return;
        }

        if (mousePos.x - transform.position.x < -0.5f) {
            spriteRenderer.flipX = true;
            animator.SetInteger("last_dir", 2);
            return;
        }

        if (mousePos.y > transform.position.y) {
            animator.SetInteger("last_dir", 1);
            return;
        }
        animator.SetInteger("last_dir", 0);
        
    }

    private int DirectionPriority(float horizontal, float vertical)
    {
        if (horizontal != 0)
        {
            return 2;
        }
        if (vertical > 0)
        {
            return 1;
        }
        return 0;
    }

    private AudioClip[] FootstepSoundsFromMaterial() {
        Vector3Int playerPos = new Vector3Int((int)transform.position.x, (int)transform.position.y, 0);
        TileBase tile = map.GetTile(playerPos);
        if (tile == dirtTile) {
            return dirtFootsteps;
        }

        return dirtFootsteps;

    }
}
