using UnityEngine;

public class Projectile : MonoBehaviour
{
    private int _damage;
    private PlayerHealthController _player;

    public void Initialize(PlayerHealthController player, int damage)
    {
        _player = player;
        _damage = damage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PlayerHealthController>(out var player))
        {
            player.ReceiveDamage(_damage);
            Destroy(gameObject);
        }
    }
}
