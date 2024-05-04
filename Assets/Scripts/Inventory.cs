using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {
    public int[] items = new int[7];
    private int[] valueById = { 1, 4, 10, 15, 25, 50, 175 };
    public Sprite[] spriteByID = new Sprite[7];
    private GameState gameState;
    public GameObject itemPickup;
    public Image itemPickupImage;
    
    void Start() {
        gameState = GameObject.Find("GameManager").GetComponent<GameState>();
    }

    public void Add(int id, int count = 1) {
        items[id] += count;
        itemPickupImage.sprite = spriteByID[id];
        itemPickup.SetActive(true);
        Invoke(nameof(HideItemPickup), 2.0f);
    }
    
    private void HideItemPickup() {
        itemPickup.SetActive(false);
    }

    public int GetValue() {
        int value = 0;
        for (int i = 0; i < items.Length; i++) {
            value += items[i] * valueById[i];
        }

        return value;
    }

    public int SellAll() {
        int value = GetValue();
        for (int i = 0; i < items.Length; i++) {
            items[i] = 0;
        }

        gameState.balance = value;
        return value;
    }
}
