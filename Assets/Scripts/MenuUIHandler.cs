using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

[DefaultExecutionOrder(1000)]
public class MenuUIHandler : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI inputName; // input box for player name (NOT WORKING CURRENTLY: look into input boxes and WebGL conflicts)
    [SerializeField] private TextMeshProUGUI highScoreText;
    [SerializeField] private GameObject highScoreScreen;

    private void Awake() // sets highscore text box to highscore from score manager
    {
        highScoreText.text = $"Best Score: {ScoreManager.Instance.HighScore} - {ScoreManager.Instance.HighScorePlayer}";
    }
    public void StartNew() // start button - PlayerName string is assigned input name text value, then loads main game scene
    {
        ScoreManager.Instance.PlayerName = inputName.text;
        SceneManager.LoadScene(1);
    }
    public void Score() // highscore button - toggles highscore UI window
    {
        if (highScoreScreen.activeInHierarchy)
            highScoreScreen.SetActive(false);
        else
            highScoreScreen.SetActive(true);
    }
    public void ExitGame() // exit button - saves high score and closes application
    {
        ScoreManager.Instance.SaveHighScore();

#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
}
