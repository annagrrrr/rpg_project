using UnityEngine;

public class AttackUseCase
{
    private readonly WeaponInventory _inventory;
    private readonly IAttackPresenter _attackPresenter;
    private readonly float _attackRange = 2.0f;
    private readonly LayerMask _enemyLayer;
    private readonly Transform _playerTransform;

    public AttackUseCase(WeaponInventory inventory, IAttackPresenter attackPresenter, Transform playerTransform)
    {
        _inventory = inventory;
        _attackPresenter = attackPresenter;
        _enemyLayer = LayerMask.GetMask("Enemy");
        _playerTransform = playerTransform;
    }

    public void ExecutePrimaryAttack()
    {
        var weapon = _inventory.GetRightHandWeapon();
        if (weapon == null)
        {
            Debug.LogWarning("No weapon in right hand!");
            return;
        }

        _attackPresenter.ShowAttack(weapon.AttackType);
        AttemptHit(weapon);
    }

    public void ExecuteSecondaryAttack()
    {
        var weapon = _inventory.GetLeftHandWeapon();
        if (weapon == null)
        {
            Debug.LogWarning("No weapon in left hand!");
            return;
        }

        _attackPresenter.ShowAttack(weapon.AttackType);
        AttemptHit(weapon);
    }

    private void AttemptHit(IWeapon weapon)
    {
        if (_playerTransform == null)
        {
            Debug.LogWarning("Player transform is not assigned!");
            return;
        }

        Vector3 attackOrigin = _playerTransform.position; 
        Vector3 attackDirection = _playerTransform.forward;

        RaycastHit[] hits = Physics.SphereCastAll(attackOrigin, 1f, attackDirection, _attackRange, _enemyLayer);

        foreach (var hit in hits)
        {
            if (hit.collider.TryGetComponent(out EnemyHealthPresenter enemyHealth))
            {
                enemyHealth.ReceiveDamage(weapon.Damage);
                Debug.Log($"Hit enemy! Dealt {weapon.Damage} damage.");
            }
        }
    }
}
