using UnityEngine;

public class PlayerHealthController : MonoBehaviour, IPlayerTarget
{
    private PlayerHealthPresenter _presenter;
    public Transform Transform => transform;
    public void Initialize(PlayerHealthPresenter presenter)
    {
        _presenter = presenter;
    }

    public void ReceiveDamage(int damage)
    {
        _presenter.TakeDamage(damage);
    }
}
