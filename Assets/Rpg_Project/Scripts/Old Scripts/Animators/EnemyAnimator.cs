using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public void SetMove(bool isMoving)
    {
        animator.SetBool("isMoving", isMoving);
        animator.SetBool("isAttacking", false);
        animator.SetBool("isStunned", false);
    }

    public void Attack()
    {
        animator.SetBool("isMoving", false);
        animator.SetBool("isStunned", false);
        animator.SetBool("isAttacking", true);
    }

    public void Stun()
    {
        animator.SetBool("isMoving", false);
        animator.SetBool("isAttacking", false);
        animator.SetBool("isStunned", true);
    }

    public void Die()
    {
        animator.SetBool("isAttacking", false);
        animator.SetBool("isStunned", false);
        animator.SetBool("isMoving", false);
        animator.SetTrigger("isDead");
    }
}
