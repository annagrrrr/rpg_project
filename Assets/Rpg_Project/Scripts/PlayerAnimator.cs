using UnityEngine;

public class PlayerAnimator : MonoBehaviour, IPlayerAnimator
{
    private Animator animator;

    private static readonly int Move = Animator.StringToHash("isMoving");
    private static readonly int Attack = Animator.StringToHash("Attack");
    private static readonly int Jump = Animator.StringToHash("Jump");
    private static readonly int Die = Animator.StringToHash("isDead");
    private static readonly int Stun = Animator.StringToHash("Stun");

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
        animator.CrossFade(Attack, 0.05f);
        //animator.SetTrigger(Attack);
    }

    public void PlayStun()
    {
        animator.CrossFade(Stun, 0.1f);
        //animator.SetTrigger(Stun);
    }
    public void PlayJump()
    {
        animator.ResetTrigger(Attack);
        animator.ResetTrigger(Stun);
        animator.Play(Jump, 0, 0f); // Запускаем анимацию прыжка с самого начала
    }


    public void PlayIdle()
    {
        animator.SetBool(Move, false);
    }

    public void PlayDie()
    {
        animator.SetTrigger(Die);
    }
}
