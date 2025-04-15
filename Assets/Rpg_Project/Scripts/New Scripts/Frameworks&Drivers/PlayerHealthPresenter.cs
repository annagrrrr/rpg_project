public class PlayerHealthPresenter
{
    private readonly TakeDamageUseCase _takeDamageUseCase;

    public PlayerHealthPresenter(Health health)
    {
        _takeDamageUseCase = new TakeDamageUseCase(health);
    }

    public void TakeDamage(int damage)
    {
        _takeDamageUseCase.Execute(damage);
    }
}
