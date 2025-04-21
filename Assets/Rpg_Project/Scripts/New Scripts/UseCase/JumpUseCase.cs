using UnityEngine;

public class JumpUseCase
{
    private readonly IPlayerJumpPresenter _presenter;
    private readonly IPlayerGroundChecker _groundChecker;
    private readonly float _jumpForce;

    public JumpUseCase(IPlayerJumpPresenter presenter, IPlayerGroundChecker groundChecker, float jumpForce)
    {
        _presenter = presenter;
        _groundChecker = groundChecker;
        _jumpForce = jumpForce;
    }

    public void Execute()
    {
        if (_groundChecker.IsGrounded())
        {
            _presenter.Jump(_jumpForce);
        }
    }
}
