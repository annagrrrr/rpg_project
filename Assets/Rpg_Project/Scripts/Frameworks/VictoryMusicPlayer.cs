using UnityEngine;

public class VictoryMusicPlayer : MonoBehaviour
{
    [SerializeField] private AudioClip victoryClip;
    [SerializeField] private AudioSource audioSource;

    public void PlayVictory()
    {
        audioSource.clip = victoryClip;
        audioSource.Play();
    }
}
