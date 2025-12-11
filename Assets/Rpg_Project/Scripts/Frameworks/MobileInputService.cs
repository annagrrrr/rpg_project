using UnityEngine;

public class MobileInputService : IInputService
{
    private DynamicJoystick _joystick;
    private MobileButton _jumpButton;
    private MobileButton _attackButton;
    private MobileButton _magicButton;
    private MobileButton _pickupButton;
    private MobileButton _sprintButton;

    public MobileInputService(DynamicJoystick joystick,
                             MobileButton jumpButton,
                             MobileButton attackButton,
                             MobileButton magicButton,
                             MobileButton pickupButton,
                             MobileButton sprintButton)
    {
        _joystick = joystick;
        _jumpButton = jumpButton;
        _attackButton = attackButton;
        _magicButton = magicButton;
        _pickupButton = pickupButton;
        _sprintButton = sprintButton;

        Debug.Log("🎮 MobileInputService initialized with 6 controls");

        LogButtonStatus("Joystick", _joystick != null);
        LogButtonStatus("Jump", _jumpButton != null);
        LogButtonStatus("Attack", _attackButton != null);
        LogButtonStatus("Magic", _magicButton != null);
        LogButtonStatus("Pickup", _pickupButton != null);
        LogButtonStatus("Sprint", _sprintButton != null);
    }

    private void LogButtonStatus(string name, bool isAssigned)
    {
        if (isAssigned)
            Debug.Log($"✅ {name} button assigned");
        else
            Debug.LogWarning($"⚠️ {name} button is NULL");
    }

    public float GetAxis(PlayerInputAction action)
    {
        if (_joystick == null) return 0f;

        switch (action)
        {
            case PlayerInputAction.MoveHorizontal:
                return _joystick.Horizontal;
            case PlayerInputAction.MoveVertical:
                return _joystick.Vertical;
        }
        return 0f;
    }

    public bool GetActionDown(PlayerInputAction action)
    {
        switch (action)
        {
            case PlayerInputAction.Jump:
                return CheckButtonPressed(_jumpButton, "🦘 Jump");

            case PlayerInputAction.PrimaryAttack:
                return CheckButtonPressed(_attackButton, "⚔️ Sword Attack");

            case PlayerInputAction.SecondaryAttack:
                return CheckButtonPressed(_magicButton, "🔮 Magic Attack");

            case PlayerInputAction.Pickup:
                return CheckButtonPressed(_pickupButton, "🎁 Pickup Weapon");

            case PlayerInputAction.Sprint:
                return CheckButtonPressed(_sprintButton, "💨 Sprint");
        }
        return false;
    }

    public bool GetAction(PlayerInputAction action)
    {
        switch (action)
        {
            case PlayerInputAction.Jump:
                return _jumpButton != null && _jumpButton.IsHeld;

            case PlayerInputAction.PrimaryAttack:
                return _attackButton != null && _attackButton.IsHeld;

            case PlayerInputAction.SecondaryAttack:
                return _magicButton != null && _magicButton.IsHeld;

            case PlayerInputAction.Pickup:
                return _pickupButton != null && _pickupButton.IsHeld;

            case PlayerInputAction.Sprint:
                return _sprintButton != null && _sprintButton.IsHeld;
        }
        return false;
    }

    private bool CheckButtonPressed(MobileButton button, string actionName)
    {
        if (button != null && button.WasPressed)
        {
            Debug.Log($"{actionName} button pressed!");
            return true;
        }
        return false;
    }
}