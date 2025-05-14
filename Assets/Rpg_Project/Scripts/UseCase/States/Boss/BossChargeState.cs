using UnityEngine;

public class BossChargeState : IBossState
{
    public void Enter(BossController boss)
    {
        Debug.Log("Boss: Entering Charge state");
        boss.StartCharge();
    }

    public void Execute(BossController boss)
    {
        boss.UpdateCharge();

        if (boss.HasReachedChargeTarget())
        {
            boss.StopCharge();
            boss.ChangeState(new BossAttackState());
        }
    }

    public void Exit(BossController boss)
    {
        Debug.Log("Boss: Exiting Charge state");
    }
}
