using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer mainMixer;
    public Slider volumeSlider;

    private void Start()
    {
        float savedVolume = PlayerPrefs.GetFloat("MasterVolume", 0.75f);
        SetMasterVolume(savedVolume);
    }

    private void OnEnable()
    {
        float savedVolume = PlayerPrefs.GetFloat("MasterVolume", 0.75f);

        if (volumeSlider != null)
        {
            volumeSlider.onValueChanged.RemoveListener(SetMasterVolume);
            volumeSlider.value = savedVolume;
            volumeSlider.onValueChanged.AddListener(SetMasterVolume);
        }

        SetMasterVolume(savedVolume);
    }

    public void SetMasterVolume(float volume)
    {
        if (mainMixer != null)
        {
            mainMixer.SetFloat("MasterVolume", Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1f)) * 20);
            PlayerPrefs.SetFloat("MasterVolume", volume);
        }
    }
}
