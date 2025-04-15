using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;

    private PlayerHealthPresenter _presenter;

    private void Awake()
    {
        var health = new Health(maxHealth);
        _presenter = new PlayerHealthPresenter(health);
    }

    public void ReceiveDamage(int damage)
    {
        _presenter.TakeDamage(damage);
    }
}
