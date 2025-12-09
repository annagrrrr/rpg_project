using UnityEngine;

public class StatsSceneBootstrapper : MonoBehaviour
{
    [SerializeField] private GameStatsView statsView;

    private void Start()
    {
        Debug.Log("StatsView object: " + statsView.gameObject.name);
        var statsRepository = new PlayerPrefsStatsRepository();
        var sceneLoader = new SceneLoader();

        var presenter = new GameStatsPresenter(statsView, sceneLoader, statsRepository);

        Debug.Log("StatsScene initialized (REAL statistics displayed).");
    }
}
