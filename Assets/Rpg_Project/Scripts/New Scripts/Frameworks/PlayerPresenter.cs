using UnityEngine;

public class PlayerPresenter : IPlayerPresenter
{
    private readonly Transform _transform;

    public PlayerPresenter(Transform transform)
    {
        _transform = transform;
    }

    public void Move(Vector3 movement)
    {
        _transform.position += movement;
    }

    public void RotateTowards(Vector3 direction)
    {
        if (direction.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            _transform.rotation = Quaternion.Slerp(_transform.rotation, targetRotation, 10f * Time.deltaTime);
        }
    }
}