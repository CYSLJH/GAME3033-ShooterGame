using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;


public class TimerSystem : MonoBehaviour
{
    public TextMeshProUGUI textDisplay;
    public float gameTime;
    private float timer;
    private bool stopTimer;

    private int minutes = 0;
    private int seconds = 0;
    private string textTime = "";

    private void Start()
    {
        textDisplay = GetComponent<TextMeshProUGUI>();
        stopTimer = false;
        timer = gameTime;
    }

    private void Update()
    {
        UpdateTimer();
    }

    private void UpdateTimer()
    {
        timer -= Time.deltaTime;
        minutes = Mathf.FloorToInt(timer / 60);
        seconds = Mathf.FloorToInt(timer - minutes * 60f);
        textTime = string.Format("{0:00}:{1:00}", minutes, seconds);
        if (timer <= 0)
        {
            stopTimer = true;
            timer = 0;
            GameManager.Instance.GetComponent<AudioSource>().Stop();

            // Load Win Screen from here
            SceneLoadManager.Instance.LoadWinScreen();
        }
        if (!stopTimer)
        {
            textDisplay.SetText(textTime);
        }
    }
}
