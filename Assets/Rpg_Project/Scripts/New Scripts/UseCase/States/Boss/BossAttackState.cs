using UnityEngine;

public class BossAttackState : IBossState
{
    private float attackCooldown = 2f;
    private float lastAttackTime;

    public void Enter(BossController boss)
    {
        Debug.Log("Boss: Entering Attack state");
        lastAttackTime = Time.time - attackCooldown;
    }

    public void Execute(BossController boss)
    {
        if (Time.time - lastAttackTime >= attackCooldown)
        {
            boss.PerformAttack();
            lastAttackTime = Time.time;

            if (boss.ShouldUseHeavyAttack())
            {
                boss.ChangeState(new BossHeavyAttackState());
            }
        }

        if (!boss.IsPlayerInRange(boss.AttackRange))
        {
            boss.ChangeState(new BossAggroState());
        }
    }

    public void Exit(BossController boss)
    {
        Debug.Log("Boss: Exiting Attack state");
    }
}
