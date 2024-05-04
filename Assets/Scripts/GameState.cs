using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;


public class GameState : MonoBehaviour {
    [NonSerialized]
    public int day = 1;

    private Text timeText;
    private Text dayText;

    private TimeOfDay time;

    public int balance = 0;
    private Text balText;

    private OreGeneration oreGen;

    private GameObject player;
    private SpriteRenderer tirednessIndicator;

    void Start() {
        Cursor.SetCursor(Resources.Load<Texture2D>("cursor_default"), Vector2.zero, CursorMode.Auto);
        time = new TimeOfDay(8, 0, 10);
        timeText = GameObject.Find("Time Text").GetComponent<Text>();
        dayText = GameObject.Find("Day Text").GetComponent<Text>();
        time.ManageUIText(true, timeText);
        time.AddEvent(new TimedEvent(0, 0, PassOut, true));
        InvokeRepeating(nameof(TickTime), 5.0f, 5.0f);

        balText = GameObject.Find("Money Text").GetComponent<Text>();
        oreGen = GetComponent<OreGeneration>();
        player = GameObject.Find("Player");
        tirednessIndicator = GameObject.Find("Tiredness").GetComponent<SpriteRenderer>();
        time.AddEvent(new TimedEvent(22, 0, () => tirednessIndicator.enabled = true, true));

        SoundPlayer.Play(Resources.Load<AudioClip>("Sounds/Music/alone_in_space"), true);
        Invoke(nameof(PlayMusic), Resources.Load<AudioClip>("Sounds/Music/alone_in_space").length + UnityEngine.Random.Range(30, 90));
    }

    public String GetTime() {
        return time.ToString();
    }

    public void PlayMusic() {
        int rand = UnityEngine.Random.Range(0, 3);
        if (rand == 0) {
            SoundPlayer.Play(Resources.Load<AudioClip>("Sounds/Music/alone_in_space"), true);
            Invoke(nameof(PlayMusic), Resources.Load<AudioClip>("Sounds/SFX/alone_in_space").length + UnityEngine.Random.Range(30, 60));
        }
        else {
            SoundPlayer.Play(Resources.Load<AudioClip>("Sounds/SFX/cave_ambience"), true);
            Invoke(nameof(PlayMusic), Resources.Load<AudioClip>("Sounds/SFX/cave_ambience").length + UnityEngine.Random.Range(30, 60));
        }

        if (rand == 2) {
            SoundPlayer.Play(Resources.Load<AudioClip>("Sounds/SFX/looming_presence"), true);
            Invoke(nameof(PlayMusic), Resources.Load<AudioClip>("Sounds/SFX/looming_presence").length + UnityEngine.Random.Range(30, 60));
        }
    }

    public void SetTime(int hour, int min) {
        time.SetTime(hour, min);
    }

    public void SetBalance(int newBal) {
        balance = newBal;
        balText.text = "$" + balance;
    }

    public void PassOut() {
        GameObject.Find("Bed").GetComponent<BedInteraction>().Sleep();
    }

    public void OnNewDay() {
        time.SetTime(8, 0);
        day++;
        dayText.text = "Day " + day;

        if (day < 100) {
            oreGen.stoneChance -= 2;
            oreGen.mutantChance += 2;
        }
        oreGen.ClearExisting();
        oreGen.Generate();
        
        tirednessIndicator.enabled = false;
        player.transform.position = new Vector3(-0.47f, -22.44f, 0.15f);
        balance = balance / 2;
    }

    private void TickTime() {
        time.Tick();
    }
}

class TimeOfDay {
    private int hour;
    private int minute;
    private int minuteIncrement;
    public bool dayIsActive = true;
    private bool managesUI = false;
    private Text timeText;
    private List<TimedEvent> events = new List<TimedEvent>();

    public TimeOfDay(int hour, int minute, int minuteIncrement) {
        this.hour = hour;
        this.minute = minute;
        this.minuteIncrement = minuteIncrement;
    }

    public void SetTime(int hour, int minute) {
        this.hour = hour;
        this.minute = minute;
    }

    public void Tick() {
        if (dayIsActive) {
            minute += minuteIncrement;
            CheckOverflow();
            if (managesUI) {
                timeText.text = ToString();
            }
            if (events.Count > 0) {
                List<TimedEvent> toRemove = new List<TimedEvent>();
                foreach (TimedEvent te in events) {
                    if (te.TimeMatches(hour, minute)) {
                        te.Call();
                        if (!te.persistant) {
                            toRemove.Add(te);
                        }
                    }
                }
                foreach (TimedEvent te in toRemove) {
                    events.Remove(te);
                }
                toRemove.Clear();
            }
        }
    }

    private void CheckOverflow() {
        while (minute >= 60) {
            minute -= 60;
            hour++;
        }

        if (hour >= 24) {
            hour = 0;
            minute = 0;
        }
    }

    public void AddEvent(TimedEvent te) {
        events.Add(te);
    }

    public void ManageUIText(bool flag, Text timeText = null) {
        managesUI = flag;
        if (flag) {
            if (timeText == null) {
                Debug.LogError("Text object not provided in ManageUIText() call");
            }
            else {
                this.timeText = timeText;
            }
        }

    }

    public new String ToString() {
        return hour.ToString("D2") + ":" + minute.ToString("D2");
    }
}

class TimedEvent {
    private int hour;
    private int minute;
    private Action action;
    public bool persistant;

    public TimedEvent(int hour, int minute, Action action, bool persistant = false) {
        this.hour = hour;
        this.minute = minute;
        this.action = action;
        this.persistant = persistant;
    }

    public bool TimeMatches(int hour, int minute) {
        return this.hour == hour && this.minute == minute;
    }

    public void Call() {
        action();
    }
}
