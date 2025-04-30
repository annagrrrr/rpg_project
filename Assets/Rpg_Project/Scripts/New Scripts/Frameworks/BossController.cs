using UnityEngine;

public class BossController : MonoBehaviour
{
    [Header("Boss Settings")]
    [SerializeField] private BossData data;
    [SerializeField] private Transform player;
    public Transform Player => player;

    public BossData Data => data;

    [SerializeField] private Transform playerTransform;

    private IBossState currentState;

    // Управление движением
    private bool canMove = true;

    // Для ChargeState
    private Vector3 chargeTarget;

    private void Start()
    {
        if (playerTransform == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null) playerTransform = player.transform;
        }

        
    }

    private void Update()
    {
        currentState?.Execute(this);
    }
    public void Initialize(Transform player)
    {
        this.playerTransform = player;
        ChangeState(new BossIdleState());
    }

    public void ChangeState(IBossState newState)
    {
        currentState?.Exit(this);
        currentState = newState;
        currentState?.Enter(this);

        Debug.Log($"Boss: Changed state to {newState.GetType().Name}");
    }

    // ======== Свойства доступа к параметрам из BossData ========
    public float AggroRange => data.AggroRange;
    public float AttackRange => data.AttackRange;
    public float HeavyAttackRange => data.HeavyAttackRange;
    public float RetreatThreshold => data.RetreatHealthThreshold;
    public float ChargeSpeed => data.ChargeSpeed;
    public float ChargeDuration => data.ChargeDuration;

    // ======== Движение и перемещение ========
    public void MoveTowards(Vector3 target, float speed)
    {
        if (!canMove) return;

        Vector3 direction = (target - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;

        RotateTowards(direction);
    }

    public void RotateTowards(Vector3 direction)
    {
        if (direction == Vector3.zero) return;

        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 10f * Time.deltaTime);
    }

    public void EnableMovement() => canMove = true;
    public void DisableMovement() => canMove = false;
    public bool CanMove() => canMove;

    // ======== Работа с ChargeState ========
    public void SetChargeTarget(Vector3 target)
    {
        chargeTarget = target;
    }

    public bool HasReachedChargeTarget()
    {
        return Vector3.Distance(transform.position, chargeTarget) <= 1f;
    }

    // ======== Информация о расстоянии до игрока ========
    public bool IsPlayerInRange(float range)
    {
        if (playerTransform == null) return false;
        return Vector3.Distance(transform.position, playerTransform.position) <= range;
    }

    public float DistanceToPlayer()
    {
        if (playerTransform == null) return Mathf.Infinity;
        return Vector3.Distance(transform.position, playerTransform.position);
        
        
    }
    // ==== CHARGE ====
    private float chargeTimer = 0f;
    private bool isCharging = false;

    public void StartCharge()
    {
        Debug.Log("Boss: Started Charge");
        isCharging = true;
        chargeTimer = ChargeDuration;
        SetChargeTarget(PlayerPosition);
        DisableMovement(); // отключим стандартное движение
    }

    public void UpdateCharge()
    {
        if (!isCharging) return;

        chargeTimer -= Time.deltaTime;
        MoveTowards(chargeTarget, ChargeSpeed);
    }

    public void StopCharge()
    {
        Debug.Log("Boss: Stopped Charge");
        isCharging = false;
        EnableMovement();
    }

    // ==== RETREAT ====
    public void MoveAwayFromPlayer(float speed)
    {
        Vector3 dir = (transform.position - PlayerPosition).normalized;
        MoveTowards(transform.position + dir, speed);
    }

    // ==== ENRAGED ====
    private bool enraged = false;
    public void IncreaseDamageAndSpeed()
    {
        if (enraged) return;

        Debug.Log("Boss: Enraged! Increasing stats.");
        data.Damage *= 2;
        data.MoveSpeed *= 1.5f;
        enraged = true;
    }

    // ==== ATTACK ====
    public bool ShouldUseHeavyAttack()
    {
        // Пример условия: если игрок близко и рандом
        return IsPlayerInRange(HeavyAttackRange) && Random.value > 0.5f;
    }

    public void MoveTowardsPlayer(float speed)
    {
        MoveTowards(PlayerPosition, speed);
    }


    public Vector3 PlayerPosition => playerTransform?.position ?? transform.position;

    // ======== Атаки (можно заменить на анимации или визуал эффекты) ========
    public void PerformAttack()
    {
        Debug.Log("Boss: Performing normal attack!");
    }

    public void PerformHeavyAttack()
    {
        Debug.Log("Boss: Performing heavy attack!");
    }

    public void PerformCharge()
    {
        Debug.Log("Boss: Charging!");
    }

    public void PerformRetreat()
    {
        Debug.Log("Boss: Retreating!");
    }

    public void PerformEnrage()
    {
        Debug.Log("Boss: Enraged!");
    }

    public void PerformStun()
    {
        Debug.Log("Boss: Stunned!");
    }
}
