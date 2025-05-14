using UnityEngine;

public class PlayerJumpPresenter : IPlayerJumpPresenter
{
    private readonly Rigidbody _rigidbody;

    public PlayerJumpPresenter(Rigidbody rigidbody)
    {
        _rigidbody = rigidbody;
    }

    public void Jump(float jumpForce)
    {
        _rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }
}
