using UnityEngine;

public class InMemoryStatisticsRepository : IStatisticsRepository
{
    public RunStatistics Stats { get; private set; }

    public InMemoryStatisticsRepository()
    {
        Stats = new RunStatistics();
    }

    public void AddKill()
    {
        Stats.Kills++;
    }

    public void AddDamageDealt(int amount)
    {
        Stats.DamageDealt += amount;
    }

    public void AddDamageReceived(int amount)
    {
        Stats.DamageReceived += amount;
    }

    public void UpdatePlayTime(float deltaTime)
    {
        Stats.PlayTime += deltaTime;
    }

    public void Reset()
    {
        Stats = new RunStatistics();
    }
}

