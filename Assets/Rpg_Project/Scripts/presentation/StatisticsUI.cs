using UnityEngine;
using UnityEngine.SceneManagement;

public class StatisticsUI : MonoBehaviour
{
    [SerializeField] private StatisticsView view;
    private StatisticsPresenter presenter;

    private void Awake()
    {
        presenter = new StatisticsPresenter(view);
        gameObject.SetActive(false);
    }

    public void Show(RunStatistics stats)
    {
        presenter.Show(stats);
        gameObject.SetActive(true);
    }

    public void OnRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void OnExit()
    {
        Application.Quit();
    }
}
