using UnityEngine;
using System.IO;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;

public class HighscoreManager : MonoBehaviour
{
    private static string filePath;
    private static HighscoreData highscoreData;

    private void Awake()
    {
        filePath = Path.Combine(Application.persistentDataPath, "highscore.json");
        highscoreData = new HighscoreData();
        LoadHighscores();
    }

    public static void SaveHighscore(int level, float newTime)
    {
        LoadHighscores();

        if (highscoreData == null || highscoreData.bestTimes == null)
        {
            highscoreData = new HighscoreData();
        }

        HighscoreEntry existingEntry = highscoreData.bestTimes.Find(entry => entry.level == level); //finds entry from a specific level

        if (existingEntry != null)
        {
            if (newTime < existingEntry.time)
            {
                existingEntry.time = newTime; //override
            }
        }
        else
        {
            highscoreData.bestTimes.Add(new HighscoreEntry { level = level, time = newTime });
        }

        string json = JsonUtility.ToJson(highscoreData, true);
        File.WriteAllText(filePath, json);
    }

    public static float LoadHighscore(int level)
    {
        LoadHighscores();
        HighscoreEntry entry = highscoreData.bestTimes.Find(e => e.level == level);
        return entry != null ? entry.time : float.MaxValue; //return entry time
    }

    private static void LoadHighscores()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath); // reads into string
            highscoreData = JsonUtility.FromJson<HighscoreData>(json);
        }

        if (highscoreData == null || highscoreData.bestTimes == null)
        {
            highscoreData = new HighscoreData(); //creates new object for corruption in json
        }
    }

    public static void ResetHighscore(int level)
    {
        LoadHighscores();
        highscoreData.bestTimes.RemoveAll(entry => entry.level == level);

        string json = JsonUtility.ToJson(highscoreData, true);
        File.WriteAllText(filePath, json);
    }
}
