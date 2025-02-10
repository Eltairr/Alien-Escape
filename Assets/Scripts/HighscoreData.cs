using System.Collections.Generic;

[System.Serializable]
public class HighscoreEntry
{
    public int level;
    public float time;
}

[System.Serializable]
public class HighscoreData
{
    public List<HighscoreEntry> bestTimes = new List<HighscoreEntry>();
}
