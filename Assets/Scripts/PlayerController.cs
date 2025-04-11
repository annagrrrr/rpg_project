using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public HealthManager healthManager; 
    public DamageHandler damageHandler; 
<<<<<<< HEAD
    public MovementController movementController;
    //public Weapon equippedWeapon; 
    public Weapon meleeWeapon;
    public Weapon magicWeapon;
=======
    public MovementController movementController; 
    public Weapon equippedWeapon; 
>>>>>>> 762a2a4 (PlayerController)
    public int Mana { get; private set; } 
    public int PhysicalAttack { get; private set; } 
    public int MagicAttack { get; private set; } 

    private Rigidbody rb; 
    public float moveSpeed = 5f;
    private bool isGrounded;
    public float jumpForce = 5f;
    public LayerMask groundLayer;
    private Collider capsuleCollider;
<<<<<<< HEAD
    private IAttack meleeAttack;
    private IAttack magicAttack;
=======
>>>>>>> 762a2a4 (PlayerController)

    private void Start()
    {
        
        healthManager = GetComponent<HealthManager>();
        damageHandler = GetComponent<DamageHandler>();
        movementController = GetComponent<MovementController>();
        rb = GetComponent<Rigidbody>();
<<<<<<< HEAD
        meleeAttack = new MeleeAttack();
        magicAttack = new MagicAttack();

=======

        
>>>>>>> 762a2a4 (PlayerController)
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
<<<<<<< HEAD
        HandleAttack();
        
=======
>>>>>>> 762a2a4 (PlayerController)
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
<<<<<<< HEAD
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
                Debug.LogWarning("��� �������� ������!");
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
                Debug.LogWarning("��� ����������� ������!");
            }
        }
    }
=======
>>>>>>> 762a2a4 (PlayerController)

    public void ReceiveDamage(int amount, DamageType type, float physicalResistance, float magicResistance)
    {
        int finalDamage = damageHandler.CalculateDamage(amount, type, physicalResistance, magicResistance);
        healthManager.TakeDamage(finalDamage);
    }

<<<<<<< HEAD

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
            Debug.LogWarning("��� ����������� ������ ��� �����!");
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
=======
    
    public void DealDamage(Enemy target, int damage, DamageType type)
    {
        equippedWeapon?.Attack(target, damage, type);
    }

    
    public void EquipWeapon(Weapon weapon)
    {
        equippedWeapon = weapon;
>>>>>>> 762a2a4 (PlayerController)
    }
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"Collision detected with: {collision.gameObject.name}");
    }

}
