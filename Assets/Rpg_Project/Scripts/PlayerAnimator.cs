using UnityEngine;

public class PlayerAnimator : MonoBehaviour, IPlayerAnimator
{
    private Animator animator;

    private static readonly int Move = Animator.StringToHash("isMoving");
    private static readonly int Sprint = Animator.StringToHash("isSprinting");
    private static readonly int Attack = Animator.StringToHash("Attack");
    private static readonly int MagicAttack = Animator.StringToHash("MagicAttack");
    private static readonly int Jump = Animator.StringToHash("Jump");
    private static readonly int Die = Animator.StringToHash("Die");
    private static readonly int Stun = Animator.StringToHash("Stun");

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void SetMove(bool isMoving)
    {
        animator.SetBool(Sprint, !isMoving);
        animator.SetBool(Move, isMoving);
    }

    public void SetSprint(bool isSprinting)
    {
        animator.SetBool(Move, !isSprinting);
        animator.SetBool(Sprint, isSprinting);
    }

    public void PlayAttack()
    {
        animator.CrossFade(Attack, 0.05f);
    }
    public void PlayMagicAttack()
    {
        animator.CrossFade(MagicAttack, 0.15f);
    }

    public void PlayStun()
    {
        animator.CrossFade(Stun, 0.1f);
    }
    public void PlayJump()
    {
        animator.ResetTrigger(Attack);
        animator.ResetTrigger(Stun);
        animator.Play(Jump, 0, 0f);
    }


    public void PlayIdle()
    {
        animator.SetBool(Move, false);
    }

    public void PlayDie()
    {
        animator.CrossFade(Die, 0.1f);
    }
}
