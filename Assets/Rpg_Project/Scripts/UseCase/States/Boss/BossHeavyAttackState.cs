using UnityEngine;

public class BossHeavyAttackState : IBossState
{
    private bool hasAttacked = false;

    public void Enter(BossController boss)
    {
        Debug.Log("Boss: Entering HeavyAttack state");
        hasAttacked = false;
    }

    public void Execute(BossController boss)
    {
        if (!hasAttacked)
        {
            boss.PerformHeavyAttack();
            hasAttacked = true;
        }

        if (boss.IsPlayerInRange(boss.AttackRange))
        {
            boss.ChangeState(new BossAttackState());
        }
        else
        {
            boss.ChangeState(new BossAggroState());
        }
    }

    public void Exit(BossController boss)
    {
        Debug.Log("Boss: Exiting HeavyAttack state");
    }
}
