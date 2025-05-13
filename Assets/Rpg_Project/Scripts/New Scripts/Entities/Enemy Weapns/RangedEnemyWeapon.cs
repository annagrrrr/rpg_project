//using UnityEngine;

//public class RangedEnemyWeapon : MonoBehaviour, IEnemyWeapon
//{
//    [SerializeField] private GameObject projectilePrefab;
//    [SerializeField] private Transform firePoint;

//    private int _damage;
//    private PlayerHealthController _player;

//    public void Initialize(PlayerHealthController player, int damage)
//    {
//        _player = player;
//        _damage = damage;
//    }

//    public void Attack()
//    {
//        if (projectilePrefab == null || firePoint == null || _player == null) return;

//        GameObject projectile = GameObject.Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
//        projectile.GetComponent<Projectile>().Initialize(_player, _damage);

//        Vector3 direction = (_player.transform.position - firePoint.position).normalized;

//        projectile.GetComponent<Rigidbody>().linearVelocity = direction * 10f;

//    }
//}
