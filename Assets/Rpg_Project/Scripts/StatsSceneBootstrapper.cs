using UnityEngine;
using System.Collections;

public class StatsSceneBootstrapper : MonoBehaviour
{
    [SerializeField] private GameStatsView statsView;

    private void Start()
    {
        var statsRepository = new PlayerPrefsStatsRepository();
        var sceneLoader = new SceneLoader();

        statsRepository.ClearAllStats();

        var testStats = new GameStats
        {
            EnemiesKilled = 15,
            DamageDealt = 1250,
            DamageTaken = 320,
            GameTime = 856f,
            IsVictory = true
        };
        statsRepository.SaveStats(testStats);
        StartCoroutine(InitializeAfterDelay(statsRepository, sceneLoader));

        //var presenter = new GameStatsPresenter(statsView, sceneLoader, statsRepository);

        //Debug.Log("Stats scene initialized!");
    }
    private IEnumerator InitializeAfterDelay(IGameStatsRepository repository, SceneLoader sceneLoader)
    {
        yield return null;

        var presenter = new GameStatsPresenter(statsView, sceneLoader, repository);
        Debug.Log("Stats scene initialized with test data!");
    }
}