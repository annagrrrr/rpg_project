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
    var bestStats = _repository.GetBestStats();

    if (lastStats == null || bestStats == null)
    {
        Debug.LogWarning("Stats not found. Nothing to display.");
        return;
    }

    _view.DisplayStats(lastStats, bestStats);

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