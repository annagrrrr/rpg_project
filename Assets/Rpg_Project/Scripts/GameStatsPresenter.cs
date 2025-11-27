using UnityEngine;

public class GameStatsPresenter
{
    private readonly GameStatsView _view;
    private readonly SceneLoader _sceneLoader;
    private readonly IGameStatsRepository _repository;

    public GameStatsPresenter(GameStatsView view, SceneLoader sceneLoader, IGameStatsRepository repository)
    {
        _view = view;
        _sceneLoader = sceneLoader;
        _repository = repository;

        InitializeView();
    }

    private void InitializeView()
    {
        var lastStats = _repository.GetLastStats();

        if (lastStats != null)
        {
            _view.DisplayStats(lastStats);
            Debug.Log($"Displaying stats: {lastStats}");
        }
        else
        {
            Debug.LogWarning("No stats found to display!");
            var testStats = new GameStats
            {
                EnemiesKilled = 8,
                DamageDealt = 750,
                DamageTaken = 180,
                GameTime = 325f,
                IsVictory = true
            };
            _view.DisplayStats(testStats);
        }

        // ??????????? ??????
        _view.SetRestartAction(RestartGame);
        _view.SetMainMenuAction(GoToMainMenu);
        _view.SetQuitAction(QuitGame);
    }

    private void RestartGame()
    {
        Debug.Log("Restarting game...");
        _sceneLoader.RestartGame();
    }

    private void GoToMainMenu()
    {
        Debug.Log("Going to main menu...");
        _sceneLoader.LoadMainMenu();
    }

    private void QuitGame()
    {
        Debug.Log("Quitting game...");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
}