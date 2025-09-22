using UnityEngine;
using System.Collections.Generic;

public class MobileInputService : IInputService
{
    private VirtualJoystick _joystick;
    private MobileButton _jumpButton;
    private MobileButton _primaryAttackButton;
    private MobileButton _secondaryAttackButton;
    private MobileButton _pickupButton;
    private MobileButton _sprintButton;

    public MobileInputService(
        VirtualJoystick joystick,
        MobileButton jumpButton,
        MobileButton primaryAttackButton,
        MobileButton secondaryAttackButton,
        MobileButton pickupButton,
        MobileButton sprintButton)
    {
        _joystick = joystick;
        _jumpButton = jumpButton;
        _primaryAttackButton = primaryAttackButton;
        _secondaryAttackButton = secondaryAttackButton;
        _pickupButton = pickupButton;
        _sprintButton = sprintButton;
    }

    public float GetAxis(PlayerInputAction action)
    {
        switch (action)
        {
            case PlayerInputAction.MoveHorizontal: return _joystick.Horizontal;
            case PlayerInputAction.MoveVertical: return _joystick.Vertical;
        }
        return 0f;
    }

    public bool GetActionDown(PlayerInputAction action)
    {
        switch (action)
        {
            case PlayerInputAction.Jump: return _jumpButton.WasPressed;
            case PlayerInputAction.PrimaryAttack: return _primaryAttackButton.WasPressed;
            case PlayerInputAction.SecondaryAttack: return _secondaryAttackButton.WasPressed;
            case PlayerInputAction.Pickup: return _pickupButton.WasPressed;
            case PlayerInputAction.Sprint: return _sprintButton.WasPressed;
        }
        return false;
    }

    public bool GetAction(PlayerInputAction action)
    {
        switch (action)
        {
            case PlayerInputAction.Jump: return _jumpButton.IsHeld;
            case PlayerInputAction.PrimaryAttack: return _primaryAttackButton.IsHeld;
            case PlayerInputAction.SecondaryAttack: return _secondaryAttackButton.IsHeld;
            case PlayerInputAction.Pickup: return _pickupButton.IsHeld;
            case PlayerInputAction.Sprint: return _sprintButton.IsHeld;
        }
        return false;
    }
}
