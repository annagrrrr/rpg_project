using UnityEngine;

public class PlayerHealthPresenter
{
    private readonly TakeDamageUseCase _takeDamageUseCase;
    private readonly Health _health;
    private readonly PlayerHealthView _healthView;
    private readonly StunPlayerUseCase _stunUseCase;
    private readonly IPlayerAnimationPresenter _animator;

    public bool IsDead => _health.Current <= 0;


    public PlayerHealthPresenter(Health health, PlayerHealthView healthView, StunPlayerUseCase stunUseCase, IPlayerAnimationPresenter animator)
    {
        _health = health;
        _healthView = healthView;
        _takeDamageUseCase = new TakeDamageUseCase(health);

        _healthView.SetMaxHealth(health.Max);
        _healthView.SetCurrentHealth(health.Current);
        _stunUseCase = stunUseCase;
        _animator = animator;
    }

    public void TakeDamage(int damage)
    {
        _takeDamageUseCase.Execute(damage);
        _healthView.SetCurrentHealth(_health.Current);
        _stunUseCase.Stun(0.2f);
        if (_health.Current <= 0)
        {
            OnDeath();
        }
    }

    private void OnDeath()
    {
        UnityEngine.Debug.Log("Player died.");
        _stunUseCase.Stun(float.MaxValue);
        _animator.PlayDeathAnimation();
    }
}
