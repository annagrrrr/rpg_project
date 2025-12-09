using UnityEngine;

public class GameStatsService
{
    private GameStats _currentStats;
    private readonly IGameStatsRepository _repository;
    private float _gameStartTime;

    public GameStats CurrentStats => _currentStats;

    public GameStatsService(IGameStatsRepository repository)
    {
        _repository = repository;
        StartNewSession();
    }

    public void StartNewSession()
    {
        Debug.Log("NEW SESSION CREATED");
        _currentStats = new GameStats();
        _gameStartTime = Time.time;
    }

    public void RecordEnemyKill()
    {
        _currentStats.EnemiesKilled++;
    }

    public void RecordDamageDealt(int damage)
    {
        _currentStats.DamageDealt += damage;
    }

    public void RecordDamageTaken(int damage)
    {
        _currentStats.DamageTaken += damage;
    }

    public void CompleteGame(bool isVictory)
    {
        Debug.Log("RAW JSON BEFORE SAVE:");
        string jsonBefore = JsonUtility.ToJson(_currentStats, true);
        Debug.Log(jsonBefore);

        _repository.SaveStats(_currentStats);

        string raw = PlayerPrefs.GetString("LastGameStats", "NO KEY FOUND");
        Debug.Log("RAW JSON IN PLAYER PREFS:");
        Debug.Log(raw);

        _currentStats.GameTime = Time.time - _gameStartTime;
        _currentStats.IsVictory = isVictory;
        _repository.SaveStats(_currentStats);
        Debug.Log($"Game completed! Stats saved: {_currentStats}");

        _currentStats = null;
    }

    public bool HasActiveSession => _currentStats != null;
}