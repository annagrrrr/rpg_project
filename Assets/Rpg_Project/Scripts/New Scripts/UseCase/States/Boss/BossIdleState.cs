using UnityEngine;

public class BossIdleState : IBossState
{
    public void Enter(BossController boss)
    {
        Debug.Log("Boss: Entering Idle state");
    }

    public void Execute(BossController boss)

    {
        Debug.Log("Boss in Idle. Checking player range...");
        if (boss.IsPlayerInRange(boss.AggroRange))
        {
            Debug.Log("Player in range! Switching to Aggro.");
            boss.ChangeState(new BossAggroState());
        }
    }

    public void Exit(BossController boss)
    {
        Debug.Log("Boss: Exiting Idle state");
    }
}
