using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

// Game Manager singleton that tracks game states as well as holding public score methods and a public reference to the Spawn Manager class

public class GameManager : MonoBehaviour
{

    // ENCAPSULATION
    public static GameManager Instance { get; private set; }
    public SpawnManager SpawnManager { get; private set; }    

    // ENCAPSULATION
    public bool GameStarted { get; set; }
    public bool GamePaused { get; set; }
    public bool GameOver { get; set; }
    public bool GameWon { get; set; }

    public int EnemiesDefeatedCount { get; set; }
    public int TotalEnemies { get; internal set; }

    // text boxes during gameplay
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI highScoreText;
    [SerializeField] private TextMeshProUGUI statusText;

    private int currentScore;

    private void Awake()
    {
        if (Instance != null)  // check to make sure there is not another instance of this class
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        SpawnManager = FindObjectOfType<SpawnManager>();
        currentScore = 0;
    }
    private void Start() // initialize score textboxes using score data
    {
        if (ScoreManager.Instance != null)
        {
            scoreText.text = SetScore(); // ABSTRACTION
            highScoreText.text = SetHighScore(); // ABSTRACTION
        }
    }
    public void ShowStatus(string status) // sets textbox active, assigns string parameter and starts a cooldown before text dissapears
    {
        statusText.text = status;
        statusText.gameObject.SetActive(true);
        StartCoroutine(nameof(StatusCooldown));
    }
    public void AddPoint(int point) // add points to current score and check to see if new highscore
    {
        currentScore += point;
        scoreText.text = SetScore();
        CheckHighScore();
    }
    private IEnumerator StatusCooldown() // 4 second cooldown on status text before dissapearing
    {
        yield return new WaitForSeconds(4);
        statusText.gameObject.SetActive(false);
    }
    private void CheckHighScore() // if current score is greater than highscore then the current score is the new highscore, and highscore data should be saved
    {
        if (ScoreManager.Instance != null)
        {
            if (currentScore > ScoreManager.Instance.HighScore)
            {
                ScoreManager.Instance.HighScore = currentScore;
                ScoreManager.Instance.HighScorePlayer = ScoreManager.Instance.PlayerName;
                highScoreText.text = SetHighScore();
                ScoreManager.Instance.SaveHighScore();
                ScoreManager.Instance.UploadHighScore();
            }
        }
    }
    private string SetHighScore() // returns a string using highscore data from score manager
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
    private string SetScore() // returns a string using current games score and player name
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
