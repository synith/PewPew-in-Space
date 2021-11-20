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
    [SerializeField] private TMP_InputField inputName;
    [SerializeField] private TextMeshProUGUI highScoreText;
    [SerializeField] private GameObject highScoreScreen;
    [SerializeField] private GameObject settingsScreen;
    [SerializeField] private GameObject howToPlayScreen;
    [SerializeField] private GameObject enterNameObject;
    [SerializeField] private GameObject scoreTable;

    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    [SerializeField] private AudioClip menuSound;
    [SerializeField] private AudioClip errorSound;
    [SerializeField] private AudioClip gameMusic;

    private PlayFabManager playFabManager;
    private AudioSource menuAudio;
    private UIShake uiShake;
    private float menuVolume;

    private void Awake() // sets highscore text box to highscore from score manager
    {
        highScoreText.text = $"Your Best Score: {ScoreManager.Instance.HighScore}";
        menuAudio = GetComponent<AudioSource>();
        uiShake = GetComponent<UIShake>();
        playFabManager = ScoreManager.Instance.GetComponent<PlayFabManager>();

        inputName.characterLimit = 10;
        inputName.characterValidation = TMP_InputField.CharacterValidation.Name;
    }
    private void Start()
    {
        musicSlider.value = SoundManager.Instance.musicVolume;
        sfxSlider.value = SoundManager.Instance.sfxVolume;

        menuVolume = SoundManager.Instance.sfxVolume;
        StartCoroutine(SetPlayerNameToDisplayName());
    }

    private void OnEnable()
    {
        playFabManager.SetScoreTable(scoreTable.transform);
    }
    private IEnumerator SetPlayerNameToDisplayName()
    {
        yield return new WaitForSeconds(0.5f);
        inputName.text = ScoreManager.Instance.PlayerName;

    }
    private void PlaySound(AudioClip audioClip)
    {
        menuAudio.PlayOneShot(audioClip, menuVolume * 0.1f);
    }
    public void OnNameChanged()
    {
        ScoreManager.Instance.PlayerName = inputName.text;
    }
    public void StartNew() // start button - PlayerName string is assigned input name text value, then loads main game scene
    {

        if (string.IsNullOrEmpty(ScoreManager.Instance.PlayerName))
        {
            PlaySound(errorSound);
            uiShake.ShakeUI(enterNameObject);
            return;
        }
        else
        {
            PlaySound(menuSound);
            SoundManager.Instance.PlayMusic(gameMusic);
            ScoreManager.Instance.PlayerName = inputName.text;
            ScoreManager.Instance.UpdatePlayerName();
            SceneManager.LoadScene(1);
        }

    }
    public void Score()
    {
        ToggleScreen(highScoreScreen);
        if (highScoreScreen.activeInHierarchy)
            ScoreManager.Instance.DownloadHighScore();
    }
    public void Settings() => ToggleScreen(settingsScreen);
    public void HowToPlay() => ToggleScreen(howToPlayScreen);
    public void ToggleScreen(GameObject window)
    {
        PlaySound(menuSound);
        if (window.activeInHierarchy)
            window.SetActive(false);
        else
            window.SetActive(true);
    }
    public void UpdateSFXVolume()
    {        
        SoundManager.Instance.sfxVolume = sfxSlider.value;
        menuVolume = sfxSlider.value;
    }
    public void UpdateMusicVolume() => SoundManager.Instance.musicVolume = musicSlider.value;
    public void ExitGame() // exit button - saves high score and closes application
    {
        PlaySound(menuSound);
        ScoreManager.Instance.SaveHighScore();
        SoundManager.Instance.SaveVolumeSettings();

#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
}
