using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int damage;
    public DamageType damageType;
    public IAttack attackType;
    private GameObject owner;
    private PlayerInputHandler inputHandler;
    private void Awake()
    {
        inputHandler = GameObject.FindWithTag("Player").GetComponent<PlayerInputHandler>();
        if (attackType == null)
        {
            if (damageType == DamageType.PHYSICAL)
            {
                attackType = new MeleeAttack();
            }
            else if (damageType == DamageType.MAGICAL)
            {
                attackType = new MagicAttack();
            }
        }
    }

    public void SetOwner(GameObject weaponOwner)
    {
        owner = weaponOwner;
    }

    public void PerformAttack()
    {
        if (inputHandler != null)
        {
            if (inputHandler.IsMeleeAttackPressed()) 
            {
                if (attackType != null && attackType is MeleeAttack)
                {
                    attackType.ExecuteAttack(owner.transform, damage); 
                }
                else
                {
                    Debug.LogWarning("Weapon is not melee type!");
                }
            }
            else if (inputHandler.IsMagicAttackPressed()) 
            {
                if (attackType != null && attackType is MagicAttack)
                {
                    attackType.ExecuteAttack(owner.transform, damage); 
                }
                else
                {
                    Debug.LogWarning("Weapon is not magic type!");
                }
            }
        }
    }
}
