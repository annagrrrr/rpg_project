using UnityEngine;

public class MovePlayerUseCase
{
    private readonly IPlayerRepository _repository;
    private readonly IPlayerPresenter _presenter;
    private readonly ICameraPresenter _camera;

    public MovePlayerUseCase(IPlayerRepository repository, IPlayerPresenter presenter, ICameraPresenter camera)
    {
        _repository = repository;
        _presenter = presenter;
        _camera = camera;
    }

    public void Execute(float horizontalInput, float verticalInput)
    {
        var player = _repository.GetPlayer();
        var input = new Vector3(horizontalInput, 0, verticalInput);

        var cameraForward = _camera.GetForward();
        cameraForward.y = 0;
        cameraForward.Normalize();

        var cameraRight = Vector3.Cross(Vector3.up, cameraForward);
        var moveDir = (cameraForward * input.z + cameraRight * input.x).normalized;

        _presenter.Move(moveDir * player.Speed * Time.deltaTime);
    }
}
