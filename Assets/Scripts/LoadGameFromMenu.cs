using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

public class LoadGameFromMenu : MonoBehaviour {
    public GameObject transitionObj;
    private Image loadingScreen;
    public Text introText;
    private float alpha = 0;
    private String name = "ASTEROID ";
    private int nameLength;
    private int namePos;
    private int namePos2;

    void Start() {
        // Just start menu music, putting this here so I don't have to make another script
        SoundPlayer.Play(Resources.Load<AudioClip>("Sounds/Music/title_track"), true);
    }
    
    public void LoadGame() {
        loadingScreen = transitionObj.GetComponent<Image>();
        SoundPlayer.Play(Resources.Load<AudioClip>("Sounds/SFX/game_load"));
        transitionObj.SetActive(true);
        FadeToBlack();
    }

    public void FadeToBlack() {
        alpha += 0.01f;
        Color currentColor = loadingScreen.color;
        currentColor.a = alpha;
        loadingScreen.color = currentColor;
        if (alpha < 1) {
            Invoke(nameof(FadeToBlack), 0.02f);
        }
        else {
            introText.enabled = true;
            nameLength = UnityEngine.Random.Range(6, 10);
            SoundPlayer.Play(Resources.Load<AudioClip>("Sounds/SFX/scrambling"));
            DisplayRandomName();
        }
    }

    public void DisplayRandomName() {
        if (namePos < nameLength) {
            introText.text = name += RandomCharacter();
            namePos++;
            Invoke(nameof(DisplayRandomName), 0.5f);
            return;
        }
        else {
            String day = "\n\nDAY 1";
            if (namePos2 < 7) {
                introText.text = name += day[namePos2];
                namePos2++;
                Invoke(nameof(DisplayRandomName), 0.5f);
                return;
            }
            Invoke(nameof(StartGame), 4.0f);
        }
    }

    public void StartGame() {
        SceneManager.LoadScene("GameScene");
    }

    private char RandomCharacter() {
        int isNumeric = UnityEngine.Random.Range(0, 2);
        if (isNumeric == 1) {
            return (char)UnityEngine.Random.Range(48, 58);
        }

        return (char)UnityEngine.Random.Range(65, 90);
    }
}
