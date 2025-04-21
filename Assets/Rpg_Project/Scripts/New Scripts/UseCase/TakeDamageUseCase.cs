using System.Diagnostics;

public class TakeDamageUseCase
{
    private readonly Health _health;

    public TakeDamageUseCase(Health health)
    {
        _health = health;
    }

    public void Execute(int damage)
    {
        if (_health != null)
        {
            _health.TakeDamage(damage);
            UnityEngine.Debug.Log($"player took {damage} damage. cur HP: {_health.Current}");
        }
        else
        {
            UnityEngine.Debug.Log("no");
        }
    }
}
