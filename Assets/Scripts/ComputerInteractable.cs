using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComputerInteractable : MonoBehaviour {
    public GameObject computerUI;
    private PlayerController player;
    public GameObject HUD;
    private bool interacting;
    public InputField input;

    void Start() {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
    }
    void Update() {
        if (Input.GetMouseButtonDown(1)) {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null) {
                if (hit.collider.gameObject == this.gameObject) {
                    computerUI.SetActive(true);
                    player.busy = true;
                    HUD.SetActive(false);
                    interacting = true;
                    input.Select();
                }
            }
        }

        if (interacting) {
            if (Input.GetKeyDown(KeyCode.Escape)) {
                CloseUI();
            }
        }
    }

    public void CloseUI() {
        computerUI.SetActive(false);
        player.busy = false;
        HUD.SetActive(true);
        interacting = false;
    }
}
