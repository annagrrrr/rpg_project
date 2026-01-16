using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameStatsView : MonoBehaviour
{
    [Header("Text Elements")]
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI resultText;
    [SerializeField] private TextMeshProUGUI killsText;
    [SerializeField] private TextMeshProUGUI damageDealtText;
    [SerializeField] private TextMeshProUGUI damageTakenText;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI scoreText;

    [SerializeField] private TextMeshProUGUI bestKillsText;
    [SerializeField] private TextMeshProUGUI bestDamageDealtText;
    [SerializeField] private TextMeshProUGUI bestDamageTakenText;
    [SerializeField] private TextMeshProUGUI bestTimeText;
    [SerializeField] private TextMeshProUGUI bestScoreText;


    [Header("Buttons")]
    [SerializeField] private Button restartButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button quitButton;

    private GameStats cachedStats;
    private BestGameStats cachedBestStats;

    public void DisplayStats(GameStats stats, BestGameStats bestStats)
    {
        if (stats == null)
        {
            Debug.LogWarning("No stats to display!");
            return;
        }

        cachedStats = stats;
        cachedBestStats = bestStats;

        if (gameObject.activeInHierarchy)
        {
            ApplyStats();
        }
    }

    private void OnEnable()
    {
        if (cachedStats != null)
        {
            ApplyStats();
        }
    }

    private void ApplyStats()
    {
        titleText.text = "Statistics";

        resultText.text = cachedStats.IsVictory
            ? "Result: <color=green>WIN</color>"
            : "Result: <color=red>LOSS</color>";

        killsText.text = $"Kills: {cachedStats.EnemiesKilled}";
        damageDealtText.text = $"Damage Dealt: {cachedStats.DamageDealt}";
        damageTakenText.text = $"Damage Taken: {cachedStats.DamageTaken}";
        timeText.text = $"Time: {cachedStats.GetFormattedTime()}";
        scoreText.text = $"Score: {cachedStats.TotalScore}";
        bestKillsText.text = $"Best Kills: {cachedBestStats.BestEnemiesKilled}";
        bestDamageDealtText.text = $"Best Damage: {cachedBestStats.BestDamageDealt}";
        bestDamageTakenText.text = $"Best Damage Taken: {cachedBestStats.BestDamageTaken}";
        bestTimeText.text = $"Best Time: {FormatTime(cachedBestStats.BestGameTime)}";
        bestScoreText.text = $"Best Score: {cachedBestStats.BestTotalScore}";

    }

    public void SetRestartAction(System.Action action)
    {
        restartButton.onClick.RemoveAllListeners();
        restartButton.onClick.AddListener(() => action?.Invoke());
    }

    public void SetMainMenuAction(System.Action action)
    {
        mainMenuButton.onClick.RemoveAllListeners();
        mainMenuButton.onClick.AddListener(() => action?.Invoke());
    }

    public void SetQuitAction(System.Action action)
    {
        quitButton.onClick.RemoveAllListeners();
        quitButton.onClick.AddListener(() => action?.Invoke());
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
    
    private string FormatTime(float time)
    {
        int minutes = (int)(time / 60);
        int seconds = (int)(time % 60);
        return $"{minutes:00}:{seconds:00}";
    }

}
