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

        return false;
    }
}
