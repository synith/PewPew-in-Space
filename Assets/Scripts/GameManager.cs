using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public SpawnManager spawnManager { get; private set; }

    // game state
    public bool gameStarted;
    public bool gamePaused;
    public bool gameOver;
    // score
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

        // Anything else in Awake goes after this
        spawnManager = FindObjectOfType<SpawnManager>();
    }  

    // public methods go here

}
