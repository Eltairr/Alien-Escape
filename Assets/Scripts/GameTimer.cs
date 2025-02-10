using UnityEngine;
using TMPro;

public class GameTimer : MonoBehaviour
{
    private float timeElapsed = 0f;
    private bool isRunning = true;

    [Header("Timer HUD")]
    public TextMeshProUGUI timerText;

    private void Update()
    {
        if (!isRunning) return;

        timeElapsed += Time.deltaTime;
        UpdateTimerUI();
    }

    private void UpdateTimerUI()
    {
        if (timerText != null)
        {
            timerText.text = $"Time: {timeElapsed:F2}s";
        }
    }

    public void StopTimer()
    {
        isRunning = false;

        int currentLevel = LevelManager.GetCurrentLevelIndex(); 
        HighscoreManager.SaveHighscore(currentLevel, timeElapsed);
    }

    public void RestartTimer()
    {
        timeElapsed = 0f;
        isRunning = true;
        UpdateTimerUI();
    }

    public float GetTimeElapsed()
    {
        return timeElapsed;
    }
}
