using UnityEngine;

public class RangedEnemyBehaviour : IEnemyBehaviourr, IEnemyWithData
{
    private readonly RangedEnemyData _data;
    private readonly EnemyAttackUseCase _attackUseCase;

    private System.Action<Vector3> _onMoveRequested;
    private System.Action _onAttackRequested;
    private System.Action<Vector3> _onRotateRequested;

    public RangedEnemyBehaviour(RangedEnemyData data)
    {
        _data = data;
        _attackUseCase = new EnemyAttackUseCase(_data.AttackCooldown);
    }

    public void Tick(Vector3 enemyPosition, Transform playerTransform)
    {
        float distance = Vector3.Distance(enemyPosition, playerTransform.position);

        if (distance > _data.DetectionRange)
            return;

        Vector3 directionToPlayer = (playerTransform.position - enemyPosition).normalized;

        if (distance < _data.SafeDistance)
        {
            _onMoveRequested?.Invoke(-directionToPlayer);
            _onRotateRequested?.Invoke(-directionToPlayer);
        }
        else
        {
            _onRotateRequested?.Invoke(directionToPlayer);

            _attackUseCase.TryAttack(() =>
            {
                _onAttackRequested?.Invoke();
            });
        }
    }

    public void SetMoveCallback(System.Action<Vector3> callback)
    {
        _onMoveRequested = callback;
    }

    public void SetAttackCallback(System.Action callback)
    {
        _onAttackRequested = callback;
    }

    public void SetRotateCallback(System.Action<Vector3> callback)
    {
        _onRotateRequested = callback;
    }

    public EnemyData GetData() => _data;
}
