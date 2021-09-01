using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
// singleton that manages saving and loading high score data as a json file
public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; } // ENCAPSULATION

    // ENCAPSULATION
    public string PlayerName { get; set; }
    public string HighScorePlayer { get; set; }
    public int HighScore { get; set; }

    private void Awake() // check if this class exists already, and destroys this instance of the class if it does
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        LoadHighScore();
    }

    [System.Serializable]
    class HighScoreData // new class that stores name and score of highscore player
    {
        public string HighScorePlayer;
        public int HighScore;
    }

    public void SaveHighScore() // writes highscore data class to a json file
    {
        HighScoreData data = new HighScoreData();
        data.HighScore = HighScore;
        data.HighScorePlayer = HighScorePlayer;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/highscorefile.json", json);
    }

    public void LoadHighScore() // reads highscore data class from a json file
    {
        string path = Application.persistentDataPath + "/highscorefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            HighScoreData data = JsonUtility.FromJson<HighScoreData>(json);

            HighScore = data.HighScore;
            HighScorePlayer = data.HighScorePlayer;
        }
    }
}
