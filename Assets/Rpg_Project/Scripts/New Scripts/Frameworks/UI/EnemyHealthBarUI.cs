using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBarUI : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Vector3 offset = new Vector3(0, 2.2f, 0);
    private Transform _target;
    private Camera _camera;

    public void Initialize(Transform target, int maxHealth)
    {
        _target = target;
        _camera = Camera.main;
        healthSlider.maxValue = maxHealth;
        healthSlider.value = maxHealth;
    }

    public void UpdateHealth(int currentHealth)
    {
        Debug.Log(currentHealth + "dgsdfdhshtht");
        healthSlider.value = currentHealth;
    }

    private void LateUpdate()
    {
        if (_target == null) return;

        transform.position = _target.position + offset;
        //transform.LookAt(transform.position + _camera.transform.rotation * Vector3.forward, _camera.transform.rotation * Vector3.up);
    }
}
