using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int damage;
    public DamageType damageType;

    private Collider weaponCollider;
    private GameObject owner;

    private void Awake()
    {
        weaponCollider = GetComponent<Collider>();
        weaponCollider.enabled = false;
    }

    public void SetOwner(GameObject weaponOwner)
    {
        owner = weaponOwner;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == owner) return;

        bool ownerIsEnemy = owner.GetComponent<Enemy>() != null;
        bool ownerIsPlayer = owner.GetComponent<PlayerController>() != null;

        bool targetIsEnemy = other.GetComponent<Enemy>() != null;
        bool targetIsPlayer = other.GetComponent<PlayerController>() != null;

        if (ownerIsEnemy && targetIsEnemy) return;

        if (targetIsEnemy)
        {
            other.GetComponent<Enemy>().TakeDamage(damage, damageType);
        }

        if (targetIsPlayer)
        {
            other.GetComponent<PlayerController>().ReceiveDamage(damage, damageType, 0, 0);
        }
    }

    public void ActivateWeapon()
    {
        weaponCollider.enabled = true;
    }

    public void DeactivateWeapon()
    {
        weaponCollider.enabled = false;
    }


    //TEMP
    public void Attack(Enemy target, int damage, DamageType type)
    {
        target.TakeDamage(damage, type);
    }
    //TEMP
}
