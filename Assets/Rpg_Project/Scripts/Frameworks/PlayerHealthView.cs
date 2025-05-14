using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthView : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;

    public void SetMaxHealth(int max)
    {
        healthSlider.maxValue = max;
        healthSlider.value = max;
    }

    public void SetCurrentHealth(int current)
    {
        healthSlider.value = current;
    }
}
