using UnityEngine;

public class BossRangedWeapon : MonoBehaviour, IBossWeapon
{
    public float AttackRange => 10f;
    private ElementType _element;
    [SerializeField] private GameObject projectilePrefab;
    private Transform firePoint;
    public void SetFirePoint(Transform firePoint)
    {
        this.firePoint = firePoint;
    }

    public void Attack(Transform target, int damage)
    {
        if (projectilePrefab == null || firePoint == null) return;

        var proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        var projectile = proj.GetComponent<BossProjectile>();
        projectile?.Initialize(target, damage, _element);
    }
    public void SetElement(ElementType element)
    {
        _element = element;
    }
}
