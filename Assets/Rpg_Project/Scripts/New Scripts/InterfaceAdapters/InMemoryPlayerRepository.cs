public class InMemoryPlayerRepository : IPlayerRepository
{
    private Player _player;

    public InMemoryPlayerRepository()
    {
        _player = new Player(5f); // ������� ��������
    }

    public Player GetPlayer()
    {
        return _player;
    }
}