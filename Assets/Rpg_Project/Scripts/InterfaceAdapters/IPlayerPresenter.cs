using UnityEngine;

public interface IPlayerPresenter
{
    void Move(Vector3 movement);
    void RotateTowards(Vector3 direction);
}