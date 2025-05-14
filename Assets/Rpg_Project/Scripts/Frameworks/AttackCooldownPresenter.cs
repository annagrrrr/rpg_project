using UnityEngine;
using UnityEngine.UI;

public class AttackCooldownPresenter : MonoBehaviour, IAttackCooldownPresenter
{
    [SerializeField] private Image _cooldownImage;
    [SerializeField] private Color _readyColor = Color.blue;
    [SerializeField] private Color _cooldownColor = Color.white;

    public void UpdateSecondaryCooldown(float currentTime, float cooldownTime)
    {
        float fillAmount = Mathf.Clamp01(1f - (currentTime / cooldownTime));
        _cooldownImage.fillAmount = fillAmount;

        _cooldownImage.color = fillAmount >= 1f ? _readyColor : _cooldownColor;
    }
}
