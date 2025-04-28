public interface IInputService
{
    float GetAxis(PlayerInputAction action);
    bool GetActionDown(PlayerInputAction action);
    bool GetAction(PlayerInputAction action);

}
