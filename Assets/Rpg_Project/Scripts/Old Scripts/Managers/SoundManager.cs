using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField] private AudioSource playerAudioSource;
    [SerializeField] private AudioClip meleeClip;
    [SerializeField] private AudioClip magicClip;
    [SerializeField] private AudioClip runClip;
    [SerializeField] private AudioClip jumpClip;
    private bool isRunning = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        SetVolume(1f);
    }

    public void PlayMelee() => PlayClip(meleeClip);
    public void PlayMagic() => PlayClip(magicClip);
    public void PlayJump() => PlayClip(jumpClip);

    private void PlayClip(AudioClip clip)
    {
        if (clip != null && playerAudioSource != null)
        {
            playerAudioSource.PlayOneShot(clip);
        }
    }

    public void StartRunning(float stepSpeed)
    {
        if (!isRunning && runClip != null && playerAudioSource != null)
        {
            playerAudioSource.clip = runClip;
            playerAudioSource.loop = true;
            playerAudioSource.Play();
            playerAudioSource.pitch = stepSpeed;  
            isRunning = true;
        }
    }

    public void StopRunning()
    {
        if (isRunning && playerAudioSource != null)
        {
            playerAudioSource.Stop();
            isRunning = false;
        }
    }
    public void SetVolume(float volume)
    {
        if (playerAudioSource != null)
        {
            playerAudioSource.volume = volume;
        }
    }

}
