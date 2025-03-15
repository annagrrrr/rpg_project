using UnityEngine;

public class Weapon : MonoBehaviour
{
    public void Attack(Enemy target, int damage, DamageType type)
    {
        target.TakeDamage(damage, type);
    }
}
