using UnityEngine;
public static class ScoreCalculator
{
    private const int KILL_SCORE = 100;
    private const int DAMAGE_DEALT_DIVIDER = 5;
    private const int DAMAGE_TAKEN_DIVIDER = 3;
    private const float BASE_TIME_SECONDS = 180f;

    public static int Calculate(GameStats stats)
    {
        int killsScore = stats.EnemiesKilled * KILL_SCORE;
        int damageDealtScore = stats.DamageDealt / DAMAGE_DEALT_DIVIDER;
        int damageTakenPenalty = stats.DamageTaken / DAMAGE_TAKEN_DIVIDER;

        int timePenalty = 0;
        if (stats.GameTime > BASE_TIME_SECONDS)
        {
            timePenalty = Mathf.RoundToInt(stats.GameTime - BASE_TIME_SECONDS);
        }

        int total =
            killsScore +
            damageDealtScore -
            damageTakenPenalty -
            timePenalty;

        return Mathf.Max(0, total);
    }
}
