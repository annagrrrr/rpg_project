using UnityEngine;

public class MeleeBehavior : IEnemyBehaviour
{
    public float attackRange = 3f;

    public void UpdateBehaviour(Enemy enemy, Transform player)
    {
        float distance = Vector3.Distance(enemy.transform.position, player.position);

        if (distance <= attackRange)
        {
            enemy.Attack(player);
        }
        else
        {
            enemy.MoveTowards(player.transform, 5f);
        }
    }
}
