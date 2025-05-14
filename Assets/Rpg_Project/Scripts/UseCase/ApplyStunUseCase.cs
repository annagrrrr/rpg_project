public class ApplyStunUseCase
{
    private readonly IStunnable _stunnable;

    public ApplyStunUseCase(IStunnable stunnable)
    {
        _stunnable = stunnable;
    }

    public void Execute(float duration)
    {
        _stunnable.ApplyStun(duration);
    }
}
