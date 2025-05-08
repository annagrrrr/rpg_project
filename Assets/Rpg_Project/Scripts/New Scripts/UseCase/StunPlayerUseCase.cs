public class StunPlayerUseCase
{
    private readonly PlayerStunState _stunState;

    public StunPlayerUseCase(PlayerStunState stunState)
    {
        _stunState = stunState;
    }

    public void Stun(float duration)
    {
        _stunState.Stun(duration);
    }

    public bool IsStunned()
    {
        _stunState.Update();
        return _stunState.IsStunned;
    }
}
