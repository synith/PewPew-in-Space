using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private readonly Transform[][] Rooms = new Transform[5][];

    // array of spawn positions for each room
    [SerializeField] private Transform[] spawnPositionsRoom0;
    [SerializeField] private Transform[] spawnPositionsRoom1;
    [SerializeField] private Transform[] spawnPositionsRoom2;
    [SerializeField] private Transform[] spawnPositionsRoom3;
    [SerializeField] private Transform[] spawnPositionsRoom4;

    [SerializeField] private GameObject fighterPrefab;  // enemy fighter starship

    private void Awake()
    {
        Rooms[0] = spawnPositionsRoom0;
        Rooms[1] = spawnPositionsRoom1;
        Rooms[2] = spawnPositionsRoom2;
        Rooms[3] = spawnPositionsRoom3;
        Rooms[4] = spawnPositionsRoom4;
    }
    private void Start()
    {
        SpawnFighter(0); // spawns enemies in first room on game start
        CountAllEnemies();
    }
    public void SpawnFighter(int roomIndex) // spawns enemies in specified room
    {
        Transform[] room = Rooms[roomIndex];
        foreach (Transform pos in room)
            Instantiate(fighterPrefab, pos);
    }
    private void CountAllEnemies()
    {
        foreach (Transform[] transforms in Rooms)
        {
            GameManager.Instance.TotalEnemies += transforms.Length;
        }
    }
}
