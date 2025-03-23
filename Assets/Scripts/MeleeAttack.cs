using UnityEngine;

public class MeleeAttack : IAttack
{
    //private int damage = 20;
    private float attackRange = 2f;

    public void ExecuteAttack(Transform attackerTransform, int damage)
    {
        Vector3 attackStart = attackerTransform.position + Vector3.up * 0.5f; 
        Vector3 direction = attackerTransform.TransformDirection(Vector3.forward);
        Vector3 attackEnd = attackStart + direction * attackRange;

        Debug.DrawLine(attackStart, attackEnd, Color.red, 1f);
        Debug.Log($"Melee Attack Ray: Start={attackStart}, End={attackEnd}");

        if (Physics.Raycast(attackStart, direction, out RaycastHit hit, attackRange))
        {
            Debug.Log($"Hit: {hit.collider.name}");
            if (hit.collider.TryGetComponent(out Enemy enemy))
            {
                enemy.TakeDamage(damage, DamageType.PHYSICAL);
            }
        }
        else
        {
            Debug.Log("No hit detected");
        }
    }
}
