using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float detectionRange = 10f;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float attackCooldown = 1.5f;
    [SerializeField] private int damage = 10;
    [SerializeField] private EnemyBehaviourType behaviourType;
    [SerializeField] private float safeDistance = 5f;
    [SerializeField] private PlayerHealthController playerHealth;
    [SerializeField] private EnemyHealthController enemyHealthController;

    private EnemyPresenter _presenter;

    private EnemyData CreateData()
    {
        switch (behaviourType)
        {
            case EnemyBehaviourType.Melee:
                return new EnemyData
                {
                    DetectionRange = detectionRange,
                    AttackRange = attackRange,
                    MoveSpeed = moveSpeed,
                    AttackCooldown = attackCooldown,
                    Damage = damage
                };

            case EnemyBehaviourType.Ranged:
                return new RangedEnemyData
                {
                    DetectionRange = detectionRange,
                    AttackRange = attackRange,
                    MoveSpeed = moveSpeed,
                    AttackCooldown = attackCooldown,
                    SafeDistance = safeDistance,
                    Damage = damage
                };

            default:
                throw new System.NotImplementedException($"unknown behaviour type: {behaviourType}");
        }
    }

    private void Start()
    {
        var data = CreateData();

        IEnemyBehaviourr behaviour = behaviourType switch
        {
            EnemyBehaviourType.Melee => new MeleeEnemyBehaviour(data),
            EnemyBehaviourType.Ranged => new RangedEnemyBehaviour((RangedEnemyData)data),
            _ => throw new System.NotImplementedException()
        };

        var player = GameObject.FindWithTag("Player")?.transform;
        _presenter = new EnemyPresenter(behaviour, transform, player, moveSpeed, playerHealth, enemyHealthController);
    }


    private void Update()
    {
        _presenter.Tick();
    }
}