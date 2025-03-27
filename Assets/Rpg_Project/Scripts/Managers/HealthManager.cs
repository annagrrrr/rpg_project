using System;
using UnityEngine;

public class HealthManager
{
    public int currentHealth { get; private set; }
    public int maxHealth { get; private set; }
    public event Action onDeath;
    public event Action<int> onDamageTaken;
    public event Action<int> onHeal;
    public Action<int, int> OnHealthChanged;

    [SerializeField] private HealthBar healthBar;

    private void Start()
    {
        currentHealth = maxHealth;
        if (healthBar != null)
        {
            healthBar.SetHealth(currentHealth, maxHealth);
        }
        else
        {
            Debug.LogError("не определен healthBar");
        }
    }
    public HealthManager(int maxHealth)
    {
        this.maxHealth = maxHealth;
        this.currentHealth = maxHealth;

        if (healthBar != null)
        {
            healthBar.SetHealth(currentHealth, maxHealth);
        }
    }

    public void TakeDamage(int amount)
    {
        if (amount <= 0) return;

        currentHealth -= amount;
        onDamageTaken?.Invoke(currentHealth);
        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        Console.WriteLine($"Здоровье: {currentHealth}/{maxHealth}");

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            onDeath?.Invoke();
        }
    }

    public void Heal(int amount)
    {
        if (amount <= 0 || currentHealth <= 0) return;

        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        onHeal?.Invoke(amount);
    }

    public void SetMaxHealth(int newMaxHealth)
    {
        maxHealth = newMaxHealth;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        OnHealthChanged?.Invoke(currentHealth, maxHealth);

    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
    }

}
