using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public int CurrentRoomNumber { get; set; }

    private readonly Transform[][] Rooms = new Transform[5][];

    // array of spawn positions for each room
    [SerializeField] private Transform[] spawnPositionsRoom0;
    [SerializeField] private Transform[] spawnPositionsRoom1;
    [SerializeField] private Transform[] spawnPositionsRoom2;
    [SerializeField] private Transform[] spawnPositionsRoom3;
    [SerializeField] private Transform[] spawnPositionsRoom4;

    public GameObject[] DoorsClosed { get; private set; }

    [SerializeField]
    GameObject
        doorReplacement1,
        doorReplacement2,
        doorReplacement3,
        doorReplacement4;

    [SerializeField] private GameObject fighterPrefab;  // enemy fighter starship

    private void Awake()
    {
        Rooms[0] = spawnPositionsRoom0;
        Rooms[1] = spawnPositionsRoom1;
        Rooms[2] = spawnPositionsRoom2;
        Rooms[3] = spawnPositionsRoom3;
        Rooms[4] = spawnPositionsRoom4;

        DoorsClosed = new GameObject[4];

        DoorsClosed[0] = doorReplacement1;
        DoorsClosed[1] = doorReplacement2;
        DoorsClosed[2] = doorReplacement3;
        DoorsClosed[3] = doorReplacement4;
    }
    private void Start()
    {
        SpawnFighterInRoom(0); // spawns enemies in first room on game start
        CountAllEnemies();
        CloseAllDoors();
    }
    public void OpenDoor(int room)
    {
        if (room >= DoorsClosed.Length)
        {
            return;
        }
        DoorsClosed[room].SetActive(false);
    }
    private void CloseAllDoors()
    {
        foreach (GameObject item in DoorsClosed)
        {
            item.SetActive(true);
        }
    }
    public void SpawnFighterInRoom(int roomIndex) // spawns enemies in specified room
    {
        Transform[] room = Rooms[roomIndex];
        foreach (Transform pos in room)
            Instantiate(fighterPrefab, pos);
    }
    public int EnemyTotalCountInRoom(int roomIndex)
    {
        int enemyCount;
        enemyCount = Rooms[roomIndex].Length;
        return enemyCount;
    }
    private void CountAllEnemies()
    {
        foreach (Transform[] transforms in Rooms)
        {
            GameManager.Instance.TotalEnemies += transforms.Length;
        }
    }   
}
