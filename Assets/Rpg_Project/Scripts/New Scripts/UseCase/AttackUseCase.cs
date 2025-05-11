using UnityEngine;

public class AttackUseCase
{
    private readonly WeaponInventory _inventory;
    private readonly IAttackPresenter _attackPresenter;
    private readonly IPlayerAnimationPresenter _animator;
    private readonly float _attackRange = 2.0f;
    private readonly LayerMask _enemyLayer;
    private readonly Transform _playerTransform;

    private float _primaryAttackCooldown = 1f;
    private float _secondaryAttackCooldown = 5f;

    private float _nextPrimaryAttackTime = 0f;
    private float _nextSecondaryAttackTime = 0f;

    private readonly IAttackCooldownPresenter _cooldownPresenter;

    public AttackUseCase(
        WeaponInventory inventory,
        IAttackPresenter attackPresenter,
        Transform playerTransform,
        IPlayerAnimationPresenter animator,
        IAttackCooldownPresenter cooldownPresenter)
    {
        _inventory = inventory;
        _attackPresenter = attackPresenter;
        _enemyLayer = LayerMask.GetMask("Enemy");
        _playerTransform = playerTransform;
        _animator = animator;
        _cooldownPresenter = cooldownPresenter;
    }

    public void ExecutePrimaryAttack()
    {
        if (Time.time < _nextPrimaryAttackTime)
        {
            Debug.Log("Primary attack is on cooldown!");
            return;
        }

        var weapon = _inventory.GetRightHandWeapon();
        if (weapon == null)
        {
            Debug.LogWarning("No weapon in right hand!");
            return;
        }

        _attackPresenter.ShowAttack(weapon.AttackType);
        _animator.PlayAttackAnimation(weapon.AttackType);
        AttemptHit(weapon);

        _nextPrimaryAttackTime = Time.time + _primaryAttackCooldown;
    }

    public void ExecuteSecondaryAttack()
    {
        if (Time.time < _nextSecondaryAttackTime)
        {
            Debug.Log("Secondary attack is on cooldown!");
            return;
        }

        var weapon = _inventory.GetLeftHandWeapon();
        if (weapon == null)
        {
            Debug.LogWarning("No weapon in left hand!");
            return;
        }

        _attackPresenter.ShowAttack(weapon.AttackType);
        _animator.PlayAttackAnimation(weapon.AttackType);
        AttemptHit(weapon);

        _nextSecondaryAttackTime = Time.time + _secondaryAttackCooldown;
        _cooldownPresenter.UpdateSecondaryCooldown(_secondaryAttackCooldown, _secondaryAttackCooldown);
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

        RaycastHit[] hits = Physics.SphereCastAll(
            attackOrigin, 1f, attackDirection, _attackRange, _enemyLayer);

        foreach (var hit in hits)
        {
            if (hit.collider.TryGetComponent(out IEnemyHealth enemyHealth))
            {
                enemyHealth.ReceiveDamage(weapon.Damage);
                Debug.Log($"Hit enemy! Dealt {weapon.Damage} damage.");
            }

            if (hit.collider.TryGetComponent(out IStunnable stunnable))
            {
                var stunUseCase = new ApplyStunUseCase(stunnable);
                stunUseCase.Execute(1f);
                Debug.Log("Applied stun to enemy.");
            }
        }
    }
    public void UpdateCooldowns()
    {
        float timeLeft = Mathf.Max(0, _nextSecondaryAttackTime - Time.time);
        _cooldownPresenter.UpdateSecondaryCooldown(timeLeft, _secondaryAttackCooldown);
    }

}
