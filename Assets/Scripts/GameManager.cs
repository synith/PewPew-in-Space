using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public SpawnManager SpawnManager { get; private set; }

    private int currentScore;

    // game state
    public bool gameStarted;
    public bool gamePaused;
    public bool gameOver;

    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI highScoreText;
    [SerializeField] private TextMeshProUGUI statusText;


    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        // Anything else in Awake goes after this
        SpawnManager = FindObjectOfType<SpawnManager>();
        currentScore = 0;
    }

    private void Start()
    {
        if (ScoreManager.Instance != null)
        {
            scoreText.text = SetScore();
            highScoreText.text = SetHighScore();
        }
    }

    // public methods go here

    public void ShowStatus(string status)
    {
        statusText.text = status;
        statusText.gameObject.SetActive(true);
        StartCoroutine(nameof(StatusCooldown));
    }

    public void AddPoint(int point)
    {
        currentScore += point;
        scoreText.text = SetScore();
        CheckHighScore();
    }

    private IEnumerator StatusCooldown()
    {
        yield return new WaitForSeconds(4);
        statusText.gameObject.SetActive(false);
    }

    private void CheckHighScore()
    {
        if (ScoreManager.Instance != null)
        {
            if (currentScore > ScoreManager.Instance.HighScore)
            {
                ScoreManager.Instance.HighScore = currentScore;
                ScoreManager.Instance.HighScorePlayer = ScoreManager.Instance.PlayerName;
                highScoreText.text = SetHighScore();
                ScoreManager.Instance.SaveHighScore();
            }
        }
    }
    private string SetHighScore()
    {
        string highScore;
        if (ScoreManager.Instance != null)
        {
            highScore = $"Best Score : {ScoreManager.Instance.HighScore} - {ScoreManager.Instance.HighScorePlayer}";
            return highScore;
        }
        else
        {
            highScore = $"Score : ? - Not Found";
            return highScore;
        }
    }
    private string SetScore()
    {
        string score;
        if (ScoreManager.Instance != null)
        {
            score = $"Score : {currentScore} - {ScoreManager.Instance.PlayerName}";
            return score;
        }
        else
        {
            score = $"Score: ? - Not Found";
            return score;
        }
    }
}
