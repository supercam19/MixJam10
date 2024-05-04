using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class HandleUserCommands : MonoBehaviour {
    private InputField input;
    public Text output;
    public Inventory inv;
    private GameState gameState;
    private PlayerBehavior player;
    private PlayerController playerController;
    private Transform darknessOverlay;

    void Start() {
        input = GetComponent<InputField>();
        gameState = GameObject.Find("GameManager").GetComponent<GameState>();
        player = GameObject.Find("Player").GetComponent<PlayerBehavior>();
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        darknessOverlay = GameObject.Find("Darkness Overlay").GetComponent<Transform>();
    }

    public void OnSubmit() {
        String str = input.text;
        str = str.ToLower();
        input.text = "";
        input.Select();
        if (str == "help") {
            output.text = "help - list commands\n" +
                          "sell - sell inventory\n" +
                          "upgrades - view upgrades\n" + 
                          "buy [number] - buy upgrade\n" +
                          "exit - leave computer";
        }
        else if (str == "sell") {
            int money = inv.SellAll();
            output.text = "Sold all items for $" + money + "!";
        }
        else if (str == "upgrades") {
            output.text = "Upgrades:\n" +
                          "1. Health Restore   - $200\n" +
                          "2. Vision Range     - $50\n" +
                          "3. Mining Power     - $150\n" +
                          "4. Increase Health  - $100\n" +
                          "5. Increase Stamina - $100\n" +
                          "6. Stamina Regen+   - $120\n" +
                          "Buy upgrades with 'buy [number]'";
        }
        else if (str.StartsWith("buy")) {
            String[] parts = str.Split(' ');
            if (parts.Length < 2) {
                output.text = "Invalid command. Type 'help' for a list of commands.";
                return;
            }

            int upgrade = 0;
            if (!int.TryParse(parts[1], out upgrade)) {
                output.text = "Invalid syntax for buy command. Try: buy [1-6]";
                return;
            }

            if (upgrade < 1 || upgrade > 6) {
                output.text = "Not an availible upgrade. Type 'upgrades' for a list of upgrades.";
                return;
            }

            int cost = 0;
            switch (upgrade) {
                case 1:
                    cost = 200;
                    break;
                case 2:
                    cost = 50;
                    break;
                case 3:
                    cost = 150;
                    break;
                case 4:
                    cost = 100;
                    break;
                case 5:
                    cost = 100;
                    break;
                case 6:
                    cost = 120;
                    break;
            }

            if (gameState.balance >= cost) {
                gameState.SetBalance(gameState.balance - cost);;
                output.text = "Bought upgrade " + upgrade + " for $" + cost + "!";
                UpgradePlayer(upgrade);
            }
            else {
                output.text = "Not enough money to buy upgrade " + upgrade + ". Upgrade costs $" + cost + ".";
            }
        }
        else if (str == "exit") {
            GameObject.Find("Computer").GetComponent<ComputerInteractable>().CloseUI();
        }
        else if (str == "cheat.givemoney") {
            output.text = "ok fine";
            gameState.SetBalance(999999);
        }
        else if (str == "cheat.godmode") {
            output.text = "yes my lord";
            player.SetHealth(999999);
        }
        else if (str == "cheat.night") {
            gameState.SetTime(21, 50);
        }
        else if (str == "amongus") {
            output.text = "sus";
        }
        else {
            output.text = "Unknown command. Type 'help' for a list of commands.";
        }
    }
    
    private void UpgradePlayer(int upgrade) {
        SoundPlayer.Play(Resources.Load<AudioClip>("Sounds/SFX/purchase"));
        switch (upgrade) {
            case 1:
                player.SetHealth(player.maxHealth);
                break;
            case 2:
                Vector3 size = darknessOverlay.localScale;
                darknessOverlay.localScale = new Vector3(size.x + 0.5f, size.y + 0.5f, size.z + 0.5f);
                break;
            case 3:
                playerController.miningPower++;
                break;
            case 4:
                player.SetMaxHealth(player.maxHealth + 20);
                break;
            case 5:
                playerController.SetMaxStamina(playerController.stamina + 20);
                break;
            case 6:
                playerController.staminaRegenRate += 0.1f;
                break;
        }
    }
}
