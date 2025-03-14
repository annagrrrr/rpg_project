using UnityEngine;

public class RangedBehaviour : IEnemyBehaviour
{
    public float safeDistance = 10f;

    public void UpdateBehaviour(Enemy enemy, Transform player)
    {
        if (player == null || enemy == null) return;

        float distance = Vector3.Distance(enemy.transform.position, player.position);

        if (distance <= safeDistance)
        {
            enemy.MoveAwayFrom(player, 5f, safeDistance); 
        }
        else
        {
            enemy.Attack(player);
        }
    }
}
