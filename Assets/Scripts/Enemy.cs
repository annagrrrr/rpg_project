using UnityEngine;

public class Enemy : MonoBehaviour
{
    public string Name { get; private set; }
    public HealthManager Health { get; private set; }
    public float PhysicalResistance { get; private set; }
    public float MagicResistance { get; private set; }

    private DamageHandler damageHandler;

    public void Initialize(string name, int maxHealth, float physicalResistance, float magicResistance)
    {
        Name = name;
        Health = new HealthManager(maxHealth);
        PhysicalResistance = physicalResistance;
        MagicResistance = magicResistance;

        damageHandler = new DamageHandler();

        Health.onDamageTaken += (damage) => Debug.Log($"{Name} получил {damage} урона!");
        Health.onDeath += () => Debug.Log($"{Name} погиб!");
    }

    public void TakeDamage(int baseDamage, DamageType damageType)
    {
        int finalDamage = damageHandler.CalculateDamage(baseDamage, damageType, PhysicalResistance, MagicResistance);
        Health.TakeDamage(finalDamage);
    }
}
