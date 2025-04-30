using UnityEngine;

public class EnemyPresenter
{
    private readonly IEnemyBehaviourr _behaviour;
    private readonly Transform _transform;
    private readonly Transform _playerTransform;
    private readonly PlayerHealthController _playerHealth;
    private readonly IEnemyHealth _enemyHealth;
    private readonly float _moveSpeed;

    private bool _isDead = false;
    private bool _isStunned = false;
    private float _stunTimer = 0f;
    public EnemyPresenter(
        IEnemyBehaviourr behaviour,
        Transform transform,
        PlayerHealthController playerHealth,
        IEnemyHealth enemyHealth,
        float moveSpeed)
    {
        _behaviour = behaviour;
        _transform = transform;
        _playerHealth = playerHealth;
        _playerTransform = playerHealth.transform;
        _enemyHealth = enemyHealth;
        _moveSpeed = moveSpeed;

        if (_behaviour is MeleeEnemyBehaviour melee)
        {
            melee.SetMoveCallback(MoveTowards);
            melee.SetAttackCallback(AttackPlayer);
        }
        else if (_behaviour is RangedEnemyBehaviour ranged)
        {
            ranged.SetMoveCallback(MoveTowards);
            ranged.SetAttackCallback(AttackPlayer);
            ranged.SetRotateCallback(RotateTowards);
        }

        _enemyHealth.OnDamaged += OnDamaged;
    }

    public void Tick()
    {
        if (_isDead) return;

        if (_enemyHealth.IsDead)
        {
            HandleDeath();
            return;
        }

        if (_playerTransform == null)
        {
            Debug.LogWarning("Player transform is null!");
            return;
        }

        if (_isStunned)
        {
            _stunTimer -= Time.deltaTime;
            if (_stunTimer <= 0f)
            {
                _isStunned = false;
            }
            return;
        }

        _behaviour.Tick(_transform.position, _playerTransform);
    }

    private void MoveTowards(Vector3 direction)
    {
        if (direction == Vector3.zero) return;

        _transform.position += direction.normalized * _moveSpeed * Time.deltaTime;
    }

    private void RotateTowards(Vector3 direction)
    {
        if (direction == Vector3.zero) return;

        Quaternion lookRotation = Quaternion.LookRotation(direction);
        _transform.rotation = Quaternion.Slerp(_transform.rotation, lookRotation, 10f * Time.deltaTime);
    }

    private void AttackPlayer()
    {
        if (_behaviour is IEnemyWithData withData)
        {
            int damage = withData.GetData().Damage;
            _playerHealth.ReceiveDamage(damage);
            Debug.Log($"Enemy attacks player for {damage} damage!");
        }
    }

    private void HandleDeath()
    {
        if (_isDead) return;

        _isDead = true;
        Debug.Log("Enemy has died!");

        GameObject.Destroy(_transform.gameObject, 2f);
    }

    private void OnDamaged()
    {
        Stun(0.5f);
    }

    public void Stun(float duration)
    {
        _isStunned = true;
        _stunTimer = duration;
    }
}
