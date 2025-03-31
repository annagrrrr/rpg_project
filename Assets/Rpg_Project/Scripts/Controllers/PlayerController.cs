using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private HealthManager healthManager;
    [SerializeField] private DamageHandler damageHandler;
    [SerializeField] private MovementController movementController;
    [SerializeField] private PlayerInputHandler inputHandler;
    [SerializeField] private Weapon meleeWeapon;
    [SerializeField] private Weapon magicWeapon;
    [SerializeField] private LayerMask groundLayer;

    [SerializeField] private HealthBar healthBar;


    private Rigidbody rb;
    private Collider capsuleCollider;
    private IAttack meleeAttack;
    private IAttack magicAttack;

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float rotationSpeed = 700f;

    public void Initialize(HealthManager health, DamageHandler damage)
    {
        healthManager = health ?? throw new System.ArgumentNullException(nameof(health), "HealthManager не инициализирован! Установите его через Initialize().");
        damageHandler = damage ?? throw new System.ArgumentNullException(nameof(damage), "DamageHandler не найден!");
        if (healthBar != null)
        {
            healthManager.OnHealthChanged += healthBar.SetHealth;
        }
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb != null) rb.useGravity = true;
        else Debug.LogError("Rigidbody не найден! Добавьте его на объект.");

        capsuleCollider = GetComponent<Collider>();
        IgnoreSelfCollisions();

        meleeAttack = new MeleeAttack();
        magicAttack = new MagicAttack();
    }

    private void Update()
    {
        Move();
        Jump();
        Rotate();
        HandleAttack();
    }

    private void Move()
    {
        Vector2 input = inputHandler.GetMovementInput();
        Vector3 movement = new Vector3(input.x, 0, input.y).normalized;

        if (movement.magnitude >= 0.1f && rb != null)
        {
            Vector3 moveDirection = transform.forward * movement.z + transform.right * movement.x;
            rb.MovePosition(rb.position + moveDirection * moveSpeed * Time.deltaTime);
        }
    }

    private void Jump()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 1.1f, groundLayer) && inputHandler.IsJumpPressed() && rb != null)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }


    private void Rotate()
    {
        Vector2 mouseDelta = inputHandler.GetMouseDelta();
        float rotationY = mouseDelta.x * rotationSpeed * Time.deltaTime;
        transform.Rotate(0, rotationY, 0);
    }

    private void HandleAttack()
    {
        if (inputHandler.IsMeleeAttackPressed() && meleeWeapon != null)
        {
            meleeWeapon.attackType.ExecuteAttack(transform, meleeWeapon.damage);
        }
        else if (inputHandler.IsMagicAttackPressed() && magicWeapon != null)
        {
            magicWeapon.attackType.ExecuteAttack(transform, magicWeapon.damage);
        }
    }

    private void IgnoreSelfCollisions()
    {
        Collider[] allColliders = GetComponentsInChildren<Collider>();
        foreach (var otherCollider in allColliders)
        {
            if (otherCollider != capsuleCollider)
            {
                Physics.IgnoreCollision(capsuleCollider, otherCollider, true);
            }
        }
    }

    public void EquipWeapon(Weapon weapon, bool isMelee)
    {
        if (isMelee) meleeWeapon = weapon;
        else magicWeapon = weapon;
    }

    public void ReceiveDamage(int amount, DamageType type, float physicalResistance, float magicResistance)
    {
        if (healthManager != null && damageHandler != null)
        {
            int finalDamage = damageHandler.CalculateDamage(amount, type, physicalResistance, magicResistance);
            healthManager.TakeDamage(finalDamage);
            Debug.Log("dddd");
            healthBar.SetHealth(healthManager.currentHealth, finalDamage);
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

    public bool HasMeleeWeapon()
    {
        return meleeWeapon != null;
    }

    public bool HasMagicWeapon()
    {
        return magicWeapon != null;
    }

    public Weapon GetMeleeWeapon()
    {
        return meleeWeapon;
    }

    public Weapon GetMagicWeapon()
    {
        return magicWeapon;
    }

}
