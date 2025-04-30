using UnityEngine;

public class BossEnragedState : IBossState
{
    public void Enter(BossController boss)
    {
        Debug.Log("Boss: Entering Enraged state");
        boss.IncreaseDamageAndSpeed();
    }

    public void Execute(BossController boss)
    {
        if (boss.IsPlayerInRange(boss.AttackRange))
        {
            boss.ChangeState(new BossHeavyAttackState());
        }
        else
        {
            boss.MoveTowardsPlayer(boss.Data.MoveSpeed * 1.5f);
        }
    }

    public void Exit(BossController boss)
    {
        Debug.Log("Boss: Exiting Enraged state");
    }
}
