using UnityEngine;

public class MobileInputService : IInputService
{
    private DynamicJoystick _joystick;
    private MobileButton _jumpButton;

    public MobileInputService(DynamicJoystick joystick, MobileButton jumpButton)
    {
        _joystick = joystick;
        _jumpButton = jumpButton;

        if (_joystick == null)
        {
            Debug.LogError("❌ DynamicJoystick is NULL in MobileInputService constructor!");
        }
        else
        {
            Debug.Log("✅ MobileInputService initialized with DynamicJoystick");
        }

        if (_jumpButton == null)
        {
            Debug.LogWarning("⚠️ JumpButton is NULL - прыжок не будет работать");
        }
        else
        {
            Debug.Log("✅ JumpButton assigned to MobileInputService");
        }
    }

    public float GetAxis(PlayerInputAction action)
    {
        if (_joystick == null)
        {
            return 0f;
        }

        float value = 0f;
        switch (action)
        {
            case PlayerInputAction.MoveHorizontal:
                value = _joystick.Horizontal;
                break;
            case PlayerInputAction.MoveVertical:
                value = _joystick.Vertical;
                break;
        }

        return value;
    }

    public bool GetActionDown(PlayerInputAction action)
    {
        switch (action)
        {
            case PlayerInputAction.Jump:
                if (_jumpButton != null && _jumpButton.WasPressed)
                {
                    Debug.Log("🦘 Jump button pressed!");
                    return true;
                }
                return false;

            case PlayerInputAction.PrimaryAttack:
            case PlayerInputAction.SecondaryAttack:
            case PlayerInputAction.Pickup:
            case PlayerInputAction.Sprint:
                return false; // Пока заглушки
            default:
                return false;
        }
    }

    public bool GetAction(PlayerInputAction action)
    {
        switch (action)
        {
            case PlayerInputAction.Jump:
                return _jumpButton != null && _jumpButton.IsHeld;

            case PlayerInputAction.PrimaryAttack:
            case PlayerInputAction.SecondaryAttack:
            case PlayerInputAction.Pickup:
            case PlayerInputAction.Sprint:
                return false; // Пока заглушки
            default:
                return false;
        }
    }
}