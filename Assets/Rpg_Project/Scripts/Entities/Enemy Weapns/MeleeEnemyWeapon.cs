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

        
            _playerTarget.ReceiveDamage(damage);
            
        
        
    }
}
