using UnityEngine;

public class MeleeEnemyBehaviour : IEnemyBehaviourr, IEnemyWithData
{
    private readonly EnemyData _data;
    private readonly EnemyAttackUseCase _attackUseCase;

    private System.Action<Vector3> _onMoveRequested;
    private System.Action _onAttackRequested;

    public MeleeEnemyBehaviour(EnemyData data)
    {
        _data = data;
        _attackUseCase = new EnemyAttackUseCase(_data.AttackCooldown);
    }

    public void Tick(Vector3 enemyPosition, Transform playerTransform)
    {
        float distance = Vector3.Distance(enemyPosition, playerTransform.position);

        if (distance <= _data.AttackRange)
        {
            _attackUseCase.TryAttack(() =>
            {
                _onAttackRequested?.Invoke();
            });
        }
        else if (distance <= _data.DetectionRange)
        {
            Vector3 direction = (playerTransform.position - enemyPosition).normalized;
            _onMoveRequested?.Invoke(direction);
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
    public EnemyData GetData() => _data;
}
