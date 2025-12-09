using UnityEngine;
using System;

[Serializable]
public class GameStats
{
    public int EnemiesKilled;
    public int DamageDealt;
    public int DamageTaken;
    public float GameTime;
    public bool IsVictory;

    public GameStats()
    {
        EnemiesKilled = 0;
        DamageDealt = 0;
        DamageTaken = 0;
        GameTime = 0f;
        IsVictory = false;
    }

    public string GetFormattedTime()
    {
        int minutes = (int)(GameTime / 60);
        int seconds = (int)(GameTime % 60);
        return $"{minutes:00}:{seconds:00}";
    }

    public override string ToString()
    {
        return $"Stats: {EnemiesKilled} kills, {DamageDealt} dealt, {DamageTaken} taken, Time: {GetFormattedTime()}, Victory: {IsVictory}";
    }
}