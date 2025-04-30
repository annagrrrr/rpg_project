using UnityEngine;

public class BossRetreatState : IBossState
{
    private float retreatDuration = 2f;
    private float startTime;

    public void Enter(BossController boss)
    {
        Debug.Log("Boss: Entering Retreat state");
        startTime = Time.time;
    }

    public void Execute(BossController boss)
    {
        boss.MoveAwayFromPlayer(boss.Data.MoveSpeed);

        if (Time.time - startTime >= retreatDuration)
        {
            boss.ChangeState(new BossAggroState());
        }
    }

    public void Exit(BossController boss)
    {
        Debug.Log("Boss: Exiting Retreat state");
    }
}
