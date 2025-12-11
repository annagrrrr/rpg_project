using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider volumeSlider;

    private const string VolumeKey = "MasterVolume";

    private void Start()
    {
        float savedVolume = PlayerPrefs.GetFloat(VolumeKey, 1f);
        volumeSlider.value = savedVolume;
        SetVolume(savedVolume);

        volumeSlider.onValueChanged.AddListener(SetVolume);
    }

    private void SetVolume(float value)
    {
        float volumeInDb = Mathf.Log10(value) * 20f;

        audioMixer.SetFloat("MasterVolume", volumeInDb);
        PlayerPrefs.SetFloat(VolumeKey, value);
    }
}
