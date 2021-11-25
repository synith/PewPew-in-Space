using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnDoorTrigger : MonoBehaviour
{
    // bools flagging whether door has been opened or not
    private bool door1Open;
    private bool door2Open;
    private bool door3Open;
    private bool door4Open;

    private SpawnManager spawnManager;

    private void Start() // set all doors to closed on start
    {
        spawnManager = GameManager.Instance.SpawnManager;
        door1Open = false;
        door2Open = false;
        door3Open = false;
        door4Open = false;

        spawnManager.CurrentRoomNumber = 0;
        CountEnemiesInRoom(spawnManager.CurrentRoomNumber);
    }

    private void OnTriggerEnter(Collider other) // checking if player triggered door
    {
        if (other.CompareTag("Door"))
        {
            CheckDoor(other);
        }
    }
    private void CloseDoorBehindPlayer(int room) => spawnManager.DoorsClosed[room - 1].SetActive(true);
    private void CountEnemiesInRoom(int room) => GameManager.Instance.TotalEnemiesInRoom = spawnManager.EnemyTotalCountInRoom(room);
    private void OpenNextRoom(int room)
    {
        CloseDoorBehindPlayer(room);
        spawnManager.SpawnFighterInRoom(room);
        CountEnemiesInRoom(room);
        spawnManager.CurrentRoomNumber = room;
        GameManager.Instance.EnemiesDefeatedInRoom = 0;
    }

    private void CheckDoor(Collider other) // checks which numbered door player entered and spawns fighters in the appropriate room
    {
        int room;
        if (other.name == "Door1" && !door1Open)
        {
            room = 1;
            door1Open = true;
            OpenNextRoom(room);
        }
        else if (other.name == "Door2" && !door2Open)
        {
            room = 2;
            door2Open = true;
            OpenNextRoom(room);
        }
        else if (other.name == "Door3" && !door3Open)
        {
            room = 3;
            door3Open = true;
            OpenNextRoom(room);
        }
        else if (other.name == "Door4" && !door4Open)
        {
            room = 4;
            door4Open = true;
            OpenNextRoom(room);
        }
    }
}
