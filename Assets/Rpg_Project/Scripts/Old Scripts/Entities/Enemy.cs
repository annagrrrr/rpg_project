using System;
using System.Collections;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.InputSystem.OnScreen.OnScreenStick;

public class Enemy : MonoBehaviour
{
    public string Name { get; private set; }
    public HealthManager Health { get; private set; }
    private HealthBar healthBar;

    public float PhysicalResistance { get; private set; }
    public float MagicResistance { get; private set; }

    public int Damage { get; private set; }
    public DamageType DamageType { get; private set; }
    private IAttack AttackType;
    private float attackRange;
    [SerializeField] private string enemyID;
    public string EnemyID => enemyID;


    private DamageHandler damageHandler;

    private IEnemyBehaviour currentBehaviour;

    private PlayerController playerController;

    private EnemyAnimator animator;

    public bool isAttacking = false;

    //���� ���������
    private bool isStunned = false;
    private float stunDuration = 0f;

    //������ ��� �����
    private float attackCooldown = 1.5f;
    private float nextAttackTime = 0f;

    public void Initialize(string name, int maxHealth, int damage, float physicalResistance, float magicResistance, IEnemyBehaviour behaviour, IAttack attackType, HealthBar enemyHealthBar)
    {
        Transform child = transform.Find("MeleeAttackerMesh");
        Name = name;
        Health = new HealthManager(maxHealth);
        damageHandler = new DamageHandler();
        healthBar = enemyHealthBar;
        Health.OnHealthChanged += UpdateHealthBar;
        healthBar = GetComponentInChildren<HealthBar>();
        Health.TakeDamage(0);
        Damage = damage;
        animator = GetComponent<EnemyAnimator>();
        AttackType = attackType;
        attackRange = attackType.GetAttackRange();
        PhysicalResistance = physicalResistance;
        MagicResistance = magicResistance;
        currentBehaviour = behaviour;
        Debug.Log(behaviour);
        
        damageHandler = new DamageHandler();
        
        playerController = GameObject.FindWithTag("Player").GetComponentInChildren<PlayerController>();

        Health.onDamageTaken += (damage) => Debug.Log($"{Name} ������� {damage} �����!");
        Health.onDeath += () => Debug.Log($"{Name} �����!");
    }

    private void Update()
    {
        if (isStunned)
        {
            return;
        }

        if (currentBehaviour != null)
        {
            Transform player = FindPlayerTransform();
            currentBehaviour.UpdateBehaviour(this, player);
        }
    }
    

    public void SetEnemyID(string id)
    {
        enemyID = id;
    }

    public void TakeDamage(int baseDamage, DamageType damageType)
    {
        if (Health.currentHealth <= 0) return;
        ApplyStun(0.5f);
        int finalDamage = damageHandler.CalculateDamage(baseDamage, damageType, PhysicalResistance, MagicResistance);
        Health.TakeDamage(finalDamage);

        if (Health.currentHealth <= 0)
        {
            Die();
        }
    }

    private Transform FindPlayerTransform()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        return player?.transform;
    }
    public void Attack(Transform target)
    {
        if (isAttacking || target == null || !target.CompareTag("Player")) return;

        float distanceToPlayer = Vector3.Distance(transform.position, target.position);
        if (distanceToPlayer <= attackRange)
        {
            Vector3 directionToTarget = (target.position - transform.position).normalized;
            float angle = Vector3.Angle(transform.forward, directionToTarget);
            if (angle > 10f)
            {
                Quaternion lookRotation = Quaternion.LookRotation(new Vector3(directionToTarget.x, 0, directionToTarget.z));
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
                return;
            }
            Debug.Log($"{Name} �������� ���������!");

            PlayerController player = target.GetComponent<PlayerController>();
            if (player != null)
            {
                isAttacking = true;
                int finalDamage = damageHandler.CalculateDamage(Damage, DamageType, 0f, 0f);
                animator.Attack();
                AttackType.ExecuteAttack(transform, finalDamage);
                Debug.Log($"{Name} ������� ������ �� {Damage} �����!");
                StartCoroutine(ResetAttackCooldown(1f));
            }
        }
    }


    private IEnumerator ResetAttackCooldown(float duration)
    {
        yield return new WaitForSeconds(duration);
        isAttacking = false;
    }

    public void MoveTowards(Transform target, float speed)
    {
        if (target == null)
        {
            Debug.LogWarning("no target err");
            animator.SetMove(false);
            return;
        }
        animator.SetMove(true);
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        Vector3 direction = (target.position - transform.position).normalized;

        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
    }

    public void MoveAwayFrom(Transform target, float speed, float safeDistance)
    {
        float distanceToTarget = Vector3.Distance(transform.position, target.position);
        float error = 0.1f;

        if (distanceToTarget + error < safeDistance)
        {
            animator.SetMove(true);
            Vector3 direction = (transform.position - target.position).normalized;
            transform.position += direction * speed * Time.deltaTime;

            if (direction != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
            }
        }
        else
        {
            Vector3 lookDirection = (target.position - transform.position).normalized;
            lookDirection.y = 0;
            if (lookDirection != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(lookDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
            }
        }
    }




    private void Die()
    {
        animator.Die();
        Debug.Log($"{Name} �����!");
        animator.Die();

        currentBehaviour = null;

        StopAllCoroutines();

        Destroy(gameObject, 6f);
    }
    public void ApplyStun(float duration)
    {
        if (!isStunned)
        {
            StartCoroutine(StunCoroutine(duration));
        }
    }
    private IEnumerator StunCoroutine(float duration)
    {
        isStunned = true;
        animator.Stun();
        stunDuration = duration;

        Debug.Log($"{Name} v stane na {duration} sec!");

        yield return new WaitForSeconds(duration);

        isStunned = false;
        Debug.Log($"{Name} not v stane.");
    }

    private void UpdateHealthBar(int currentHealth, int maxHealth)
    {
        if (healthBar != null)
        {
            healthBar.SetHealth(currentHealth, maxHealth);
        }
    }

}
