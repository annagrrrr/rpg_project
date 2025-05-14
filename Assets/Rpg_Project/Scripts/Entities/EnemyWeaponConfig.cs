using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/Weapon Config")]
public class EnemyWeaponConfig : ScriptableObject
{
    public GameObject weaponPrefab;
    public int damage;
}
