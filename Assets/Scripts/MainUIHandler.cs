using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainUIHandler : MonoBehaviour
{
    // UI screens for pause and game over
    [SerializeField] private GameObject PauseScreen;
    [SerializeField] private GameObject GameOverScreen;
    [SerializeField] private GameObject GameWonScreen;

    [SerializeField] private AudioClip menuSound;
    [SerializeField] private AudioClip pauseSound;
    [SerializeField] private AudioClip resumeSound;

    private AudioSource mainAudio;

    private void Awake()
    {
        mainAudio = GetComponent<AudioSource>();
        mainAudio.volume = SoundManager.Instance.sfxVolume;
    }
    private void Start() // on game start set pause and game over screens / gamestates to false
    {
        PauseScreen.SetActive(false);
        GameOverScreen.SetActive(false);
        GameManager.Instance.GamePaused = false;
        GameManager.Instance.GameOver = false;
    }

    private void Update() // checks game states and shows corresponding UI windows
    {
        if (GameManager.Instance.GamePaused && !PauseScreen.activeInHierarchy)
        {
            mainAudio.PlayOneShot(pauseSound, 0.1f);
            Time.timeScale = 0; // pauses game
            PauseScreen.SetActive(true);
        }
        if (GameManager.Instance.GameOver && !GameOverScreen.activeInHierarchy)
        {
            GameOverScreen.SetActive(true);
        }
        if (GameManager.Instance.GameWon && !GameWonScreen.activeInHierarchy) 
        {
            GameWonScreen.SetActive(true);
        }
    }

    public void ResumeGame() // resume game button - unpause
    {
        if (GameManager.Instance.GamePaused)
        {
            mainAudio.PlayOneShot(resumeSound, 0.1f);
            PauseScreen.SetActive(false);
            Time.timeScale = 1; // unpauses game
            GameManager.Instance.GamePaused = false;
        }
    }
    public void GoToMenu() // menu button - load menu scene
    {
        ResumeGame();
        SoundManager.Instance.StopMusic();
        SceneManager.LoadScene(0);
    }
    public void RestartGame() // restart button - reload current scene
    {
        ResumeGame();
        SceneManager.LoadScene(1);
    }

    public void StartGame() // start button
    {
        // code to close Start UI window after showing player controls and intro message (this might help with abrupt start and room 0 spawns)
    }
}
