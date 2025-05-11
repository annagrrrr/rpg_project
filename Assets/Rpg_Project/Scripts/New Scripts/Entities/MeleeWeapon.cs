using UnityEngine;

public class MeleeWeapon : IWeapon
{
    public AttackType AttackType { get; }
    public int Damage { get; }

    public MeleeWeapon(int damage, AttackType attackType = AttackType.Physical)
    {
        Damage = damage;
        AttackType = attackType;
    }
}
