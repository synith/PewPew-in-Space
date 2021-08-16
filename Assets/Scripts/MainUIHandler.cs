using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainUIHandler : MonoBehaviour
{
    [SerializeField] private GameObject PauseScreen;
    [SerializeField] private GameObject GameOverScreen;

    private void Start()
    {
        PauseScreen.SetActive(false);
        GameOverScreen.SetActive(false);
        GameManager.Instance.gamePaused = false;
        GameManager.Instance.gameOver = false;
    }

    private void Update()
    {
        if (GameManager.Instance.gamePaused && !PauseScreen.activeInHierarchy)
        {
            Time.timeScale = 0;
            PauseScreen.SetActive(true);
        }
        if (GameManager.Instance.gameOver && !GameOverScreen.activeInHierarchy)
        {
            GameOverScreen.SetActive(true);
        }
    }

    public void ResumeGame()
    {
        if (GameManager.Instance.gamePaused)
        {
            PauseScreen.SetActive(false);
            Time.timeScale = 1;
            GameManager.Instance.gamePaused = false;
        }
    }
    public void GoToMenu()
    {
        ResumeGame();
        SceneManager.LoadScene(0);
    }
    public void RestartGame()
    {
        ResumeGame();
        SceneManager.LoadScene(1);
    }
}
