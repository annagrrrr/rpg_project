using UnityEngine;

public class AddDamageReceivedUseCase
{
    private readonly IStatisticsRepository _repo;

    public AddDamageReceivedUseCase(IStatisticsRepository repo)
    {
        _repo = repo;
    }

    public void Execute(int amount)
    {
        _repo.AddDamageReceived(amount);
    }
}

