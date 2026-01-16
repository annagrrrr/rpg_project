using System;
using UnityEngine;

[Serializable]
public class BestGameStats
{
    public int BestEnemiesKilled;
    public int BestDamageDealt;
    public int BestDamageTaken;
    public float BestGameTime;
    public int BestTotalScore;

    public bool IsInitialized;

    public void TryUpdate(GameStats stats)
    {
        if (!IsInitialized)
        {
            BestEnemiesKilled = stats.EnemiesKilled;
            BestDamageDealt = stats.DamageDealt;
            BestDamageTaken = stats.DamageTaken;
            BestGameTime = stats.GameTime;
            BestTotalScore = stats.TotalScore;
            IsInitialized = true;
            return;
        }

        BestEnemiesKilled = Mathf.Max(BestEnemiesKilled, stats.EnemiesKilled);
        BestDamageDealt = Mathf.Max(BestDamageDealt, stats.DamageDealt);
        BestTotalScore = Mathf.Max(BestTotalScore, stats.TotalScore);

        BestDamageTaken = Mathf.Min(BestDamageTaken, stats.DamageTaken);
        BestGameTime = Mathf.Min(BestGameTime, stats.GameTime);
    }
}
