using UnityEngine;

public class AttackPresenter : IAttackPresenter
{
    public void ShowAttack(AttackType attackType)
    {
        switch (attackType)
        {
            case AttackType.Physical:
                Debug.Log("¿Ú‡Í‡ À Ã!");
                break;
            case AttackType.Magical:
                Debug.Log("¿Ú‡Í‡ œ Ã!");
                break;
        }
    }
}
