public class HealthUIUpdater
{
    private HealthBar healthBar;

    public HealthUIUpdater(HealthBar healthBar)
    {
        this.healthBar = healthBar;
    }

    public void Subscribe(HealthManager healthManager)
    {
        healthManager.OnHealthChanged += UpdateHealthBar;
    }

    private void UpdateHealthBar(int currentHealth, int maxHealth)
    {
        healthBar.SetHealth(currentHealth, maxHealth);
    }
}
