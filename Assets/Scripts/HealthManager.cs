using System;
using UnityEngine;

public class HealthManager: MonoBehaviour
{
    public int currentHealth { get; private set; }
    public int maxHealth { get; private set; }
    public event Action onDeath;
    public event Action<int> onDamageTaken;
    public event Action<int> onHeal;

    public HealthManager(int MaxHealth)
    {
        maxHealth = MaxHealth;
        currentHealth = MaxHealth;
    }

    public void TakeDamage(int amount)
    {
        if (amount <= 0) return;

        currentHealth -= amount;
        onDamageTaken?.Invoke(currentHealth);

        Console.WriteLine($"Çהמנמגüו: {currentHealth}/{maxHealth}");

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

    public void ResetHealth()
    {
        currentHealth = maxHealth;
    }
}
