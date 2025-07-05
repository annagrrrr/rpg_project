using UnityEngine;

public class WeaponTriggerPickupProvider : MonoBehaviour, IWeaponPickupProvider
{
    private WeaponPickup _currentWeaponPickup;

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Weapon"))
        {
            _currentWeaponPickup = other.GetComponent<WeaponPickup>();
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Weapon"))
        {
            if (_currentWeaponPickup != null && other.GetComponent<WeaponPickup>() == _currentWeaponPickup)
            {
                _currentWeaponPickup = null;
            }
        }
    }

    public IWeapon TryPickupWeapon()
    {
        if (_currentWeaponPickup != null)
        {
            return _currentWeaponPickup.Pickup();
        }

        return null;
    }
}
