using UnityEngine;

public class AttackState : IEnemyState
{
    private float lastAttackTime = 0f;

    public void Enter(EnemyController enemy)
    {
        lastAttackTime = Time.time - enemy.AttackCooldown;
        Debug.Log($"{enemy.name} вошел в состояние Attack");
    }

    public void Execute(EnemyController enemy)
    {
        if (!enemy.IsPlayerInRange(enemy.AttackRange))
        {
            enemy.ChangeState(new AggressionState());
            return;
        }

        if (Time.time - lastAttackTime >= enemy.AttackCooldown)
        {
            enemy.Attack();
            lastAttackTime = Time.time;
        }
    }

    public void Exit(EnemyController enemy) { }
}
