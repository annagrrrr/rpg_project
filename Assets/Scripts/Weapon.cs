using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int damage; // Для чего эти попубличные поля? Если хотим через редактор образаться, делает [SerializedField] и private доступ
    public DamageType damageType;
    public IAttack attackType;
    private GameObject owner;
    private void Awake()
    {
        if (attackType == null)
        {
            if (damageType == DamageType.PHYSICAL)
            {
                attackType = new MeleeAttack();
            }
            else if (damageType == DamageType.MAGICAL)
            {
                attackType = new MagicAttack();
            }
        }
    }

    public void SetOwner(GameObject weaponOwner)
    {
        owner = weaponOwner;
    }

    public void PerformAttack()
    {
        if (attackType != null)
        {
            attackType.ExecuteAttack(owner.transform, damage);
        }
        else
        {
            Debug.LogWarning("������ �� ����� ���� �����!");
        }
    }
}
