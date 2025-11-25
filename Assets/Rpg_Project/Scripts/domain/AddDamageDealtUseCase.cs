using UnityEngine;

public class AddDamageDealtUseCase
{
    private readonly IStatisticsRepository _repo;

    public AddDamageDealtUseCase(IStatisticsRepository repo)
    {
        _repo = repo;
    }

    public void Execute(int amount)
    {
        _repo.AddDamageDealt(amount);
    }
}
