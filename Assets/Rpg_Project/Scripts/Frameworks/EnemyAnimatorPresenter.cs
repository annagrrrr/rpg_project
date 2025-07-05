using UnityEngine;

public class EnemyAnimatorPresenter : MonoBehaviour, IEnemyAnimationPresenter
{
    [SerializeField] private Animator _animator;

    public void PlayRunAnimation(bool isRunning)
    {
        _animator.SetBool("isRunning", isRunning);
    }

    public void PlayAttackAnimation()
    {
        _animator.SetTrigger("Attack");
    }
    public void PlayStunAnimation()
    {
        _animator.SetTrigger("Stun");
    }

    public void PlayDeathAnimation()
    {
        _animator.SetTrigger("Death");
    }
}
