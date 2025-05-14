using UnityEngine;

public interface IBossWeapon
{
    float AttackRange { get; }
    void Attack(Transform target, int damage);
    void SetElement(ElementType element);
}
