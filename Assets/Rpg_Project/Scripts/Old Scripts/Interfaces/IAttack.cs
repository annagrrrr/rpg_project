using UnityEngine;
public interface IAttack
{
    void ExecuteAttack(Transform attackerTransform, int damage);
    float GetAttackRange();
}
