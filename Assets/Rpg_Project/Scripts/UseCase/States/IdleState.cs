using UnityEngine;

public class IdleState : IEnemyState
{
    public void Enter(EnemyController enemy) {
        //Debug.Log($"{enemy.name} вошел в состояние Idle");
        }

    public void Execute(EnemyController enemy)
    {
        if (enemy.IsPlayerInRange(enemy.DetectionRange))
        {
            enemy.ChangeState(new AggressionState());
        }
    }

    public void Exit(EnemyController enemy) { }
}
