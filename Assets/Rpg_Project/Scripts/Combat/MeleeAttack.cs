using UnityEngine;

public class MeleeAttack : IAttack
{
    private float attackRange;
    private int attackDamage;
    private float attackCooldown = 1f; 
    private float lastAttackTime = -1f; 

    public MeleeAttack() 
    {
        attackRange = 5f;
        attackDamage = 20;
    }

    public MeleeAttack(float range, int damage) 
    {
        attackRange = range;
        attackDamage = damage;
    }

    public void ExecuteAttack(Transform attackerTransform, int damage = -1)
    {
        if (Time.time - lastAttackTime < attackCooldown)
        {
            return;
        }

        lastAttackTime = Time.time;

        int finalDamage = (damage != -1) ? damage : attackDamage;

        Vector3 attackStart = attackerTransform.position + Vector3.up * 0.5f;
        Vector3 direction = attackerTransform.TransformDirection(Vector3.forward);
        Vector3 attackEnd = attackStart + direction * attackRange;

        Debug.DrawLine(attackStart, attackEnd, Color.red, 1f);

        if (Physics.Raycast(attackStart, direction, out RaycastHit hit, attackRange))
        {
            Debug.Log($"Hit: {hit.collider.name}");

            if (hit.collider.TryGetComponent(out Enemy enemy))
            {
                enemy.TakeDamage(finalDamage, DamageType.PHYSICAL);
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
