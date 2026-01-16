using System.Collections.Generic;

public interface IGameStatsRepository
{
    void SaveStats(GameStats stats);
    BestGameStats GetBestStats();
    void SaveBestStats(BestGameStats stats);
    GameStats GetLastStats();
    List<GameStats> GetAllStats();
    void ClearAllStats();
}