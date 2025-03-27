using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public HealthManager healthManager;
    public DamageHandler damageHandler;
    public MovementController movementController;
    public Weapon meleeWeapon;
    public Weapon magicWeapon;
    public int Mana { get; private set; }
    public int PhysicalAttack { get; private set; }
    public int MagicAttack { get; private set; }

    private Rigidbody rb;
    public float moveSpeed = 5f;
    private bool isGrounded;
    public float jumpForce = 5f;
    public LayerMask groundLayer;
    private Collider capsuleCollider;
    private IAttack meleeAttack;
    private IAttack magicAttack;

    public void Initialize(HealthManager health, DamageHandler damage)
    {
        healthManager = health ?? throw new System.ArgumentNullException(nameof(health), "HealthManager не инициализирован! Установите его через Initialize().");
        damageHandler = damage ?? throw new System.ArgumentNullException(nameof(damage), "DamageHandler не найден!");
    }


    private void Start()
    {
        if (healthManager == null)
        {
            Debug.LogError("HealthManager не инициализирован! Установите его через Initialize().");
        }
        movementController = GetComponent<MovementController>();
        rb = GetComponent<Rigidbody>();
        meleeAttack = new MeleeAttack();
        magicAttack = new MagicAttack();

        if (rb != null)
        {
            rb.useGravity = true;
        }
        else
        {
            Debug.LogError("Rigidbody не найден! Добавьте его на объект.");
        }

        capsuleCollider = GetComponent<Collider>();
        Collider[] allColliders = GetComponentsInChildren<Collider>();

        foreach (var otherCollider in allColliders)
        {
            if (otherCollider != capsuleCollider)
            {
                Physics.IgnoreCollision(capsuleCollider, otherCollider, true);
            }
        }
    }

    private void Update()
    {
        Move();
        Jump();
        HandleAttack();
    }

    private void Move()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontal, 0, vertical).normalized;

        if (movement.magnitude >= 0.1f && rb != null)
        {
            Vector3 moveDirection = transform.forward * movement.z + transform.right * movement.x;
            rb.MovePosition(rb.position + moveDirection * moveSpeed * Time.deltaTime);
        }
    }

    private void Jump()
    {
        if (Mathf.Abs(transform.position.y - 1f) < 0.1f && Input.GetKeyDown(KeyCode.Space) && rb != null)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void HandleAttack()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (meleeWeapon != null)
            {
                meleeWeapon.attackType.ExecuteAttack(transform, meleeWeapon.damage);
                Debug.Log("Melee Attack");
            }
            else
            {
                Debug.LogWarning("Нет ближнего оружия!");
            }
        }
        else if (Input.GetMouseButtonDown(1))
        {
            if (magicWeapon != null)
            {
                magicWeapon.attackType.ExecuteAttack(transform, magicWeapon.damage);
                Debug.Log("Magic Attack");
            }
            else
            {
                Debug.LogWarning("Нет магического оружия!");
            }
        }
    }

    public void ReceiveDamage(int amount, DamageType type, float physicalResistance, float magicResistance)
    {
        if (healthManager != null && damageHandler != null)
        {
            int finalDamage = damageHandler.CalculateDamage(amount, type, physicalResistance, magicResistance);
            healthManager.TakeDamage(finalDamage);
        }
        else
        {
            Debug.LogError("HealthManager или DamageHandler не инициализированы!");
        }
    }

    public void DealDamage(Enemy target, int damage, DamageType type)
    {
        if (type == DamageType.PHYSICAL && meleeWeapon != null)
        {
            meleeWeapon.PerformAttack();
        }
        else if (type == DamageType.MAGICAL && magicWeapon != null)
        {
            magicWeapon.PerformAttack();
        }
        else
        {
            Debug.LogWarning("Нет подходящего оружия для атаки!");
        }
    }

    public void EquipWeapon(Weapon weapon, bool isMelee)
    {
        if (isMelee)
        {
            meleeWeapon = weapon;
        }
        else
        {
            magicWeapon = weapon;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"Collision detected with: {collision.gameObject.name}");
    }
}
