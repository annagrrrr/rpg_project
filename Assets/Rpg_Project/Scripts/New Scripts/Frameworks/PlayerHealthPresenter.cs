public class PlayerHealthPresenter
{
    private readonly TakeDamageUseCase _takeDamageUseCase;
    private readonly Health _health;
    private readonly PlayerHealthView _healthView;

    public PlayerHealthPresenter(Health health, PlayerHealthView healthView)
    {
        _health = health;
        _healthView = healthView;
        _takeDamageUseCase = new TakeDamageUseCase(health);

        _healthView.SetMaxHealth(health.Max);
        _healthView.SetCurrentHealth(health.Current);
    }

    public void TakeDamage(int damage)
    {
        _takeDamageUseCase.Execute(damage);
        _healthView.SetCurrentHealth(_health.Current);
    }
}
