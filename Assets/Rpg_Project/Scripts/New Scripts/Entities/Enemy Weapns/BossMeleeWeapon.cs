using UnityEngine;

public class BossMeleeWeapon : MonoBehaviour, IBossWeapon
{
    public float AttackRange => 4f;
    private ElementType _element;

    public void Attack(Transform target, int damage)
    {
        Debug.Log($"Boss Melee attacks {target.name} for {damage} damage with {_element} element.");
    }

    public void SetElement(ElementType element)
    {
        _element = element;
    }
}
