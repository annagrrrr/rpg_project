using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    private PlayerHealthPresenter _presenter;

    public void Initialize(PlayerHealthPresenter presenter)
    {
        _presenter = presenter;
    }

    public void ReceiveDamage(int damage)
    {
        _presenter.TakeDamage(damage);
    }
}
