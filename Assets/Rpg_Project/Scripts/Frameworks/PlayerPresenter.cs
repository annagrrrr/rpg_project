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
        if (direction == Vector3.zero)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
        _transform.rotation = Quaternion.Slerp(_transform.rotation, targetRotation, Time.deltaTime * 10f);
    }
}