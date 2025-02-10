using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;

public class PersonalBest : MonoBehaviour
{
    [Header("UI Reference")]
    public TextMeshProUGUI bestTimeText;

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        UpdatePersonalBestUI();
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        UpdatePersonalBestUI();
    }

    public void UpdatePersonalBestUI()
    {
        int currentLevel = GetCurrentLevel();
        float bestTime = HighscoreManager.LoadHighscore(currentLevel);

        bestTimeText.text = bestTime == float.MaxValue ? "Best Time: N/A" : $"Personal Best (Level {currentLevel}): {bestTime:F2}s";
    }

    private static int GetCurrentLevel()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        Match match = Regex.Match(sceneName, "\\d+");
        return match.Success ? int.Parse(match.Value) : 1;
    }
}
