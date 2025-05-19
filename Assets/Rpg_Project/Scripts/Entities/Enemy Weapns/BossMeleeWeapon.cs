using UnityEngine;

public class BossMeleeWeapon : MonoBehaviour, IBossWeapon
{
    public float AttackRange => 4f;
    private ElementType _element;
    [SerializeField] private Renderer weaponRenderer;

    public void Attack(Transform target, int damage)
    {
        //���� ���������
    }

    public void SetElement(ElementType element)
    {
        if (weaponRenderer == null) return;

        Color color = element switch
        {
            ElementType.Fire => Color.red,
            ElementType.Ice => Color.cyan,
            ElementType.Earth => Color.green,
            ElementType.Aether => Color.magenta,
            _ => Color.white,
        };

        weaponRenderer.material.color = color;
    }
}
