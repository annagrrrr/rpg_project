using UnityEngine;

public class EnemyController : MonoBehaviour, IStunnable
{
    [Header("Behaviour Settings")]
    [SerializeField] private float detectionRange = 10f;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float attackCooldown = 1.5f;
    [SerializeField] private int damage = 10;
    [SerializeField] private EnemyBehaviourTypes behaviourType;
    [SerializeField] private float safeDistance = 5f;
    [SerializeField] private EnemyAnimatorPresenter enemyAnimatorPresenter;
    public EnemyHealthPresenter Health => enemyHealthPresenter;

    [Header("References")]
    [SerializeField] private PlayerHealthController playerHealth;
    [SerializeField] private EnemyHealthPresenter enemyHealthPresenter;

    [SerializeField] private MonoBehaviour weaponObject;
    private IEnemyWeapon weapon;

    private EnemyPresenter _presenter;
    private IEnemyState currentState;

    private bool _isRunning = false;
    private EnemyKillTracker _killTracker;
    private bool ifDead = false;
    public Vector3 PlayerPosition => playerHealth != null ? playerHealth.transform.position : transform.position;
    public float DetectionRange => detectionRange;
    public float AttackRange => attackRange;
    public float SafeDistance => safeDistance;
    public float AttackCooldown => attackCooldown;
    public int Damage => damage;
    public bool IsDead => enemyHealthPresenter.IsDead;

    private StunState _stunState = new StunState();
    public bool IsStunned => _stunState.IsStunned;

    public void Initialize(PlayerHealthController playerHealth, EnemyKillTracker killTracker)
    {
        this.playerHealth = playerHealth;
        this._killTracker = killTracker;
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
        weapon = weaponObject.GetComponent<IEnemyWeapon>();
        weapon?.Initialize(playerHealth, CreateData());
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
            moveSpeed,
            enemyAnimatorPresenter,
            weapon
        );
        enemyHealthPresenter.OnDied += HandleDeath;
        ChangeState(new IdleState());
    }

    private void Update()
    {
        if (IsStunned) return;
        if (IsDead) return;

        currentState?.Execute(this);
    }

    public void ChangeState(IEnemyState newState)
    {
        if (currentState == newState) return;
        currentState?.Exit(this);
        currentState = newState;
        currentState?.Enter(this);
    }

    public void MoveTowards(Vector3 target)
    {
        Vector3 dir = (target - transform.position).normalized;

        if (dir == Vector3.zero)
        {
            SetRunning(false);
            return;
        }

        transform.position += dir * moveSpeed * Time.deltaTime;

        Quaternion rot = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * 10f);

        SetRunning(true);
    }

    public void Attack()
    {
        enemyAnimatorPresenter.PlayAttackAnimation();
        weapon?.Attack();
        Debug.Log("Enemy attacks player!");
    }

    public bool IsPlayerInRange(float range)
    {
        return Vector3.Distance(transform.position, PlayerPosition) <= range;
    }

    public void SetRunning(bool isRunning)
    {
        if (_isRunning == isRunning) return;
        _isRunning = isRunning;
        enemyAnimatorPresenter.PlayRunAnimation(isRunning);
    }

    private void HandleDeath()
    {
        if (ifDead)
            return;

        ifDead = true;
        enemyAnimatorPresenter.PlayDeathAnimation();
        _killTracker?.RegisterKill();
        
    }

    public void ApplyStun(float duration)
    {
        _stunState.ApplyStun(duration);
        enemyAnimatorPresenter.PlayStunAnimation();
    }
}
