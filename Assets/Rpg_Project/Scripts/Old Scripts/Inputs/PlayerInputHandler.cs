using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{
    public bool IsMeleeAttackPressed() => Input.GetMouseButtonDown(0);
    public bool IsMagicAttackPressed() => Input.GetMouseButtonDown(1);
    public bool IsJumpPressed() => Input.GetKeyDown(KeyCode.Space);

    public Vector2 GetMovementInput() => new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    public Vector2 GetMouseDelta() => new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
}
