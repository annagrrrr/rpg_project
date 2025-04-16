using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    private PlayerHealthPresenter _presenter;

    public void Initialize(PlayerHealthPresenter presenter)
    {
        Debug.Log("PlayerHealthController: Initialized");
        _presenter = presenter;
    }

    public void ReceiveDamage(int damage)
    {
        Debug.Log("PlayerHealthController: Received damage");
        Debug.Log(_presenter);
        _presenter.TakeDamage(damage);
    }
}
