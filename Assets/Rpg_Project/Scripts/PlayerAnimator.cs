using UnityEngine;

public class PlayerAnimator : MonoBehaviour, IPlayerAnimator
{
    private Animator animator;

    private static readonly int Move = Animator.StringToHash("isMoving");
    private static readonly int Attack = Animator.StringToHash("isAttacking");
    private static readonly int Jump = Animator.StringToHash("isJumping");
    //private static readonly int Idle = Animator.StringToHash("Idle");
    private static readonly int Die = Animator.StringToHash("isDead");
    private static readonly int Stun = Animator.StringToHash("isStunned");

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void SetMove(bool isMoving)
    {
        animator.SetBool(Jump, false);
        animator.SetBool(Attack, false);
        animator.SetBool(Move, isMoving);
    }

    public void PlayAttack()
    {
        animator.SetBool(Jump, false);
        animator.SetBool(Move, false);
        animator.SetTrigger(Attack);
    }
    public void PlayStun(bool isStunned)
    {
        animator.SetBool(Jump, false);
        animator.SetBool(Move, false);
        animator.SetBool(Attack, false);
        animator.SetBool(Stun, isStunned);
    }

    public void PlayJump()
    {
        animator.SetBool(Move, false);
        animator.SetBool(Attack, false);
        animator.SetTrigger(Jump);
    }

    public void PlayIdle()
    {
        animator.SetBool(Move, false);
        animator.SetBool(Attack, false);
        animator.SetBool(Jump, false);
        //animator.SetTrigger(Idle);
    }

    public void PlayDie()
    {
        animator.SetBool(Stun, false);
        animator.SetBool(Move, false);
        animator.SetBool(Jump, false);
        animator.SetBool(Attack, false);
        animator.SetBool(Die, true);
    }
}
