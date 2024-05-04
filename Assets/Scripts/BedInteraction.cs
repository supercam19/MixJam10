using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BedInteraction : MonoBehaviour {
    private float alpha = 0;
    public GameObject sleepTransition;
    private Image loadingScreen;
    public Text dayText;
    private GameState gameState;
    public GameObject bedUI;
    private PlayerController player;
    public GameObject HUD;
    
    // Start is called before the first frame update
    void Start() {
        loadingScreen = sleepTransition.GetComponent<Image>();
        gameState = GameObject.Find("GameManager").GetComponent<GameState>();
        player = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1)) {
            // Raycast to ensure player is clicking on bed
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null) {
                if (hit.collider.gameObject == this.gameObject) {
                    bedUI.SetActive(true);
                    player.busy = true;
                    HUD.SetActive(false);
                }
            }
        }
    }

    public void Sleep() {
        sleepTransition.SetActive(true);
        player.busy = true;
        bedUI.SetActive(false);
        FadeToBlack();
    }

    public void CancelUI() {
        bedUI.SetActive(false);
        player.busy = false;
        HUD.SetActive(true);
    }

    private void FadeToBlack() {
        alpha += 0.01f;
        Color currentColor = loadingScreen.color;
        currentColor.a = alpha;
        loadingScreen.color = currentColor;
        if (alpha < 1) {
            Invoke(nameof(FadeToBlack), 0.02f);
        }
        else {
            dayText.enabled = true;
            dayText.text = "DAY " + (gameState.day + 1);
            Invoke(nameof(NewDay), 4.0f);
        }
    }

    private void NewDay() {
        loadingScreen.color = new Color(0, 0, 0, 0);
        alpha = 0;
        player.gameObject.transform.position = new Vector3(-0.47f, -22.44f, 0.15f);
        dayText.enabled = false;
        HUD.SetActive(true);
        gameState.OnNewDay();
        sleepTransition.SetActive(false);
        player.busy = false;
        GameObject.Find("Darkness Overlay").GetComponent<SpriteRenderer>().enabled = false;
    }
}
