using UnityEngine;

public class MovePlayerUseCase
{
    private readonly IPlayerRepository _repository;
    private readonly IPlayerPresenter _presenter;
    private readonly ICameraPresenter _camera;
    private readonly IPlayerAnimationPresenter _animator;

    public MovePlayerUseCase(IPlayerRepository repository, IPlayerPresenter presenter, ICameraPresenter camera, IPlayerAnimationPresenter animator)
    {
        _repository = repository;
        _presenter = presenter;
        _camera = camera;
        _animator = animator;
    }

    public void Execute(float horizontalInput, float verticalInput, bool isSprinting)
    {
        var player = _repository.GetPlayer();
        var input = new Vector3(horizontalInput, 0, verticalInput);

        var cameraForward = _camera.GetForward();
        cameraForward.y = 0;
        cameraForward.Normalize();

        var cameraRight = Vector3.Cross(Vector3.up, cameraForward);
        var moveDir = (cameraForward * input.z + cameraRight * input.x).normalized;

        float speed = player.Speed;
        if (isSprinting)
        {
            speed *= player.SprintMultiplier;
        }

        _presenter.Move(moveDir * speed * Time.deltaTime);

        if (moveDir.magnitude > 0.01f)
        {
            _presenter.RotateTowards(moveDir);
            _animator.PlayRunAnimation(true);
        }
        else
        {
            _animator.PlayRunAnimation(false);
        }
    }


}
