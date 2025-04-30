using UnityEngine;

public class BossIdleState : IBossState
{
    public void Enter(BossController boss)
    {
        Debug.Log("Boss: Entering Idle state");
    }

    public void Execute(BossController boss)
    {
        if (GameModeManager.Instance.CurrentMode == GameMode.Peaceful && !boss.IsPlayerInRange(boss.AggroRange))
        {
            return; 
        }

        
        if (boss.IsPlayerInRange(boss.AggroRange))
        {
            boss.ChangeState(new BossAggroState()); 
        }
    }


    public void Exit(BossController boss)
    {
        Debug.Log("Boss: Exiting Idle state");
    }
}
