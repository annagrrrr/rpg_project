using UnityEngine;

[RequireComponent(typeof(WeaponView))]
public class WeaponPickup : MonoBehaviour
{
    private WeaponView _weaponView;

    private void Awake()
    {
        _weaponView = GetComponent<WeaponView>();
    }

    public IWeapon Pickup()
    {
        var weapon = _weaponView.CreateWeaponEntity();
        Destroy(gameObject);
        return weapon;
    }
}
