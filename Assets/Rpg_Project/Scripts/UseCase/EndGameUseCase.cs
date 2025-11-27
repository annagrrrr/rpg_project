public class EndGameUseCase
{
    private readonly GameStatsService _statsService;
    private readonly SceneLoader _sceneLoader;

    public EndGameUseCase(GameStatsService statsService, SceneLoader sceneLoader)
    {
        _statsService = statsService;
        _sceneLoader = sceneLoader;
    }

    public void Execute(bool isVictory)
    {
        // fix stats
        _statsService.CompleteGame(isVictory);

        // load stats
        _sceneLoader.LoadStatsScene();
    }
}