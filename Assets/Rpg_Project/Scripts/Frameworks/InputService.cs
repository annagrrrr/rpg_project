using UnityEngine;
using System.Collections.Generic;

public class InputService : IInputService
{
    private readonly Dictionary<PlayerInputAction, string> _axisBindings = new()
    {
        { PlayerInputAction.MoveHorizontal, "Horizontal" },
        { PlayerInputAction.MoveVertical, "Vertical" }
    };

    private readonly Dictionary<PlayerInputAction, int> _buttonBindings = new()
    {
        { PlayerInputAction.PrimaryAttack, 0 },
        { PlayerInputAction.SecondaryAttack, 1 }
    };

    private readonly Dictionary<PlayerInputAction, KeyCode> _keyBindings = new()
{
    { PlayerInputAction.Pickup, KeyCode.E },
    { PlayerInputAction.Jump, KeyCode.Space },
    { PlayerInputAction.Sprint, KeyCode.LeftShift }
};

    public float GetAxis(PlayerInputAction action)
    {
        if (_axisBindings.TryGetValue(action, out var axisName))
            return Input.GetAxis(axisName);

        return 0f;
    }

    public bool GetActionDown(PlayerInputAction action)
    {
        if (_buttonBindings.TryGetValue(action, out var mouseButton))
            return Input.GetMouseButtonDown(mouseButton);

        if (_keyBindings.TryGetValue(action, out var keyCode))
            return Input.GetKeyDown(keyCode);

        return false;
    }
    public bool GetAction(PlayerInputAction action)
    {
        if (_buttonBindings.TryGetValue(action, out var mouseButton))
            return Input.GetMouseButton(mouseButton);

        if (_keyBindings.TryGetValue(action, out var keyCode))
            return Input.GetKey(keyCode);

        return false;
    }

}
