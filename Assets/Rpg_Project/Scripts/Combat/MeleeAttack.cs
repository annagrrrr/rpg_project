using UnityEngine;

public class MeleeAttack : IAttack
{
    private float attackRange;
    private int attackDamage;
    public MeleeAttack() //для врагаа с дфеолтными параметрами
    {
        attackRange = 5f;
        attackDamage = 20;
    }

    public MeleeAttack(float range, int damage) //для игрока можно задавать параметры
    {
        attackRange = range;
        attackDamage = damage;
    }

    public void ExecuteAttack(Transform attackerTransform, int damage = -1)
    {
        Vector3 attackStart = attackerTransform.position + Vector3.up * 0.5f;
        Vector3 direction = attackerTransform.TransformDirection(Vector3.forward);
        Vector3 attackEnd = attackStart + direction * attackRange;

        Debug.DrawLine(attackStart, attackEnd, Color.red, 1f);

        if (Physics.Raycast(attackStart, direction, out RaycastHit hit, attackRange))
        {
            Debug.Log($"Hit: {hit.collider.name}");
            if (hit.collider.CompareTag("Player")) // Если попали в игрока
            {
                PlayerController player = hit.collider.GetComponent<PlayerController>();
                if (player != null)
                {
                    int finalDamage = (damage != -1) ? damage : attackDamage;
                    player.ReceiveDamage(finalDamage);  // Передаем урон игроку
                }
            }
            else if (hit.collider.CompareTag("Enemy")) // Если попали во врага
            {
                Enemy enemy = hit.collider.GetComponent<Enemy>();
                if (enemy != null)
                {
                    int finalDamage = (damage != -1) ? damage : attackDamage;
                    enemy.TakeDamage(finalDamage, DamageType.PHYSICAL);  // Передаем урон врагу
                }
            }
        }
        else
        {
            Debug.Log("No hit detected");
        }
    }


    public float GetAttackRange()
    {
        return attackRange;
    }
}
