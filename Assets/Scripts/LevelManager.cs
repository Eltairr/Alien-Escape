using UnityEngine;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;

public static class LevelManager
{
    public static int GetCurrentLevelIndex()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        
        // Extract level number from scene name 
        Match match = Regex.Match(sceneName, "\\d+"); //scene name, first digit
        if (match.Success)
        {
            return int.Parse(match.Value); //parse string to integer
        }

        return 1; // Default to level 1 if the extraction fails
    }
}