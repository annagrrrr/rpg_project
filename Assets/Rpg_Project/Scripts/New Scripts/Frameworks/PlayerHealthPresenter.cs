public class PlayerHealthPresenter
{
    private readonly TakeDamageUseCase _takeDamageUseCase;
    private readonly Health _health;
    private readonly PlayerHealthView _healthView;
    private readonly StunPlayerUseCase _stunUseCase;

    public PlayerHealthPresenter(Health health, PlayerHealthView healthView, StunPlayerUseCase stunUseCase)
    {
        _health = health;
        _healthView = healthView;
        _takeDamageUseCase = new TakeDamageUseCase(health);

        _healthView.SetMaxHealth(health.Max);
        _healthView.SetCurrentHealth(health.Current);
        _stunUseCase = stunUseCase;
    }

    public void TakeDamage(int damage)
    {
        _takeDamageUseCase.Execute(damage);
        _healthView.SetCurrentHealth(_health.Current);
        _stunUseCase.Stun(0.2f);
    }
}
