using UnityEngine;

public class BossAttackState : IBossState
{
    public void Enter(BossController boss)
    {
        Debug.Log("Boss: Entering Attack state");
    }

    public void Execute(BossController boss)
    {
        if (boss.CanAttack())
        {
            boss.PerformAttack();

            if (boss.ShouldUseHeavyAttack())
            {
                boss.ChangeState(new BossHeavyAttackState());
                return;
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
