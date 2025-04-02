using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private HealthManager healthManager;
    private DamageHandler damageHandler;
    private HealthUIUpdater healthUIUpdater;
    [SerializeField] private MovementController movementController;
    [SerializeField] private PlayerInputHandler inputHandler;
    [SerializeField] private Weapon meleeWeapon;
    [SerializeField] private Weapon magicWeapon;
    [SerializeField] private LayerMask groundLayer;

    [SerializeField] private HealthBar healthBar;
    private bool isStunned = false;
    private float stunDuration = 0f;

    private Rigidbody rb;
    private Collider capsuleCollider;
    private IAttack meleeAttack;
    private IAttack magicAttack;
    private PlayerAnimator playerAnimator;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float rotationSpeed = 700f;
    [SerializeField] private float sprintMultiplier = 1.5f;
    [SerializeField] private Image magicAttackIcon;  
    private float magicAttackCooldown = 5f;  
    private float currentCooldown = 0f;

    private bool isJumping = false;

    public void Initialize(HealthManager healthManager, DamageHandler damageHandler, HealthBar healthBar)
    {
        this.healthManager = healthManager ?? throw new ArgumentNullException(nameof(healthManager));
        this.damageHandler = damageHandler ?? throw new ArgumentNullException(nameof(damageHandler));
        this.healthUIUpdater = new HealthUIUpdater(healthBar);
        healthUIUpdater.Subscribe(healthManager);
    }

    private void Start()
    {
        playerAnimator = GetComponent<PlayerAnimator>();
        rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = true;
        }
        else
        {
            Debug.LogError("Rigidbody не найден! Добавьте его на объект.");
        }

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
        HandleMagicAttackCooldown();
    }
    private void HandleMagicAttackCooldown()
    {
        if (currentCooldown > 0)
        {
            currentCooldown -= Time.deltaTime;  
            UpdateMagicAttackIconColor();  
        }
    }
    private void Move()
    {
        if (isStunned) return;
        Vector2 input = inputHandler.GetMovementInput();
        Vector3 movement = new Vector3(input.x, 0, input.y).normalized;

        if (movement.magnitude >= 0.1f && rb != null)
        {
            float currentMoveSpeed = moveSpeed;
            if (Input.GetKey(KeyCode.LeftShift))
            {
                currentMoveSpeed *= sprintMultiplier;
                playerAnimator.SetSprint(true);
            }
            else
            {
                playerAnimator.SetMove(true);
            }

            Vector3 moveDirection = transform.forward * movement.z + transform.right * movement.x;
            rb.MovePosition(rb.position + moveDirection * currentMoveSpeed * Time.deltaTime);
        }
        else if (!isJumping)
        {
            playerAnimator.PlayIdle();
        }
    }
    private void UpdateMagicAttackIconColor()
    {
        if (currentCooldown <= 0)
        {
            
            magicAttackIcon.color = Color.green;
        }
        else
        {


            magicAttackIcon.color = Color.red;
        }
    }
    private void Jump()
    {
        if (Physics.Raycast(transform.position, Vector3.down, 1.1f, groundLayer) && inputHandler.IsJumpPressed() && rb != null)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            playerAnimator.PlayJump();
            isJumping = true;
        }
        else if (isJumping)
        {
            isJumping = false;
            //playerAnimator.PlayIdle();
            if (inputHandler.GetMovementInput().magnitude > 0.1f)
            {
                playerAnimator.SetMove(true);
            }
            else
            {
                playerAnimator.PlayIdle();
            }
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
        if (inputHandler.IsMeleeAttackPressed() && meleeWeapon != null && !isStunned)
        {
            meleeWeapon.attackType.ExecuteAttack(transform, meleeWeapon.damage);
            playerAnimator.PlayAttack();
        }
        else if (inputHandler.IsMagicAttackPressed() && magicWeapon != null && !isStunned && currentCooldown <= 0)
        {
            magicWeapon.attackType.ExecuteAttack(transform, magicWeapon.damage);
            playerAnimator.PlayMagicAttack();
            currentCooldown = magicAttackCooldown;
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

    public void ReceiveDamage(int damage)
    {
        if (healthManager != null && damageHandler != null)
        {
            healthManager.TakeDamage(damage);
            ApplyStun(0.3f);

            if (healthBar != null)
            {
                healthBar.SetHealth(healthManager.currentHealth, healthManager.maxHealth);
            }

            if (healthManager.currentHealth <= 0)
            {
                Die();
            }
        }
        else
        {
            Debug.LogError("HealthManager или DamageHandler не инициализированы!");
        }
    }

    private void Die()
    {
        FindFirstObjectByType<UIManager>().ShowGameOverPanel();
        playerAnimator.PlayDie();
        this.enabled = false;
        movementController.enabled = false;
        inputHandler.enabled = false;
        //Destroy(gameObject, 3f);
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
        Debug.Log("player stunned?!!");
        isStunned = true;
        playerAnimator.PlayStun();
        movementController.enabled = false;
        yield return new WaitForSeconds(duration);
        isStunned = false;
        movementController.enabled = true;
        playerAnimator.PlayIdle();
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
