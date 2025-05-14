using System;

public interface IEnemyHealth
{
    void ReceiveDamage(int damage);
    void ReceiveDamage(int damage, AttackType attackType);
    bool IsDead { get; }

    event Action OnDamaged;
}
