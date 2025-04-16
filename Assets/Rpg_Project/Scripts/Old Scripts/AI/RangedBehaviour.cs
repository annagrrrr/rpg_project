using UnityEngine;

public class RangedBehaviour : IEnemyBehaviour
{
    public float safeDistance = 10f;

    public void UpdateBehaviour(Enemy enemy, Transform player)
    {
        float distance = Vector3.Distance(enemy.transform.position, player.position);

        if (distance < safeDistance - 0.5f)
        {
            enemy.MoveAwayFrom(player, 3f, safeDistance);
        }
        else if (distance <= safeDistance + 0.5f)
        {
            if (!enemy.isAttacking)
            {
                enemy.Attack(player);
            }
        }
        else
        {
            enemy.isAttacking = false;
        }
    }
}
