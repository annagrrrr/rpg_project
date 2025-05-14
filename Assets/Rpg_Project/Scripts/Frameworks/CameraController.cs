using UnityEngine;

public class CameraController : MonoBehaviour
{
    private FollowCameraUseCase _cameraUseCase;

    public void Initialize(FollowCameraUseCase useCase)
    {
        _cameraUseCase = useCase;
    }

    private void LateUpdate()
    {
        _cameraUseCase?.Tick();
    }
}
