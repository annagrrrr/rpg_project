using System;

public interface IEnemyHealth
{
    int ReceiveDamage(int damage);
    int ReceiveDamage(int damage, AttackType attackType);
    bool IsDead { get; }

    event Action OnDamaged;
}
