using UnityEngine;

public class BossProjectile : MonoBehaviour
{
    private Transform _target;
    private int _damage;
    private ElementType _element;
    private float _speed = 10f;

    public void Initialize(Transform target, int damage, ElementType element)
    {
        _target = target;
        _damage = damage;
        _element = element;

        ApplyElementVisual();
    }

    private void Update()
    {
        if (_target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 direction = (_target.position - transform.position).normalized;
        transform.position += direction * _speed * Time.deltaTime;

        if (Vector3.Distance(transform.position, _target.position) < 0.5f)
        {
            HitTarget();
        }
    }

    private void HitTarget()
    {
        var health = _target.GetComponent<PlayerHealthController>();
        health?.ReceiveDamage(_damage);
        //Debug.Log($"Projectile hit player with {_element} element for {_damage} damage.");
        Destroy(gameObject);
    }

    private void ApplyElementVisual()
    {
        var renderer = GetComponent<Renderer>();
        if (renderer == null) return;

        switch (_element)
        {
            case ElementType.Fire:
                renderer.material.color = Color.red;
                break;
            case ElementType.Ice:
                renderer.material.color = Color.cyan;
                break;
            case ElementType.Earth:
                renderer.material.color = Color.green;
                break;
            case ElementType.Aether:
                renderer.material.color = Color.magenta;
                break;
        }
    }
}
