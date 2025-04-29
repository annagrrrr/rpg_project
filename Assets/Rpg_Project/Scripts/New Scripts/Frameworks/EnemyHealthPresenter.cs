using System;
using UnityEngine;

public class EnemyHealthPresenter : MonoBehaviour, IEnemyHealth
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private EnemyHealthBarUI barUI;

    public event Action OnDamaged;

    private EnemyHealth _health;

    public bool IsDead => _health.IsDead;
    private void Awake()
    {
        _health = new EnemyHealth(maxHealth);
        if (barUI != null)
            barUI.Initialize(transform, maxHealth);
        UpdateView();
    }

    public void ReceiveDamage(int damage)
    {
        _health.TakeDamage(damage);
        Debug.Log($"Enemy received {damage} damage. Current HP: {_health.Current}");

        UpdateView();

        if (IsDead)
        {
            Die();
        }
        OnDamaged?.Invoke();
    }

    private void UpdateView()
    {
        if (barUI != null)
            barUI.UpdateHealth(_health.Current);
    }
    private void Die()
    {
        Debug.Log("Enemy has died!");
        Destroy(gameObject);
    }
}
