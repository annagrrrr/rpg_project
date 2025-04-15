using UnityEngine;

public class AttackPresenter : IAttackPresenter
{
    public void ShowAttack(AttackType attackType)
    {
        switch (attackType)
        {
            case AttackType.Physical:
                Debug.Log("����� ���!");
                break;
            case AttackType.Magical:
                Debug.Log("����� ���!");
                break;
        }
    }
}
