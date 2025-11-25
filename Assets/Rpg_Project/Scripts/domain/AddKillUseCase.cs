using UnityEngine;

public class AddKillUseCase
{
    private readonly IStatisticsRepository _repo;

    public AddKillUseCase(IStatisticsRepository repo)
    {
        _repo = repo;
    }

    public void Execute()
    {
        _repo.AddKill();
    }
}
