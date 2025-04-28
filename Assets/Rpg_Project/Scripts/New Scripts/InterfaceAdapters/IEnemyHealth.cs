public interface IEnemyHealth
{
    void ReceiveDamage(int damage);
    bool IsDead { get; }
}
