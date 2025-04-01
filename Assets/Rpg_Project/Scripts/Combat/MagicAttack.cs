using UnityEngine;

public class MagicAttack : IAttack
{
    private float attackRange = 15f;
    private int defaultDamage = 15;
    private float lastAttackTime = 0f;
    private float attackCooldown = 1f; 

    public void ExecuteAttack(Transform attackerTransform, int damage = -1)
    {
        if (Time.time - lastAttackTime < attackCooldown)
            return;

        lastAttackTime = Time.time;

        int finalDamage = (damage != -1) ? damage : defaultDamage;

        Vector3 attackStart = attackerTransform.position + Vector3.up * 0.5f;
        Vector3 direction = attackerTransform.forward;
        Vector3 attackEnd = attackStart + direction * attackRange;

        Debug.DrawLine(attackStart, attackEnd, Color.blue, 1f);
        Debug.Log($"ÀÒÀÊÓÞ Magic Attack Ray: Start={attackStart}, End={attackEnd}");

        if (Physics.Raycast(attackStart, direction, out RaycastHit hit, attackRange))
        {
            Debug.Log($"Hit: {hit.collider.name}");
            if (hit.collider.TryGetComponent(out Enemy enemy))
            {
                enemy.TakeDamage(finalDamage, DamageType.MAGICAL);
            }
            else if (hit.collider.TryGetComponent(out PlayerController player))
            {
                player.ReceiveDamage(finalDamage); 
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
