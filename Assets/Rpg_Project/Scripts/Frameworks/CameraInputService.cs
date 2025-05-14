using UnityEngine;

public class CameraInputService : ICameraInputService
{
    public float GetMouseX() => Input.GetAxis("Mouse X");
    public float GetMouseY() => Input.GetAxis("Mouse Y");
}
