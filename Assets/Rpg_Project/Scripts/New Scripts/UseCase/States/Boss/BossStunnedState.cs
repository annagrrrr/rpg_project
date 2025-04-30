using UnityEngine;

public class BossStunnedState : IBossState
{
    private float stunDuration = 2f;
    private float startTime;

    public void Enter(BossController boss)
    {
        Debug.Log("Boss: Entering Stunned state");
        startTime = Time.time;
        boss.DisableMovement();
    }

    public void Execute(BossController boss)
    {
        if (Time.time - startTime >= stunDuration)
        {
            boss.EnableMovement();
            boss.ChangeState(new BossAggroState());
        }
    }

    public void Exit(BossController boss)
    {
        Debug.Log("Boss: Exiting Stunned state");
    }
}
