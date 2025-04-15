public interface IPlayerAnimator
{
    void SetMove(bool isMoving);
    void PlayAttack();
    void PlayJump();
    void PlayIdle();
    void PlayDie();
}
