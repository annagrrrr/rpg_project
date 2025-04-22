using UnityEngine;

public class WeaponRaycastPickupProvider : IWeaponPickupProvider
{
    private readonly Transform _origin;

    public WeaponRaycastPickupProvider(Transform origin)
    {
        _origin = origin;
    }

    public IWeapon TryPickupWeapon()
    {
        Ray ray = new Ray(_origin.position, _origin.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, 2f))
        {
            var pickup = hit.collider.GetComponent<WeaponPickup>();
            if (pickup != null)
                return pickup.Pickup();
        }

        return null;
    }
}
