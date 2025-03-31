using UnityEngine;

public class PlayerMeleeAttack : IAttack
{
    private float attackRange;
    private int attackDamage;

    public PlayerMeleeAttack(float range, int damage)
    {
        attackRange = range;
        attackDamage = damage;
    }

    public void ExecuteAttack(Transform attackerTransform, int damage)
    {
        Vector3 attackStart = attackerTransform.position + Vector3.up * 0.5f;
        Vector3 direction = attackerTransform.forward;

        if (Physics.Raycast(attackStart, direction, out RaycastHit hit, attackRange))
        {
            if (hit.collider.TryGetComponent(out Enemy enemy))
            {
                enemy.TakeDamage(attackDamage, DamageType.PHYSICAL);
            }
        }
    }
    public float GetAttackRange()
    {
        return attackRange;
    }
}
