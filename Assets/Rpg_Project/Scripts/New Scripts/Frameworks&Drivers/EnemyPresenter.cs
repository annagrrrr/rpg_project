using UnityEngine;

public class EnemyPresenter
{
    private readonly IEnemyBehaviourr _behaviour;
    private readonly Transform _transform;
    private readonly Transform _playerTransform;
    private readonly float _moveSpeed;
    private readonly PlayerHealthController _playerHealth;
    private readonly EnemyHealthController _enemyHealthController; 

    public EnemyPresenter(
        IEnemyBehaviourr behaviour,
        Transform transform,
        Transform playerTransform,
        float moveSpeed,
        PlayerHealthController playerHealth,
        EnemyHealthController enemyHealthController)  
    {
        _behaviour = behaviour;
        _transform = transform;
        _playerTransform = playerTransform;
        _moveSpeed = moveSpeed;
        _playerHealth = playerHealth;
        _enemyHealthController = enemyHealthController; 

        if (_behaviour is MeleeEnemyBehaviour melee)
        {
            melee.SetMoveCallback(MoveTowards);
            melee.SetAttackCallback(Attack);
        }
        else if (_behaviour is RangedEnemyBehaviour ranged)
        {
            ranged.SetMoveCallback(MoveTowards);
            ranged.SetAttackCallback(Attack);
            ranged.SetRotateCallback(RotateTowards);
        }
    }

    public void Tick()
    {
        _behaviour.Tick(_transform.position, _playerTransform);
    }

    private void MoveTowards(Vector3 direction)
    {
        _transform.position += direction * _moveSpeed * Time.deltaTime;
    }

    private void Attack()
    {
        Debug.Log("Enemy attacks!");
        if (_behaviour is IEnemyWithData withData)
        {
            int damage = withData.GetData().Damage;
            _playerHealth.ReceiveDamage(damage);
        }
    }

    private void RotateTowards(Vector3 direction)
    {
        if (direction == Vector3.zero) return;

        Quaternion lookRotation = Quaternion.LookRotation(direction);
        _transform.rotation = Quaternion.Slerp(_transform.rotation, lookRotation, 10f * Time.deltaTime);
    }
    private void HandleDeath()
    {
        if (_enemyHealthController.IsDead())
        {
            Debug.Log("Enemy is dead!");
        }
    }
}
