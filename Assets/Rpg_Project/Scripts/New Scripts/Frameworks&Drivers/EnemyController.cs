using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float detectionRange = 10f;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float attackCooldown = 1.5f;
    [SerializeField] private int damage = 10;
    [SerializeField] private EnemyBehaviourTypes behaviourType;
    [SerializeField] private float safeDistance = 5f;
    [SerializeField] private PlayerHealthController playerHealth;
    [SerializeField] private EnemyHealthController enemyHealthController;

    private EnemyPresenter _presenter;
    public void Initialize(PlayerHealthController playerHealth)
    {
        this.playerHealth = playerHealth;
    }

    private EnemyData CreateData()
    {


        switch (behaviourType)
        {
            case EnemyBehaviourTypes.Melee:
                return new EnemyData
                {
                    DetectionRange = detectionRange,
                    AttackRange = attackRange,
                    MoveSpeed = moveSpeed,
                    AttackCooldown = attackCooldown,
                    Damage = damage
                };

            case EnemyBehaviourTypes.Ranged:
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
        if (playerHealth == null)
        {
            Debug.LogError("EnemyController: PlayerHealthController not assigned!");
            return;
        }

        var data = CreateData();

        IEnemyBehaviourr behaviour = behaviourType switch
        {
            EnemyBehaviourTypes.Melee => new MeleeEnemyBehaviour(data),
            EnemyBehaviourTypes.Ranged => new RangedEnemyBehaviour((RangedEnemyData)data),
            _ => throw new System.NotImplementedException()
        };

        var player = playerHealth.transform;

        _presenter = new EnemyPresenter(behaviour, transform, player, moveSpeed, playerHealth, enemyHealthController);
    }



    private void Update()
    {
        _presenter.Tick();
    }
}