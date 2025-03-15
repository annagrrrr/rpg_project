using UnityEngine;

public class MagicAttack : IAttack
{
    private int damage = 15;
    private float attackRange = 10f;

    public void ExecuteAttack(Transform attackerTransform)
    {
        Vector3 attackStart = attackerTransform.position + Vector3.up * 0.5f;
        Vector3 direction = attackerTransform.TransformDirection(Vector3.forward);
        Vector3 attackEnd = attackStart + direction * attackRange;

        Debug.DrawLine(attackStart, attackEnd, Color.blue, 1f);
        Debug.Log($"Magic Attack Ray: Start={attackStart}, End={attackEnd}");

        if (Physics.Raycast(attackStart, direction, out RaycastHit hit, attackRange))
        {
            Debug.Log($"Hit: {hit.collider.name}");
            if (hit.collider.TryGetComponent(out Enemy enemy))
            {
                enemy.TakeDamage(damage, DamageType.MAGICAL);
            }
        }
        else
        {
            Debug.Log("No hit detected");
        }
    }
}
