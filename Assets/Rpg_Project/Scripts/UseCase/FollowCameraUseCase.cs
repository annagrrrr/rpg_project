using UnityEngine;

public class FollowCameraUseCase
{
    private readonly ICameraInputService _input;
    private readonly CameraPresenter _presenter;
    private Transform _target;
    private readonly CameraSettings _settings;

    private float _yaw;
    private float _pitch;

    private bool _isCameraLocked = false;
    private float _lockTimer = 0f;
    private bool _useMobileInput = false;

    public FollowCameraUseCase(ICameraInputService input, CameraPresenter presenter, Transform target, CameraSettings settings)
    {
        _input = input;
        _presenter = presenter;
        _target = target;
        _settings = settings;

        _yaw = target.eulerAngles.y;
        _pitch = 20f; 

#if UNITY_ANDROID || UNITY_IOS
        _useMobileInput = true;
#endif
    }

    public void Tick()
    {
        if (_isCameraLocked)
        {
            _lockTimer -= Time.deltaTime;
            if (_lockTimer <= 0f)
            {
                _isCameraLocked = false;
            }

            UpdateCameraPosition(false);
            return;
        }

        float mouseX = _input.GetMouseX();
        float mouseY = _input.GetMouseY();

        if (_useMobileInput && Mathf.Approximately(mouseX, 0f) && Mathf.Approximately(mouseY, 0f))
        {
            UpdateCameraPosition(true);
            return;
        }

        _yaw += mouseX * _settings.Sensitivity;
        _pitch -= mouseY * _settings.Sensitivity;
        _pitch = Mathf.Clamp(_pitch, _settings.MinPitch, _settings.MaxPitch);

        UpdateCameraPosition(true);
    }

    private void UpdateCameraPosition(bool lookAtTarget)
    {
        Quaternion rotation = Quaternion.Euler(_pitch, _yaw, 0);

        Vector3 desiredPosition = _target.position + rotation * _settings.Offset;

        Vector3 direction = desiredPosition - _target.position;
        if (Physics.Raycast(_target.position, direction.normalized, out RaycastHit hit, _settings.Distance, _settings.CollisionMask))
        {
            desiredPosition = hit.point - direction.normalized * 0.3f;
        }

        _presenter.SetPosition(desiredPosition);

        if (lookAtTarget)
        {
            _presenter.LookAt(_target.position + Vector3.up * 1.5f);
        }
    }

    public void LockCameraForDuration(float duration)
    {
        _isCameraLocked = true;
        _lockTimer = duration;
        Debug.Log($"Camera locked for {duration} seconds");
    }

    public void ResetCameraToDefault()
    {
        _yaw = _target.eulerAngles.y;
        _pitch = 20f; 
    }

    public void UpdateTarget(Transform newTarget)
    {
        _target = newTarget;
        ResetCameraToDefault();
    }

    public bool IsCameraLocked() => _isCameraLocked;
    public float GetCurrentYaw() => _yaw;
    public float GetCurrentPitch() => _pitch;
}