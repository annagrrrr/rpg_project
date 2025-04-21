using UnityEngine;

public class AttackUseCase
{
    private readonly WeaponInventory _inventory;
    private readonly IAttackPresenter _attackPresenter;

    public AttackUseCase(WeaponInventory inventory, IAttackPresenter attackPresenter)
    {
        _inventory = inventory;
        _attackPresenter = attackPresenter;
    }

    public void ExecutePrimaryAttack()
    {
        var weapon = _inventory.GetRightHandWeapon();
        if (weapon == null)
        {
            Debug.LogWarning("no weapon right hand");
            return;
        }

        _attackPresenter.ShowAttack(weapon.AttackType);
        // later: calculate dmg, apply to tagret
    }

    public void ExecuteSecondaryAttack()
    {
        var weapon = _inventory.GetLeftHandWeapon();
        if (weapon == null)
        {
            Debug.LogWarning("no weapon left hand");
            return;
        }

        _attackPresenter.ShowAttack(weapon.AttackType);
        // later: calculate dmg, apply to tagret
    }
}
