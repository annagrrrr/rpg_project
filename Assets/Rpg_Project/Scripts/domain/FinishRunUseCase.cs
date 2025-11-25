using UnityEngine;

public class FinishRunUseCase
{
    private readonly IStatisticsRepository _repo;

    public FinishRunUseCase(IStatisticsRepository repo)
    {
        _repo = repo;
    }

    public RunStatistics Execute()
    {
        return _repo.Stats;
    }
}
