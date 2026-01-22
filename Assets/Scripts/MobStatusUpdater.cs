using UnityEngine;
using TMPro;

public class MobStatusUpdater : MonoBehaviour
{
    private int totalMobs = 0;
    private int mobsKilled = 0;

    [Header("UI References")]
    public TextMeshProUGUI mobCounterText;
    public GameObject gameOverUI;

    private GameTimer gameTimer;
    private PersonalBest personalBestManager;
    private ScreenManager screenManager;

private void Start()
{
    gameTimer = FindObjectOfType<GameTimer>();
    personalBestManager = FindObjectOfType<PersonalBest>();
    screenManager = FindObjectOfType<ScreenManager>();

    // count both mobs and bosses
    int mobCount = GameObject.FindGameObjectsWithTag("Mob").Length;
    int bossCount = GameObject.FindGameObjectsWithTag("Boss").Length;
    totalMobs = mobCount + bossCount;

    UpdateMobCounter();
}


public void MobKilled()
{
    mobsKilled++;
    UpdateMobCounter();

    if (mobsKilled >= totalMobs)
    {
        if (gameTimer != null)
        {
            gameTimer.StopTimer();
            float completionTime = gameTimer.GetTimeElapsed();

            int levelIndex = LevelManager.GetCurrentLevelIndex();

            // highscore manager handles saves
            HighscoreManager.SaveHighscore(levelIndex, completionTime);

            if (screenManager != null)
            {
                screenManager.UpdateCompletionTime(completionTime);
            }
        }

        if (gameOverUI != null)
        {
            gameOverUI.SetActive(true);
        }

        // player freeze
        PlayerController player = FindObjectOfType<PlayerController>();
        if (player != null)
        {
            player.FreezePlayer(true);
        }

        
        if (screenManager != null)
        {
            screenManager.ShowLevelCompleteScreen();
        }

    }
}


    private void UpdateMobCounter()
    {
        if (mobCounterText != null)
        {
            mobCounterText.text = $"Mobs: {mobsKilled}/{totalMobs}";
        }
    }

public void ResetMobCounter()
{
    mobsKilled = 0;
    int mobCount = GameObject.FindGameObjectsWithTag("Mob").Length;
    int bossCount = GameObject.FindGameObjectsWithTag("Boss").Length;
    totalMobs = mobCount + bossCount;
    UpdateMobCounter();
}

}
