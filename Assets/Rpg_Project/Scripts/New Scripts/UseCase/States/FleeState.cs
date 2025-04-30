using UnityEngine;

public class FleeState : IEnemyState
{
    public void Enter(EnemyController enemy) { Debug.Log($"{enemy.name} вошел в состояние Flee"); }

    public void Execute(EnemyController enemy)
    {
        Vector3 direction = (enemy.transform.position - enemy.PlayerPosition).normalized;
        Vector3 fleePosition = enemy.transform.position + direction * enemy.SafeDistance;
        enemy.MoveTowards(fleePosition);

        if (Vector3.Distance(enemy.transform.position, enemy.PlayerPosition) >= enemy.SafeDistance)
        {
            enemy.ChangeState(new IdleState());
        }
    }

    public void Exit(EnemyController enemy) { }
}
