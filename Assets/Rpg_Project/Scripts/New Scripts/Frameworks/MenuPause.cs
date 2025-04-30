using UnityEngine;

public class MenuPause : MonoBehaviour
{
    public void ToggleGameMode()
    {
        GameModeManager.Instance.ToggleMode();
        Debug.Log($"Game mode set to: {GameModeManager.Instance.CurrentMode}");
    }
}
