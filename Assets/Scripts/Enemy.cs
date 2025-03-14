using System;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.InputSystem.OnScreen.OnScreenStick;

public class Enemy : MonoBehaviour
{
    public string Name { get; private set; }
    public HealthManager Health { get; private set; }
    public float PhysicalResistance { get; private set; }
    public float MagicResistance { get; private set; }

    private DamageHandler damageHandler;

    private IEnemyBehaviour currentBehaviour;


    //for raycaaast
    public float groundCheckDistance = 10f;
    public LayerMask groundLayer;
    public void Initialize(string name, int maxHealth, float physicalResistance, float magicResistance, IEnemyBehaviour behaviour)
    {
        Name = name;
        Health = new HealthManager(maxHealth);
        PhysicalResistance = physicalResistance;
        MagicResistance = magicResistance;
        currentBehaviour = behaviour;

        damageHandler = new DamageHandler();

        Health.onDamageTaken += (damage) => Debug.Log($"{Name} ������� {damage} �����!");
        Health.onDeath += () => Debug.Log($"{Name} �����!");
    }

    private void Update()
    {
        if (currentBehaviour != null)
        {
            Transform player = FindPlayerTransform();
            currentBehaviour.UpdateBehaviour(this, player);
        }
    }

    public void TakeDamage(int baseDamage, DamageType damageType)
    {
        int finalDamage = damageHandler.CalculateDamage(baseDamage, damageType, PhysicalResistance, MagicResistance);
        Health.TakeDamage(finalDamage);
    }

    private Transform FindPlayerTransform()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        return player?.transform;
    }
    public void Attack(Transform target)
    {
        Debug.Log($"{Name} ������� {target.name}");
    }

    public void MoveTowards(Transform target, float speed)
    {
        if (target == null)
        {
            Debug.LogWarning("no target err");
            return;
        }

        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        Vector3 direction = (target.position - transform.position).normalized;
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
    }

    public void MoveAwayFrom(Transform target, float speed, float safeDistance)
    {
        if (target == null)
        {
            Debug.LogWarning("no target err");
            return;
        }


        float distanceToTarget = Vector3.Distance(transform.position, target.position);


        if (distanceToTarget < safeDistance)
        {

            Vector3 direction = (transform.position - target.position).normalized;

            //if (!IsGroundAhead(direction))
            //{
            //    Debug.Log($"{Name} �� ����� ��������� � �����!");
            //    return;
            //}

            transform.position += direction * speed * Time.deltaTime;

            if (direction != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
            }

            Debug.Log($"{Name} ��������� �� {target.name}");
        }
        else
        {
            Debug.Log($"{Name} �� ���������� ���������� �� {target.name}");
        }
    }

    //public bool IsGroundAhead(Vector3? checkDirection = null)
    //{
    //    Vector3 rayStart = transform.position + Vector3.up * 1.5f;
    //    Vector3 direction = checkDirection ?? transform.forward;

    //    RaycastHit hit;
    //    if (Physics.Raycast(rayStart + direction * 0.5f, Vector3.down, out hit, groundCheckDistance, groundLayer))
    //    {
    //        Debug.Log($"����� �������! ������: {hit.collider.gameObject.name}");
    //        Debug.DrawRay(rayStart + direction * 0.5f, Vector3.down * groundCheckDistance, Color.green);
    //        return true;
    //    }

    //    Debug.Log("����� �� �������!");
    //    Debug.DrawRay(rayStart + direction * 0.5f, Vector3.down * groundCheckDistance, Color.red);
    //    return false;
    //}
}
