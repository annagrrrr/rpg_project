using UnityEngine;

public class RangedEnemyWeapon : MonoBehaviour, IEnemyWeapon
{
    [SerializeField] private int damage = 10;
    [SerializeField] private float rayDistance = 100f;
    [SerializeField] private LayerMask hitMask; // Укажи нужный слой (например, "Player")

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

        Vector3 direction = (_playerTarget.Transform.position - transform.position).normalized;

        // RaycastAll возвращает все попадания
        RaycastHit[] hits = Physics.RaycastAll(transform.position, direction, rayDistance, hitMask);

        foreach (var hit in hits)
        {
            if (hit.collider.TryGetComponent(out IPlayerTarget target))
            {
                target.ReceiveDamage(damage);
                Debug.Log($"[RangedEnemyWeapon] Попадание по игроку! Урон: {damage}");
                break; // Остановить после первого попадания по игроку
            }
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (_playerTarget != null)
        {
            Gizmos.color = Color.red;
            Vector3 direction = (_playerTarget.Transform.position - transform.position).normalized;
            Gizmos.DrawRay(transform.position, direction * rayDistance);
        }
    }
#endif
}
