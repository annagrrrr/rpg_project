using UnityEngine;
using System;

public class BossController : MonoBehaviour
{
    [Header("Boss Settings")]
    [SerializeField] private float detectionRange = 25f;
    [SerializeField] private float moveSpeed = 2.5f;
    [SerializeField] private int damage = 500;

    [Header("References")]
    [SerializeField] private EnemyController enemyController;
    [SerializeField] private EnemyHealthPresenter healthPresenter;

    private Transform player;
    private PlayerHealthController playerHealth;
    private EnemyKillTracker killTracker;
    private bool isRangedBoss = false;

    public event Action OnBossDied;

    private void Start()
    {
        Debug.Log("=== BOSS INITIALIZATION ===");

        FindPlayer();

        killTracker = FindObjectOfType<EnemyKillTracker>();
        Debug.Log($"KillTracker found: {killTracker != null}");

        if (enemyController == null)
            enemyController = GetComponent<EnemyController>();

        if (healthPresenter == null)
            healthPresenter = GetComponent<EnemyHealthPresenter>();

        Debug.Log($"EnemyController: {enemyController != null}");
        Debug.Log($"HealthPresenter: {healthPresenter != null}");
        Debug.Log($"PlayerHealth: {playerHealth != null}");

        isRangedBoss = UnityEngine.Random.value > 0.5f;
        Debug.Log($"Boss type: {(isRangedBoss ? "RANGED" : "MELEE")}");

        ConfigureBossType();

        InitializeEnemyController();

        Invoke(nameof(ForceInitializeWeapon), 0.5f);

        if (healthPresenter != null)
        {
            healthPresenter.OnDied += HandleDeath;
        }
    }

    private void FindPlayer()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
            Debug.Log($"Player object found: {playerObj.name}");

            playerHealth = playerObj.GetComponent<PlayerHealthController>();

            if (playerHealth == null)
            {
                playerHealth = playerObj.GetComponentInChildren<PlayerHealthController>();
                Debug.Log("Trying GetComponentInChildren...");
            }

            if (playerHealth == null)
            {
                playerHealth = FindObjectOfType<PlayerHealthController>();
                Debug.Log("Trying FindObjectOfType...");
            }

            if (playerHealth != null)
            {
                Debug.Log($"✅ PlayerHealthController FOUND: {playerHealth.gameObject.name}");
            }
            else
            {
                Debug.LogError("❌ PlayerHealthController NOT FOUND on player!");
                Debug.Log("Player components:");
                foreach (var comp in playerObj.GetComponents<Component>())
                {
                    Debug.Log($"  - {comp.GetType().Name}");
                }
            }
        }
        else
        {
            Debug.LogError("❌ Player object with tag 'Player' not found!");
        }
    }

    private void ConfigureBossType()
    {
        if (enemyController == null) return;

        enemyController.transform.localScale = new Vector3(0.55f, 0.55f, 0.55f);

        try
        {
            var behaviourField = typeof(EnemyController).GetField("behaviourType",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            if (behaviourField != null)
            {
                if (isRangedBoss)
                {
                    behaviourField.SetValue(enemyController, EnemyBehaviourTypes.Ranged);
                    Debug.Log("Boss configured as RANGED");

                    var attackRangeField = typeof(EnemyController).GetField("attackRange",
                        System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                    if (attackRangeField != null)
                        attackRangeField.SetValue(enemyController, 10f);

                    var safeDistanceField = typeof(EnemyController).GetField("safeDistance",
                        System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                    if (safeDistanceField != null)
                        safeDistanceField.SetValue(enemyController, 6f);
                }
                else
                {
                    behaviourField.SetValue(enemyController, EnemyBehaviourTypes.Melee);
                    Debug.Log("Boss configured as MELEE");

                    var attackRangeField = typeof(EnemyController).GetField("attackRange",
                        System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                    if (attackRangeField != null)
                        attackRangeField.SetValue(enemyController, 4f);
                }
            }

            var damageField = typeof(EnemyController).GetField("damage",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (damageField != null)
                damageField.SetValue(enemyController, 30);

            if (healthPresenter != null)
            {
                var maxHealthField = typeof(EnemyHealthPresenter).GetField("maxHealth",
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                if (maxHealthField != null)
                    maxHealthField.SetValue(healthPresenter, 300);
            }
        }
        catch (Exception e)
        {
            Debug.LogWarning($"Could not configure boss via reflection: {e.Message}");
        }
    }

    private void InitializeEnemyController()
    {
        if (enemyController == null)
        {
            Debug.LogError("EnemyController is null!");
            return;
        }

        if (playerHealth == null)
        {
            Debug.LogError("PlayerHealthController is null! Cannot initialize EnemyController");
            return;
        }

        if (killTracker == null)
        {
            Debug.LogError("KillTracker is null!");
            killTracker = FindObjectOfType<EnemyKillTracker>();
        }

        enemyController.Initialize(playerHealth, killTracker);
        Debug.Log("✅ EnemyController initialized for boss!");

        enemyController.enabled = true;
    }

    private void ForceInitializeWeapon()
    {
        Debug.Log("=== FORCE INITIALIZING BOSS WEAPON ===");

        var weapon = GetComponentInChildren<MeleeEnemyWeapon>();
        if (weapon == null)
        {
            Debug.LogError("MeleeEnemyWeapon not found on boss!");
            return;
        }

        if (playerHealth == null)
        {
            Debug.LogError("PlayerHealthController still null! Cannot initialize weapon");
            return;
        }

        var bossData = new EnemyData
        {
            DetectionRange = detectionRange,
            AttackRange = isRangedBoss ? 10f : 4f, 
            MoveSpeed = moveSpeed,
            AttackCooldown = 1.5f,
            Damage = 30 
        };

        weapon.Initialize(playerHealth, bossData);
        Debug.Log($" Boss weapon initialized! AttackRange: {bossData.AttackRange}, Damage: {bossData.Damage}");

        if (enemyController != null)
        {
            var weaponObjectField = typeof(EnemyController).GetField("weaponObject",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            if (weaponObjectField != null)
            {
                weaponObjectField.SetValue(enemyController, weapon as MonoBehaviour);
                Debug.Log("Weapon linked to EnemyController");
            }
        }
    }

    private void FixWeaponParenting()
    {
        var weapons = GetComponentsInChildren<IEnemyWeapon>(true);
        Debug.Log($"Found {weapons.Length} weapons on boss");

        foreach (var weapon in weapons)
        {
            var weaponTransform = (weapon as MonoBehaviour)?.transform;
            if (weaponTransform != null)
            {
                Debug.Log($"Weapon: {weaponTransform.name}, Parent: {weaponTransform.parent?.name}");

                if (weaponTransform.parent != transform)
                {
                    weaponTransform.SetParent(transform);
                    weaponTransform.localPosition = new Vector3(0, 0, 1f); 
                    Debug.Log($"Moved weapon {weaponTransform.name} to boss");
                }
            }
        }
    }

    private void Update()
    {
        if (player == null || healthPresenter == null || healthPresenter.IsDead) return;
    }

    private void HandleDeath()
    {
        Debug.Log("=== BOSS DEFEATED! ===");

        OnBossDied?.Invoke();

        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.AddScore(50);
            Debug.Log("Added 50 score for boss kill");
        }

        Destroy(gameObject, 3f);
    }

    public void Initialize(Transform playerTransform)
    {
        player = playerTransform;
        if (player != null)
        {
            playerHealth = player.GetComponent<PlayerHealthController>();
            if (playerHealth == null)
                playerHealth = player.GetComponentInChildren<PlayerHealthController>();

            Debug.Log($"Boss initialized with player: {player.name}, HealthController: {playerHealth != null}");
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying) return;

        Gizmos.color = Color.red;
        float attackRadius = isRangedBoss ? 10f : 4f;
        Gizmos.DrawWireSphere(transform.position, attackRadius);

        if (player != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, player.position);
        }
    }
}