using UnityEngine;

public class PlayerAnimator : MonoBehaviour, IPlayerAnimator
{
    private Animator animator;

    private static readonly int Move = Animator.StringToHash("Move");
    private static readonly int Attack = Animator.StringToHash("Attack");
    private static readonly int Jump = Animator.StringToHash("Jump");
    private static readonly int Idle = Animator.StringToHash("Idle");
    private static readonly int Die = Animator.StringToHash("Die");

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void SetMove(bool isMoving)
    {
        animator.SetBool(Move, isMoving);
    }

    public void PlayAttack()
    {
        animator.SetTrigger(Attack);
    }

    public void PlayJump()
    {
        animator.SetTrigger(Jump);
    }

    public void PlayIdle()
    {
        animator.SetTrigger(Idle);
    }

    public void PlayDie()
    {
        animator.SetTrigger(Die);
    }
}
