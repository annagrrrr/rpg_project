public interface IEnemyWeapon
{
    void Initialize(IPlayerTarget playerTarget, EnemyData data);
    void Attack();
}