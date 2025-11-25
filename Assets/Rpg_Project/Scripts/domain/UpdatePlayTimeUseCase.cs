using UnityEngine;

public class UpdatePlayTimeUseCase
{
    private readonly IStatisticsRepository _repo;

    public UpdatePlayTimeUseCase(IStatisticsRepository repo)
    {
        _repo = repo;
    }

    public void Execute(float deltaTime)
    {
        _repo.UpdatePlayTime(deltaTime);
    }
}

