using UnityEngine;

public enum GameMode { Normal, Peaceful }

public class GameModeManager : MonoBehaviour
{
    public static GameModeManager Instance { get; private set; }

    public GameMode CurrentMode { get; private set; } = GameMode.Normal;

    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void ToggleMode()
    {
        CurrentMode = (CurrentMode == GameMode.Normal) ? GameMode.Peaceful : GameMode.Normal;
        Debug.Log("Game mode set to: " + CurrentMode);
    }
}
