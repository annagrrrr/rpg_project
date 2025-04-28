using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Behaviour Settings")]
    [SerializeField] private float detectionRange = 10f;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float attackCooldown = 1.5f;
    [SerializeField] private int damage = 10;
    [SerializeField] private EnemyBehaviourTypes behaviourType;
    [SerializeField] private float safeDistance = 5f;

    [Header("References")]
    [SerializeField] private PlayerHealthController playerHealth;
    [SerializeField] private EnemyHealthPresenter enemyHealthPresenter;

    private EnemyPresenter _presenter;

    public void Initialize(PlayerHealthController playerHealth)
    {
        this.playerHealth = playerHealth;
    }

    private EnemyData CreateData()
    {
        return behaviourType switch
        {
            EnemyBehaviourTypes.Melee => new EnemyData
            {
                DetectionRange = detectionRange,
                AttackRange = attackRange,
                MoveSpeed = moveSpeed,
                AttackCooldown = attackCooldown,
                Damage = damage
            },

            EnemyBehaviourTypes.Ranged => new RangedEnemyData
            {
                DetectionRange = detectionRange,
                AttackRange = attackRange,
                MoveSpeed = moveSpeed,
                AttackCooldown = attackCooldown,
                SafeDistance = safeDistance,
                Damage = damage
            },

            _ => throw new System.NotImplementedException($"Unknown behaviour type: {behaviourType}")
        };
    }

    private void Start()
    {
        if (playerHealth == null)
        {
            Debug.LogError("EnemyController: PlayerHealthController not assigned!");
            return;
        }

        if (enemyHealthPresenter == null)
        {
            Debug.LogError("EnemyController: EnemyHealthPresenter not assigned!");
            return;
        }

        var data = CreateData();

        IEnemyBehaviourr behaviour = behaviourType switch
        {
            EnemyBehaviourTypes.Melee => new MeleeEnemyBehaviour(data),
            EnemyBehaviourTypes.Ranged => new RangedEnemyBehaviour((RangedEnemyData)data),
            _ => throw new System.NotImplementedException()
        };

        _presenter = new EnemyPresenter(
            behaviour,
            transform,
            playerHealth,
            enemyHealthPresenter,
            moveSpeed
        );
    }

    private void Update()
    {
        _presenter?.Tick();
    }
}
