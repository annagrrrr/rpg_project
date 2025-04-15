using UnityEngine;

public class PickupWeaponUseCase
{
    private readonly IWeaponPickupProvider _pickupProvider;
    private readonly WeaponInventory _inventory;

    public PickupWeaponUseCase(IWeaponPickupProvider pickupProvider, WeaponInventory inventory)
    {
        _pickupProvider = pickupProvider;
        _inventory = inventory;
    }

    public void Execute()
    {
        IWeapon weapon = _pickupProvider.TryPickupWeapon();
        if (weapon != null)
        {
            _inventory.EquipRightHand(weapon);
            Debug.Log($"Оружие подобрано: {weapon.AttackType} с уроном {weapon.Damage}");
        }
    }
}
