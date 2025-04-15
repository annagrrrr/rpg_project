using UnityEngine;

public class CameraRotationController : MonoBehaviour, ICameraRotation
{
    public float rotationSpeed = 5f;
    private float currentRotation = 0f;

    public void RotateCamera(float horizontalInput, float verticalInput)
    {
        currentRotation += horizontalInput * rotationSpeed;
        transform.rotation = Quaternion.Euler(0, currentRotation, 0);
    }
}
