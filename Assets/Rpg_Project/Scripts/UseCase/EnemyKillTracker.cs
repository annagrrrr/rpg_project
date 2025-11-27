using System;
using UnityEngine;

public class EnemyKillTracker : MonoBehaviour
{
    public event Action OnEnemyKilled;
    public event Action OnThreeEnemiesKilled;
    public event Action OnFiveEnemiesKilled;

    private int killCount = 0;

    public void RegisterKill()
    {
        killCount++;
        ScoreManager.Instance.AddScore(1);
        Debug.Log($"Kills: {killCount}");

        OnEnemyKilled?.Invoke();

        if (killCount == 3)
            OnThreeEnemiesKilled?.Invoke();

        if (killCount == 5)
            OnFiveEnemiesKilled?.Invoke();
    }
}
