using UnityEngine;

public class MeleeEnemyWeapon : MonoBehaviour, IEnemyWeapon
{
    [SerializeField] private int damage = 10;
    private IPlayerTarget _playerTarget;
    private EnemyData _enemyData;

    public void Initialize(IPlayerTarget playerTarget, EnemyData enemyData)
    {
        _playerTarget = playerTarget;
        _enemyData = enemyData;
    }

    public void Attack()
    {
        if (_playerTarget == null || _enemyData == null)
            return;

        float distance = Vector3.Distance(transform.parent.position, _playerTarget.Transform.position);  

        if (distance <= _enemyData.AttackRange)
        {
            _playerTarget.ReceiveDamage(damage);
            Debug.Log($"[MeleeEnemyWeapon] Удар прошёл! Урон: {damage}, расстояние: {distance:F2}");
        }
        else
        {
            Debug.Log($"[MeleeEnemyWeapon] Игрок слишком далеко: {distance:F2} > {_enemyData.AttackRange}");
        }
    }
}
