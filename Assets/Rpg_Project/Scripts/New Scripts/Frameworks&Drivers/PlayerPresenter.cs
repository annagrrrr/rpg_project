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
}