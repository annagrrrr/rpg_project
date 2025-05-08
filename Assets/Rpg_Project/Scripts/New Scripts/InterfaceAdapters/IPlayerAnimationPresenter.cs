public interface IPlayerAnimationPresenter
{
    void PlayRunAnimation(bool isRunning);
    void PlayAttackAnimation(AttackType type);
    void PlayJumpAnimation();
    void PlayStunAnimation();
}
