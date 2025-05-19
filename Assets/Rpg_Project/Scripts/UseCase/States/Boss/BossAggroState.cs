using UnityEngine;

public class BossAggroState : IBossState
{
    public void Enter(BossController boss)
    {
        //Debug.Log("Boss: Entering Aggro state");
    }

    public void Execute(BossController boss)
    {
        boss.MoveTowardsPlayer(boss.Data.MoveSpeed);

        if (boss.IsPlayerInRange(boss.AttackRange))
        {
            boss.ChangeState(new BossAttackState());
        }
    }

    public void Exit(BossController boss)
    {
        //Debug.Log("Boss: Exiting Aggro state");
    }
}
