using UnityEngine;
using UnityEngine.UI;
using System;
using Sirenix.OdinInspector;

public class PersistentTimer : MonoBehaviour
{
    public Text timerText; // Optional, for displaying the timer on UI
    public string timerKey = "GameTimer"; // PlayerPrefs key
    public string startTimeKey = "StartTime"; // PlayerPrefs key for start time
    public float timerDuration; // Total time in seconds to count down
    private bool isTimerRunning = false;

    private void Start()
    {
        LoadTimer();
    }

    private void Update()
    {
        if (isTimerRunning)
        {
            UpdateTimerDisplay();
        }
    }

    [Button]
    public void StartTimer(int durationInSeconds)
    {
        timerDuration = durationInSeconds;
        PlayerPrefs.SetFloat(timerKey, timerDuration);
        PlayerPrefs.SetString(startTimeKey, DateTime.Now.ToString());
        PlayerPrefs.Save();
        isTimerRunning = true;
    }

    private void LoadTimer()
    {
        if (PlayerPrefs.HasKey(timerKey) && PlayerPrefs.HasKey(startTimeKey))
        {
            timerDuration = PlayerPrefs.GetFloat(timerKey);
            DateTime startTime = DateTime.Parse(PlayerPrefs.GetString(startTimeKey));
            float elapsed = (float)(DateTime.Now - startTime).TotalSeconds;
            timerDuration -= elapsed;

            if (timerDuration > 0)
            {
                isTimerRunning = true;
            }
            else
            {
                timerDuration = 0;
                timerText.text = "You are ready to spin!";
                isTimerRunning = false;
                WheelManager.isTimerComplete = true;
                Debug.Log("Timer finished!");
            }
        }
    }

    private void UpdateTimerDisplay()
    {
        if (timerDuration > 0)
        {
            timerDuration -= Time.deltaTime;
            TimeSpan timeSpan = TimeSpan.FromSeconds(timerDuration);

            if (timerText != null)
            {
                timerText.text = timeSpan.ToString(@"hh\:mm\:ss");
            }

            PlayerPrefs.SetFloat(timerKey, timerDuration);
            PlayerPrefs.Save();
        }
        else
        {
            timerDuration = 0;
            timerText.text = "You are ready to spin!";
            isTimerRunning = false;
            WheelManager.isTimerComplete = true;
            Debug.Log("Timer finished!");
        }
    }

    public void StopTimer()
    {
        isTimerRunning = false;
    }
}
