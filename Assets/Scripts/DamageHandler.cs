using UnityEngine;

public class DamageHandler : MonoBehaviour
{
    public int CalculateDamage(int baseDamage, DamageType damageType, float physicalResistance, float magicResistance)
    {
        float finalDamage = baseDamage;

        switch (damageType)
        {
            case DamageType.PHYSICAL:
                finalDamage *= 1 - physicalResistance;
                break;

            case DamageType.MAGICAL:
                finalDamage *= 1 - magicResistance;
                break;
        }

        return Mathf.Max(0, (int)finalDamage);
    }
}
