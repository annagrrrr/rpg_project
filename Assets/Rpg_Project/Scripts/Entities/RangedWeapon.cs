using UnityEngine;

public class RangedWeapon : IWeapon
{
    public AttackType AttackType { get; }
    public int Damage { get; }

    public RangedWeapon(int damage, AttackType attackType = AttackType.Magical)
    {
        Damage = damage;
        AttackType = attackType;
    }
}
