using UnityEngine;

public class PlayerControllerr : MonoBehaviour
{
    private MovePlayerUseCase _movePlayerUseCase;
    private AttackUseCase _attackUseCase;
    private IInputService _input;
    private WeaponInventory _inventory;
    private PickupWeaponUseCase _pickupWeaponUseCase;
    private JumpUseCase _jumpUseCase;
    private PlayerHealthPresenter _healthPresenter;

    [SerializeField] private float jumpForce;

    private void Start()
    {
    }

    public void Initialize(
        IInputService input,
        MovePlayerUseCase movePlayerUseCase,
        AttackUseCase attackUseCase,
        PickupWeaponUseCase pickupWeaponUseCase,
        JumpUseCase jumpUseCase,
        WeaponInventory inventory,
        PlayerHealthPresenter healthPresenter)
    {
        _input = input;
        _movePlayerUseCase = movePlayerUseCase;
        _attackUseCase = attackUseCase;
        _pickupWeaponUseCase = pickupWeaponUseCase;
        _jumpUseCase = jumpUseCase;
        _inventory = inventory;
        _healthPresenter = healthPresenter;
    }

    private void Update()
    {
        HandleMovement();
        HandleAttack();
        HandlePickup();
        HandleJump();
    }

    private void HandleMovement()
    {
        float horizontal = _input.GetAxis(PlayerInputAction.MoveHorizontal);
        float vertical = _input.GetAxis(PlayerInputAction.MoveVertical);
        bool isSprinting = _input.GetAction(PlayerInputAction.Sprint);

        _movePlayerUseCase.Execute(horizontal, vertical, isSprinting);
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

    private void HandleJump()
    {
        if (_input.GetActionDown(PlayerInputAction.Jump))
        {
            _jumpUseCase.Execute();
            Debug.Log("jumpy");
        }
    }
}
