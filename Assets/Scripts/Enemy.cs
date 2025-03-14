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
            Debug.Log($"aaa");
        }
    }

    public void TakeDamage(int baseDamage, DamageType damageType)
    {
        int finalDamage = damageHandler.CalculateDamage(baseDamage, damageType, PhysicalResistance, MagicResistance);
        Health.TakeDamage(finalDamage);
    }

    private Transform FindPlayerTransform()
    {
        Debug.Log("��� ���������� � �����");
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        return player?.transform;
    }
    public void Attack(Transform target)
    {
        Debug.Log($"{Name} ������� {target.name}!");
    }

    public void MoveTowards(Vector3 position)
    {
        Debug.Log($"{Name} ��� � ����!");
    }

    public void MoveAwayFrom(Vector3 position)
    {
        Debug.Log($"{Name} ���������!");
    }
}
