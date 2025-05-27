using UnityEngine;
using UnityEngine.UI;

public class ScoreDisplay : MonoBehaviour
{
    [SerializeField] private Text scoreText;

    private void Start()
    {
        ScoreManager.Instance.OnScoreChanged += UpdateScore;
        UpdateScore(ScoreManager.Instance.Score);
    }

    private void UpdateScore(int score)
    {
        scoreText.text = "Score: " + score;
    }
}
