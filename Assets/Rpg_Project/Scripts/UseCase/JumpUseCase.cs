using UnityEngine;

public class JumpUseCase
{
    private readonly PlayerJumpPresenter _jumpPresenter;
    private readonly IPlayerGroundChecker _groundChecker;
    private readonly float _jumpForce;
    private readonly IPlayerAnimationPresenter _animatorPresenter;
    private readonly FollowCameraUseCase _cameraUseCase;

    public JumpUseCase(
        PlayerJumpPresenter jumpPresenter,
        IPlayerGroundChecker groundChecker,
        float jumpForce,
        IPlayerAnimationPresenter animatorPresenter,
        FollowCameraUseCase cameraUseCase = null) // Опционально для обратной совместимости
    {
        _jumpPresenter = jumpPresenter;
        _groundChecker = groundChecker;
        _jumpForce = jumpForce;
        _animatorPresenter = animatorPresenter;
        _cameraUseCase = cameraUseCase;
    }

    public void Execute()
    {
        if (!_groundChecker.IsGrounded())
        {
            Debug.Log("Cannot jump - not grounded");
            return;
        }

        // Выполняем прыжок
        _jumpPresenter.Jump(_jumpForce);
        _animatorPresenter.PlayJumpAnimation();

        // Блокируем вращение камеры на 0.5 секунды при прыжке
        // чтобы камера не дергалась
        if (_cameraUseCase != null)
        {
            _cameraUseCase.LockCameraForDuration(0.5f);
        }

        Debug.Log($"Player jumped with force {_jumpForce}");
    }

    // Альтернативный конструктор без камеры (для обратной совместимости)
    public JumpUseCase(
        PlayerJumpPresenter jumpPresenter,
        IPlayerGroundChecker groundChecker,
        float jumpForce,
        IPlayerAnimationPresenter animatorPresenter)
        : this(jumpPresenter, groundChecker, jumpForce, animatorPresenter, null)
    {
    }
}