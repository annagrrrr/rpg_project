using UnityEngine;

public class WeaponView : MonoBehaviour
{
    [SerializeField] private WeaponType weaponType;
    [SerializeField] private AttackType attackType;
    [SerializeField] private int damage;

    public IWeapon CreateWeaponEntity()
    {
        switch (weaponType)
        {
            case WeaponType.Melee:
                return new MeleeWeapon(damage);
            case WeaponType.Ranged:
                return new RangedWeapon(damage);
            default:
                Debug.LogWarning("unknown weapon typee");
                return null;
        }
    }
}

public enum WeaponType
{
    Melee,
    Ranged
}
