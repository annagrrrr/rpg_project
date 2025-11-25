using UnityEngine;

public class AttackUseCase
{
    private readonly WeaponInventory _inventory;
    private readonly IAttackPresenter _attackPresenter;
    private readonly IPlayerAnimationPresenter _animator;
    private readonly Transform _playerTransform;
    private readonly IAttackCooldownPresenter _cooldownPresenter;
    private readonly LayerMask _enemyLayer;

    private readonly AddDamageDealtUseCase _addDamageDealtUseCase;

    private float _primaryCooldown = 1f;
    private float _secondaryCooldown = 5f;

    private float _nextPrimaryTime = 0f;
    private float _nextSecondaryTime = 0f;

    public AttackUseCase(
        WeaponInventory inventory,
        IAttackPresenter attackPresenter,
        Transform playerTransform,
        IPlayerAnimationPresenter animator,
        IAttackCooldownPresenter cooldownPresenter,
        AddDamageDealtUseCase addDamageDealtUseCase)
    {
        _inventory = inventory;
        _attackPresenter = attackPresenter;
        _animator = animator;
        _playerTransform = playerTransform;
        _cooldownPresenter = cooldownPresenter;
        _enemyLayer = LayerMask.GetMask("Enemy");

        _addDamageDealtUseCase = addDamageDealtUseCase;
    }

    public void ExecutePrimaryAttack()
    {
        if (Time.time < _nextPrimaryTime) return;

        var weapon = _inventory.GetRightHandWeapon();
        if (weapon == null) return;

        _attackPresenter.ShowAttack(weapon.AttackType);
        _animator.PlayAttackAnimation(weapon.AttackType);
        AttemptHit(weapon);

        _nextPrimaryTime = Time.time + _primaryCooldown;
    }

    public void ExecuteSecondaryAttack()
    {
        if (Time.time < _nextSecondaryTime) return;

        var weapon = _inventory.GetLeftHandWeapon();
        if (weapon == null) return;

        _attackPresenter.ShowAttack(weapon.AttackType);
        _animator.PlayAttackAnimation(weapon.AttackType);
        AttemptHit(weapon);

        _nextSecondaryTime = Time.time + _secondaryCooldown;
    }

    private void AttemptHit(IWeapon weapon)
    {
        Vector3 origin = _playerTransform.position;
        Vector3 dir = _playerTransform.forward;

        RaycastHit[] hits = Physics.SphereCastAll(origin, 1f, dir, 2f, _enemyLayer);

        foreach (var hit in hits)
        {
            if (hit.collider.TryGetComponent(out IEnemyHealth enemy))
            {
                enemy.ReceiveDamage(weapon.Damage, weapon.AttackType);
                _addDamageDealtUseCase.Execute(weapon.Damage);
            }
        }
    }
}
