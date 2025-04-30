using UnityEngine;

public class AggressionState : IEnemyState
{
    public void Enter(EnemyController enemy) { Debug.Log($"{enemy.name} вошел в состояние Aggression"); }

    public void Execute(EnemyController enemy)
    {
        
        if (GameModeManager.Instance.CurrentMode == GameMode.Peaceful)
        {
            enemy.ChangeState(new IdleState());
            return;
            if (enemy.Health.Current < enemy.Health.Max * 0.3f)
            {
                enemy.ChangeState(new FleeState());
                return;
            }
        }

        
        if (enemy.IsPlayerInRange(enemy.AttackRange))
        {
            enemy.ChangeState(new AttackState());
        }
        else if (!enemy.IsPlayerInRange(enemy.DetectionRange))
        {
            enemy.ChangeState(new IdleState());
        }
        else
        {
            enemy.MoveTowards(enemy.PlayerPosition);
        }
    }

    public void Exit(EnemyController enemy) { }
}
