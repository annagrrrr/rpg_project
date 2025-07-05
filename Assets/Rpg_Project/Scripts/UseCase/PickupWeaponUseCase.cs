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
            if (weapon is MeleeWeapon)
            {
                _inventory.EquipRightHand(weapon);
                Debug.Log($"Подобрано ближнее оружие в правую руку: {weapon.Damage} урона");
            }
            else if (weapon is RangedWeapon)
            {
                _inventory.EquipLeftHand(weapon);
                Debug.Log($"Подобрано дальнобойное оружие в левую руку: {weapon.Damage} урона");
            }
            else
            {
                Debug.LogWarning("Неизвестный тип оружия!");
            }
        }
    }

}
