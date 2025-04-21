public class MovePlayerUseCase
{
    private readonly IPlayerRepository _repository;
    private readonly IPlayerPresenter _presenter;

    public MovePlayerUseCase(IPlayerRepository repository, IPlayerPresenter presenter)
    {
        _repository = repository;
        _presenter = presenter;
    }

    public void Execute(float horizontalInput, float verticalInput)
    {
        var player = _repository.GetPlayer();
        var direction = new UnityEngine.Vector3(horizontalInput, 0, verticalInput);
        _presenter.Move(direction.normalized * player.Speed * UnityEngine.Time.deltaTime);
    }
}