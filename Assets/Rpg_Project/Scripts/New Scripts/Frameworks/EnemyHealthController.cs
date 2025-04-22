using UnityEngine;

public class EnemyHealthController : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;

    private EnemyHealth _health;

    private void Awake()
    {
        _health = new EnemyHealth(maxHealth);
    }

    public void ReceiveDamage(int damage)
    {
        _health.TakeDamage(damage);
        Debug.Log($"Enemy took {damage} damage. Current HP: {_health.Current}");
    }

    public bool IsDead() => _health.IsDead;
}
