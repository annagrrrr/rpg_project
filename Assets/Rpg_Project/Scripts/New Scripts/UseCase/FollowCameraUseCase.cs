using UnityEngine;

public class FollowCameraUseCase
{
    private readonly ICameraInputService _input;
    private readonly CameraPresenter _presenter;
    private readonly Transform _target;
    private readonly CameraSettings _settings;

    private float _yaw;
    private float _pitch;

    public FollowCameraUseCase(ICameraInputService input, CameraPresenter presenter, Transform target, CameraSettings settings)
    {
        _input = input;
        _presenter = presenter;
        _target = target;
        _settings = settings;
    }

    public void Tick()
    {
        _yaw += _input.GetMouseX() * _settings.Sensitivity;
        _pitch -= _input.GetMouseY() * _settings.Sensitivity;
        _pitch = Mathf.Clamp(_pitch, _settings.MinPitch, _settings.MaxPitch);

        Quaternion rotation = Quaternion.Euler(_pitch, _yaw, 0);
        Vector3 desiredPosition = _target.position + rotation * _settings.Offset;

        Vector3 direction = desiredPosition - _target.position;
        if (Physics.Raycast(_target.position, direction.normalized, out RaycastHit hit, _settings.Distance))
        {
            desiredPosition = hit.point - direction.normalized * 0.3f;
        }

        _presenter.SetPosition(desiredPosition);
        _presenter.LookAt(_target.position + Vector3.up * 1.5f);
    }
}
