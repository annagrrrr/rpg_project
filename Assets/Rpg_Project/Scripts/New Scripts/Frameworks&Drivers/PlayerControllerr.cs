using UnityEngine;

public class PlayerControllerr : MonoBehaviour
{
    private MovePlayerUseCase _movePlayerUseCase;
    private AttackUseCase _attackUseCase;
    private IInputService _input;
    private WeaponInventory _inventory;
    private PickupWeaponUseCase _pickupWeaponUseCase;
    private void Start()
    {
        _inventory = new WeaponInventory();
        var sword = new MeleeWeapon(20);
        var staff = new RangedWeapon(15);
        _inventory.EquipRightHand(sword);
        _inventory.EquipLeftHand(staff);

        _input = new InputService();

        var repository = new InMemoryPlayerRepository();
        var presenter = new PlayerPresenter(transform);
        _movePlayerUseCase = new MovePlayerUseCase(repository, presenter);

        var attackPresenter = new AttackPresenter();
        _attackUseCase = new AttackUseCase(_inventory, attackPresenter);

        var pickupProvider = new WeaponRaycastPickupProvider(transform);
        _pickupWeaponUseCase = new PickupWeaponUseCase(pickupProvider, _inventory);
    }

    private void Update()
    {
        HandleMovement();
        HandleAttack();
        HandlePickup();
    }

    private void HandleMovement()
    {
        float horizontal = _input.GetAxis(PlayerInputAction.MoveHorizontal);
        float vertical = _input.GetAxis(PlayerInputAction.MoveVertical);
        _movePlayerUseCase.Execute(horizontal, vertical);
    }

    private void HandleAttack()
    {
        if (_input.GetActionDown(PlayerInputAction.PrimaryAttack))
        {
            _attackUseCase.ExecutePrimaryAttack();
        }

        if (_input.GetActionDown(PlayerInputAction.SecondaryAttack)) 
        {
            _attackUseCase.ExecuteSecondaryAttack();
        }
    }
    private void HandlePickup()
    {
        if (_input.GetActionDown(PlayerInputAction.Pickup))
        {
            _pickupWeaponUseCase.Execute();
        }
    }

}
