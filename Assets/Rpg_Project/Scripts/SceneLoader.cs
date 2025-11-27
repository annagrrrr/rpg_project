using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader
{
    private const string STATS_SCENE_NAME = "StatsScene";
    private const string GAME_SCENE_NAME = "FirstLocation";
    private const string MAIN_MENU_SCENE_NAME = "MainMenu";

    public void LoadStatsScene()
    {
        if (SceneExists(STATS_SCENE_NAME))
        {
            SceneManager.LoadScene(STATS_SCENE_NAME);
        }
        else
        {
            Debug.LogWarning($"Scene '{STATS_SCENE_NAME}' not found! Loading game scene instead.");
            LoadGameScene();
        }
    }

    public void LoadGameScene()
    {
        if (SceneExists(GAME_SCENE_NAME))
        {
            SceneManager.LoadScene(GAME_SCENE_NAME);
        }
        else
        {
            Debug.LogError($"Game scene '{GAME_SCENE_NAME}' not found!");
        }
    }

    public void LoadMainMenu()
    {
        if (SceneExists(MAIN_MENU_SCENE_NAME))
        {
            SceneManager.LoadScene(MAIN_MENU_SCENE_NAME);
        }
        else
        {
            Debug.LogWarning($"Main menu scene '{MAIN_MENU_SCENE_NAME}' not found! Loading game scene instead.");
            LoadGameScene();
        }
    }

    public void RestartGame()
    {
        LoadGameScene();
    }

    private bool SceneExists(string sceneName)
    {
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            string scene = System.IO.Path.GetFileNameWithoutExtension(scenePath);
            if (scene == sceneName)
                return true;
        }
        return false;
    }
}