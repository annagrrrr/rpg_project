using UnityEngine;

public interface IStatisticsRepository
{
    RunStatistics Stats { get; }

    void AddKill();
    void AddDamageDealt(int amount);
    void AddDamageReceived(int amount);
    void UpdatePlayTime(float deltaTime);

    void Reset();
}
