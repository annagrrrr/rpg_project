using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthPresenter : MonoBehaviour, IEnemyHealth
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private EnemyHealthBarUI barUI;

    [SerializeField] private AttackType resistanceType;
    [SerializeField, Range(0f, 1f)] private float resistanceValue = 0.3f;
    private ResistanceProfile _resistances;

    public event Action OnDied;
    public event Action OnDamaged;

    private EnemyHealth _health;
    
    public bool IsDead => _health.IsDead;
    public int Current => _health.Current;
    public int Max => _health.Max;

    private void Awake()
    {
        _health = new EnemyHealth(maxHealth);

        _resistances = new ResistanceProfile(new Dictionary<AttackType, float>
        {
            { resistanceType, resistanceValue }
        });

        if (barUI != null)
            barUI.Initialize(transform, maxHealth);

        UpdateView();
    }
    public void ReceiveDamage(int damage)
    {
        _health.TakeDamage(damage);
        Debug.Log($"Enemy received {damage} damage. Current HP: {_health.Current}");

        UpdateView();

        OnDamaged?.Invoke();

        if (IsDead)
        {
            OnDied?.Invoke();
        }
    }
    public void ReceiveDamage(int damage, AttackType type)
    {
        float resist = _resistances.GetResistance(type);
        int finalDamage = Mathf.CeilToInt(damage * (1f - resist));

        _health.TakeDamage(finalDamage);

        Debug.Log($"Enemy received {finalDamage} damage after {resist * 100}% resistance. Type: {type}");

        UpdateView();
        OnDamaged?.Invoke();

        if (IsDead)
            OnDied?.Invoke();
    }
    private void UpdateView()
    {
        if (barUI != null)
            barUI.UpdateHealth(_health.Current);
    }
}
