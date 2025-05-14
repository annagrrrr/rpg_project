using UnityEngine;

public class CameraPresenter : ICameraPresenter
{
    private readonly Transform _cameraTransform;

    public CameraPresenter(Transform cameraTransform)
    {
        _cameraTransform = cameraTransform;
    }

    public void SetPosition(Vector3 position)
    {
        _cameraTransform.position = position;
    }

    public void LookAt(Vector3 target)
    {
        _cameraTransform.LookAt(target);
    }

    public Vector3 GetForward()
    {
        return _cameraTransform.forward;
    }
}
