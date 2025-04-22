using UnityEngine;

public class PlayerRotationPresenter : IRotationPresenter
{
    private readonly Transform _transform;
    private readonly float _rotationSpeed;

    public PlayerRotationPresenter(Transform transform, float rotationSpeed = 10f)
    {
        _transform = transform;
        _rotationSpeed = rotationSpeed;
    }

    public void RotateTowards(Vector3 direction)
    {
        if (direction.sqrMagnitude < 0.001f)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        _transform.rotation = Quaternion.Slerp(_transform.rotation, targetRotation, Time.deltaTime * _rotationSpeed);
    }
}
