using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    private int score = 0;
    public int Score => score;

    public void AddScore(int amount)
    {
        score += amount;
        Debug.Log($"Score: {score}");
        OnScoreChanged?.Invoke(score);
    }

    public event System.Action<int> OnScoreChanged;

    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        else Instance = this;
    }
}
