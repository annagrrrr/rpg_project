using System.Collections.Generic;

public interface IGameStatsRepository
{
    void SaveStats(GameStats stats);
    GameStats GetLastStats();
    List<GameStats> GetAllStats();
    void ClearAllStats();
}