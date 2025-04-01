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

    private DamageHandler damageHandler;

    private IEnemyBehaviour currentBehaviour;

    private PlayerController playerController;

    private EnemyAnimator animator;

    //стан поведение
    private bool isStunned = false;
    private float stunDuration = 0f;

    //всякое для атаки
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

        Health.onDamageTaken += (damage) => Debug.Log($"{Name} получил {damage} урона!");
        Health.onDeath += () => Debug.Log($"{Name} погиб!");
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
        if (target != null && target.CompareTag("Player"))
        {
            float distanceToPlayer = Vector3.Distance(transform.position, target.position);

            if (distanceToPlayer <= attackRange)
            {
                PlayerController player = target.GetComponent<PlayerController>();
                Debug.Log(player);
                if (player != null)
                {
                    int finalDamage = damageHandler.CalculateDamage(Damage, DamageType, 0f, 0f);
                    animator.Attack();
                    AttackType.ExecuteAttack(transform, finalDamage);
                    Debug.Log($"{Name} атакует игрока за {Damage} урона!");
                }
            }
        }
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
        if (target == null)
        {
            Debug.LogWarning("no target err");
            return;
        }


        float distanceToTarget = Vector3.Distance(transform.position, target.position);


        if (distanceToTarget < safeDistance)
        {

            Vector3 direction = (transform.position - target.position).normalized;

            transform.position += direction * speed * Time.deltaTime;

            if (direction != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
            }

            Debug.Log($"{Name} отступает от {target.name}");
        }
        else
        {
            Debug.Log($"{Name} на безопасном расстоянии от {target.name}");
        }
    }

    private void Die()
    {
        animator.Die();
        Debug.Log($"{Name} погиб!");
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

        Debug.Log($"{Name} в стане на {duration} секунд!");

        yield return new WaitForSeconds(duration);

        isStunned = false;
        Debug.Log($"{Name} вышел из стана.");
    }

    private void UpdateHealthBar(int currentHealth, int maxHealth)
    {
        if (healthBar != null)
        {
            healthBar.SetHealth(currentHealth, maxHealth);
        }
    }

}
