using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public string PlayerName;
    public string HighScorePlayer;
    public int HighScore;

    private void Awake()
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
    class HighScoreData
    {
        public string HighScorePlayer;
        public int HighScore;
    }

    public void SaveHighScore()
    {
        HighScoreData data = new HighScoreData();
        data.HighScore = HighScore;
        data.HighScorePlayer = HighScorePlayer;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/highscorefile.json", json);
    }

    public void LoadHighScore()
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
