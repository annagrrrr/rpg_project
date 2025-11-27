using UnityEngine;
using System.Collections.Generic;

public class PlayerPrefsStatsRepository : IGameStatsRepository
{
    private const string LAST_STATS_KEY = "LastGameStats";

    public void SaveStats(GameStats stats)
    {
        string json = JsonUtility.ToJson(stats);
        PlayerPrefs.SetString(LAST_STATS_KEY, json);
        PlayerPrefs.Save();

        Debug.Log($"Statistics saved: {stats}");

        string savedJson = PlayerPrefs.GetString(LAST_STATS_KEY, "");
        if (!string.IsNullOrEmpty(savedJson))
        {
            var savedStats = JsonUtility.FromJson<GameStats>(savedJson);
            Debug.Log($"Immediate verification - Saved: {savedStats.EnemiesKilled} kills");
        }
    }

    public GameStats GetLastStats()
    {
        if (PlayerPrefs.HasKey(LAST_STATS_KEY))
        {
            string json = PlayerPrefs.GetString(LAST_STATS_KEY);
            var stats = JsonUtility.FromJson<GameStats>(json);
            Debug.Log($"Last statistics loaded: {stats}");
            Debug.Log($"Raw JSON: {json}");
            return stats;
        }

        Debug.Log("No previous statistics found");
        return null;
    }

    public List<GameStats> GetAllStats()
    {
        var lastStats = GetLastStats();
        var allStats = new List<GameStats>();
        if (lastStats != null)
            allStats.Add(lastStats);
        return allStats;
    }

    public void ClearAllStats()
    {
        PlayerPrefs.DeleteKey(LAST_STATS_KEY);
        PlayerPrefs.Save();
        Debug.Log("Statistics cleared");
    }
}