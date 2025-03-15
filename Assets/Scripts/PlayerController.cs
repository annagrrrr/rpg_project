using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public HealthManager healthManager; 
    public DamageHandler damageHandler; 
    public MovementController movementController; 
    public Weapon equippedWeapon; 
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

    private void Start()
    {
        
        healthManager = GetComponent<HealthManager>();
        damageHandler = GetComponent<DamageHandler>();
        movementController = GetComponent<MovementController>();
        rb = GetComponent<Rigidbody>();
        meleeAttack = new MeleeAttack();
        magicAttack = new MagicAttack();

        rb.useGravity = true;

        Collider capsuleCollider = GetComponent<Collider>();
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

        
        if (movement.magnitude >= 0.1f)
        {
            
            Vector3 moveDirection = transform.forward * movement.z + transform.right * movement.x;
            rb.MovePosition(rb.position + moveDirection * moveSpeed * Time.deltaTime);
        }
    }
    private void Jump()
    {
        if(Mathf.Abs(transform.position.y - 1f) < 0.1f && Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }


    }
    private void HandleAttack()
    {
        if (Input.GetMouseButtonDown(0))
        {
            meleeAttack.ExecuteAttack(transform);
            Debug.Log("melee");
        }
        else if (Input.GetMouseButtonDown(1)) 
        {
            magicAttack.ExecuteAttack(transform);
            Debug.Log("range");
        }
    }

    public void ReceiveDamage(int amount, DamageType type, float physicalResistance, float magicResistance)
    {
        int finalDamage = damageHandler.CalculateDamage(amount, type, physicalResistance, magicResistance);
        healthManager.TakeDamage(finalDamage);
    }

    
    public void DealDamage(Enemy target, int damage, DamageType type)
    {
        equippedWeapon?.Attack(target, damage, type);
    }

    
    public void EquipWeapon(Weapon weapon)
    {
        equippedWeapon = weapon;
    }
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"Collision detected with: {collision.gameObject.name}");
    }

}
