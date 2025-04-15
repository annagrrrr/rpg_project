using UnityEngine;
using UnityEngine.UI;
public class HealthBar : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    [SerializeField] private Slider healthSlider;

    public void SetHealth(int currentHealth, int maxHealth)
    {
        healthSlider.value = currentHealth;
    }
}
