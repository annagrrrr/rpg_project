using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;

    private void Start()
    {
        ScoreManager.Instance.OnScoreChanged += UpdateScore;
        UpdateScore(ScoreManager.Instance.Score);
    }

    private void UpdateScore(int score)
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score;
        else
            Debug.LogError("ScoreText is not assigned!");
    }
}
