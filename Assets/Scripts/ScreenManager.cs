using UnityEngine;
using TMPro;

public class ScreenManager : MonoBehaviour
{
    [Header("UI Screens")]
    public GameObject youDiedUI;
    public GameObject levelCompleteUI;

    [Header("HUD Elements")]
    public GameObject timerUI;
    public GameObject mobCounterUI;
    public GameObject playerHealthUI;

    [Header("Level Complete UI")]
    public TextMeshProUGUI levelCompleteTimeText;

    private void Awake()
    {
        if (youDiedUI != null) youDiedUI.SetActive(false);
        if (levelCompleteUI != null) levelCompleteUI.SetActive(false);
    }

    public void ShowDeathScreen()
    {
        if (youDiedUI != null)
        {
            youDiedUI.SetActive(true);
            HideHUD();
        }

        GameTimer gameTimer = FindObjectOfType<GameTimer>();
        if (gameTimer != null)
        {
            gameTimer.StopTimer();
        }
    }

    public void ShowLevelCompleteScreen()
    {
        if (levelCompleteUI != null)
        {
            levelCompleteUI.SetActive(true);
            HideHUD();
        }
    }

    private void HideHUD()
    {
        if (timerUI != null) timerUI.SetActive(false);
        if (mobCounterUI != null) mobCounterUI.SetActive(false);
        if (playerHealthUI != null) playerHealthUI.SetActive(false);
    }

    public void UpdateCompletionTime(float completionTime)
    {
        if (levelCompleteTimeText != null)
        {
            levelCompleteTimeText.text = $"Current Time: {completionTime:F2}s";
        }
    }
}