public class StunPlayerUseCase
{
    private readonly PlayerStunState _stunState;
    private readonly IPlayerAnimationPresenter _animator;

    public StunPlayerUseCase(PlayerStunState stunState, IPlayerAnimationPresenter animator)
    {
        _stunState = stunState;
        _animator = animator;
    }

    public void Stun(float duration)
    {
        _animator.PlayStunAnimation();
        _stunState.Stun(duration);
    }

    public bool IsStunned()
    {
        _stunState.Update();
        return _stunState.IsStunned;
    }
}
