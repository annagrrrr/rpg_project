public interface IPlayerAnimationPresenter
{
    void PlayRunAnimation(bool isRunning);

    void PlaySprintAnimation(bool isSprinting);
    void PlayAttackAnimation(AttackType type);
    void PlayJumpAnimation();
    void PlayStunAnimation();
}
