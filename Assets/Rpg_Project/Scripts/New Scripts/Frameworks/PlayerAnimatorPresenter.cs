using UnityEngine;

public class PlayerAnimatorPresenter : MonoBehaviour, IPlayerAnimationPresenter
{
    [SerializeField] private Animator _animator;

    public void PlayRunAnimation(bool isRunning)
    {
        _animator.SetBool("isRunning", isRunning);
    }

    public void PlayAttackAnimation(AttackType type)
    {
        _animator.SetTrigger(type == AttackType.Physical ? "MeleeAttack" : "MagicAttack");
    }

    public void PlayJumpAnimation()
    {
        _animator.SetTrigger("Jump");
    }

    public void PlayStunAnimation()
    {
        _animator.SetTrigger("Stun");
    }
}
