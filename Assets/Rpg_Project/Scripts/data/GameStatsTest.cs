using UnityEngine;

public class GameStatsTest : MonoBehaviour
{
    [ContextMenu("Test GameStats")]
    private void TestGameStats()
    {
        var testStats = new GameStats
        {
            EnemiesKilled = 15,
            DamageDealt = 1250,
            DamageTaken = 320,
            GameTime = 856f,
            IsVictory = true
        };

        Debug.Log($"Created stats: {testStats}");
        Debug.Log($"Formatted time: {testStats.GetFormattedTime()}");


        IGameStatsRepository repository = new PlayerPrefsStatsRepository();

        repository.SaveStats(testStats);

        var loadedStats = repository.GetLastStats();
        if (loadedStats != null)
        {
            Debug.Log($"Loaded stats: {loadedStats}");
            Debug.Log($"Are they equal? {testStats.EnemiesKilled == loadedStats.EnemiesKilled}");
        }

        var allStats = repository.GetAllStats();
        Debug.Log($"Total stats in history: {allStats.Count}");
    }

    [ContextMenu("Clear All Stats")]
    private void ClearAllStats()
    {
        IGameStatsRepository repository = new PlayerPrefsStatsRepository();
        repository.ClearAllStats();
    }
}