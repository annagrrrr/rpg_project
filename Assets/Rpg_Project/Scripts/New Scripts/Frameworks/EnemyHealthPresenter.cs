using System;
using UnityEngine;

public class EnemyHealthPresenter : MonoBehaviour, IEnemyHealth
{
    [SerializeField] private int maxHealth = 100;

    public event Action OnDamaged;

    private EnemyHealth _health;

    public bool IsDead => _health.IsDead;
    private void Awake()
    {
        _health = new EnemyHealth(maxHealth);
    }

    public void ReceiveDamage(int damage)
    {
        _health.TakeDamage(damage);
        Debug.Log($"Enemy received {damage} damage. Current HP: {_health.Current}");

        if (IsDead)
        {
            Die();
        }
        OnDamaged?.Invoke();
    }

    private void Die()
    {
        Debug.Log("Enemy has died!");
        Destroy(gameObject);
    }
}
