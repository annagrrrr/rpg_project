using UnityEngine;

public class EnemyPresenter
{
    private readonly IEnemyBehaviourr _behaviour;
    private readonly Transform _transform;
    private readonly Transform _playerTransform;
    private readonly PlayerHealthController _playerHealth;
    private readonly IEnemyHealth _enemyHealth;
    private readonly IEnemyAnimationPresenter _animator;
    private readonly float _moveSpeed;

    private bool _isDead = false;
    private bool _isStunned = false;
    private float _stunTimer = 0f;

    private bool _wasMovingThisFrame = false;

    private readonly IEnemyWeapon _weapon;
    public EnemyPresenter(
        IEnemyBehaviourr behaviour,
        Transform transform,
        PlayerHealthController playerHealth,
        IEnemyHealth enemyHealth,
        float moveSpeed,
        IEnemyAnimationPresenter animator,
        IEnemyWeapon weapon)
    {
        _behaviour = behaviour;
        _transform = transform;
        _playerHealth = playerHealth;
        _playerTransform = playerHealth.transform;
        _enemyHealth = enemyHealth;
        _moveSpeed = moveSpeed;
        _animator = animator;
        _weapon = weapon;

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

            _animator.PlayRunAnimation(false);
            return;
        }

        _wasMovingThisFrame = false;
        _behaviour.Tick(_transform.position, _playerTransform);

        if (!_wasMovingThisFrame)
        {
            _animator.PlayRunAnimation(false);
        }
    }


    private void MoveTowards(Vector3 direction)
    {
        Debug.Log("AAAAAAAAAAAAAA");
        if (direction == Vector3.zero) return;

        _wasMovingThisFrame = true;
        _transform.position += direction.normalized * _moveSpeed * Time.deltaTime;
        _animator.PlayRunAnimation(true);
    }


    private void RotateTowards(Vector3 direction)
    {
        if (direction == Vector3.zero) return;

        Quaternion lookRotation = Quaternion.LookRotation(direction);
        _transform.rotation = Quaternion.Slerp(_transform.rotation, lookRotation, 10f * Time.deltaTime);
    }

    private void AttackPlayer()
    {
        _animator.PlayAttackAnimation();
        _weapon?.Attack();

        //if (_behaviour is IEnemyWithData withData)
        //{
        //    int damage = withData.GetData().Damage;
        //    _playerHealth.ReceiveDamage(damage);
        //    Debug.Log($"Enemy attacks player for {damage} damage!");
        //}
    }

    private void HandleDeath()
    {
        if (_isDead) return;

        _isDead = true;
        _animator.PlayDeathAnimation();
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
        _animator.PlayStunAnimation();
    }
}
