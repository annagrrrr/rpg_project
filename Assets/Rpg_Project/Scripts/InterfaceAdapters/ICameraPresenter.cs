using UnityEngine;

public interface ICameraPresenter
{
    void SetPosition(Vector3 position);
    void LookAt(Vector3 target);
    Vector3 GetForward();
}
