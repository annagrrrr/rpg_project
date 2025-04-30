using UnityEngine;

public class BossController : MonoBehaviour
{
    [Header("Boss Settings")]
    [SerializeField] private BossData data;
    [SerializeField] private Transform player;

    public Transform Player => player;
    public BossData Data => data;

    private IBossState currentState;
    private bool canMove = true;
    private bool isProvoked = false;
    public float AttackRange => data.AttackRange;
    public float AggroRange => data.AggroRange;
    public float HeavyAttackRange => data.HeavyAttackRange;
    public float RetreatThreshold => data.RetreatHealthThreshold;



    private void Start()
    {
        if (player == null)
        {
            var p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
            else Debug.LogError("BossController: не найден объект с тегом Player");
        }
        ChangeState(new BossIdleState());
    }

    private void Update()
    {
        currentState?.Execute(this);
        if (isCharging)
            UpdateCharge();
    }

    public void Initialize(Transform player)
    {
        this.player = player;
        ChangeState(new BossIdleState());
    }

    public void ChangeState(IBossState newState)
    {
        currentState?.Exit(this);
        currentState = newState;
        currentState?.Enter(this);
    }

    
    private bool IsPeacefulMode()
    {
        return GameModeManager.Instance.CurrentMode == GameMode.Peaceful;
    }

    
    public void PerformAttack()
    {
        if (IsPeacefulMode()) return;
        Debug.Log("Boss: Performing normal attack!");
    }

    public void PerformHeavyAttack()
    {
        if (IsPeacefulMode()) return;
        Debug.Log("Boss: Performing heavy attack!");
    }

    public void PerformCharge()
    {
        if (IsPeacefulMode()) return;
        Debug.Log("Boss: Charging!");
    }

    public void PerformRetreat()
    {
        if (IsPeacefulMode()) return;
        Debug.Log("Boss: Retreating!");
    }

    public void PerformEnrage()
    {
        if (IsPeacefulMode()) return;
        Debug.Log("Boss: Enraged!");
    }

    public void PerformStun()
    {
        if (IsPeacefulMode()) return;
        Debug.Log("Boss: Stunned!");
    }

    
    public void MoveTowards(Vector3 target, float speed)
    {
        if (IsPeacefulMode()) return;  
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

    
    private float chargeTimer = 0f;
    private bool isCharging = false;

    public void StartCharge()
    {
        if (IsPeacefulMode()) return;
        Debug.Log("Boss: Started Charge");
        isCharging = true;
        chargeTimer = data.ChargeDuration;
        SetChargeTarget(PlayerPosition);
        DisableMovement();
    }

    public void UpdateCharge()
    {
        if (IsPeacefulMode() || !isCharging) return;

        chargeTimer -= Time.deltaTime;
        MoveTowards(chargeTarget, data.ChargeSpeed);
    }

    public void StopCharge()
    {
        if (IsPeacefulMode()) return;
        Debug.Log("Boss: Stopped Charge");
        isCharging = false;
        EnableMovement();
    }

    
    public bool IsPlayerInRange(float range)
    {
        if (player == null) return false;
        return Vector3.Distance(transform.position, player.position) <= range;
    }

    public float DistanceToPlayer()
    {
        if (player == null) return Mathf.Infinity;
        return Vector3.Distance(transform.position, player.position);
    }

    
    private Vector3 chargeTarget;
    public void SetChargeTarget(Vector3 target)
    {
        chargeTarget = target;
    }

    public bool HasReachedChargeTarget()
    {
        return Vector3.Distance(transform.position, chargeTarget) <= 1f;
    }

    
    public void MoveAwayFromPlayer(float speed)
    {
        if (IsPeacefulMode()) return;
        Vector3 dir = (transform.position - PlayerPosition).normalized;
        MoveTowards(transform.position + dir, speed);
    }

   
    private bool enraged = false;
    public void IncreaseDamageAndSpeed()
    {
        if (IsPeacefulMode()) return;
        if (enraged) return;

        Debug.Log("Boss: Enraged! Increasing stats.");
        data.Damage *= 2;
        data.MoveSpeed *= 1.5f;
        enraged = true;
    }

    
    public bool ShouldUseHeavyAttack()
    {
        if (IsPeacefulMode()) return false;
        return IsPlayerInRange(data.HeavyAttackRange) && Random.value > 0.5f;
    }

    public void MoveTowardsPlayer(float speed)
    {
        if (IsPeacefulMode()) return;
        MoveTowards(PlayerPosition, speed);
    }

    public Vector3 PlayerPosition => player?.position ?? transform.position;

    
}
