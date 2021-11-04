using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

[DefaultExecutionOrder(1000)]
public class MenuUIHandler : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI inputName; // input box for player name (NOT WORKING CURRENTLY: look into input boxes and WebGL conflicts)
    [SerializeField] private TextMeshProUGUI highScoreText;
    [SerializeField] private GameObject highScoreScreen;
    [SerializeField] private GameObject settingsScreen;

    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    [SerializeField] private AudioClip menuSound;
    [SerializeField] private AudioClip gameMusic;

    private AudioSource menuAudio;
    private float menuVolume;

    private void Awake() // sets highscore text box to highscore from score manager
    {
        highScoreText.text = $"Best Score: {ScoreManager.Instance.HighScore} - {ScoreManager.Instance.HighScorePlayer}";
        menuAudio = GetComponent<AudioSource>();
        menuAudio.volume = SoundManager.Instance.sfxVolume;
        menuVolume = SoundManager.Instance.sfxVolume;
    }
    private void MenuSound()
    {
        menuAudio.PlayOneShot(menuSound, menuVolume * 0.1f);
    }
    public void StartNew() // start button - PlayerName string is assigned input name text value, then loads main game scene
    {
        MenuSound();
        SoundManager.Instance.PlayMusic(gameMusic);
        ScoreManager.Instance.PlayerName = inputName.text;
        SceneManager.LoadScene(1);
    }
    public void Score() // highscore button - toggles highscore UI window
    {
        MenuSound();
        if (highScoreScreen.activeInHierarchy)
            highScoreScreen.SetActive(false);
        else
            highScoreScreen.SetActive(true);
    }
    public void Settings()
    {
        MenuSound();
        if (settingsScreen.activeInHierarchy)
            settingsScreen.SetActive(false);
        else
            settingsScreen.SetActive(true);
    }
    private void Update()
    {
        SoundManager.Instance.musicVolume = musicSlider.value;
        SoundManager.Instance.sfxVolume = sfxSlider.value;
        menuVolume = sfxSlider.value;
    }
    public void ExitGame() // exit button - saves high score and closes application
    {
        MenuSound();
        ScoreManager.Instance.SaveHighScore();

#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
}
