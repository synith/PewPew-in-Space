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
    private bool door5Open;

    private void Start() // set all doors to closed on start
    {
        door1Open = false;
        door2Open = false;
        door3Open = false;
        door4Open = false;
        door5Open = false;
    }

    private void OnTriggerEnter(Collider other) // checking if player triggered door
    {
        if (other.CompareTag("Door"))
        {
            CheckDoor(other);
        }
    }
    private void CheckDoor(Collider other) // checks which numbered door player entered and spawns fighters in the appropriate room
    {
        int room;
        if (other.name == "Door1" && !door1Open)
        {
            room = 1;
            door1Open = true;
            GameManager.Instance.SpawnManager.SpawnFighter(room);
        }
        else if (other.name == "Door2" && !door2Open)
        {
            room = 2;
            door2Open = true;
            GameManager.Instance.SpawnManager.SpawnFighter(room);
        }
        else if (other.name == "Door3" && !door3Open)
        {
            room = 3;
            door3Open = true;
            GameManager.Instance.SpawnManager.SpawnFighter(room);
        }
        else if (other.name == "Door4" && !door4Open)
        {
            room = 4;
            door4Open = true;
            GameManager.Instance.SpawnManager.SpawnFighter(room);
        }
        else if (other.name == "Door5" && !door5Open)
        {
            room = 5;
            door5Open = true;
            GameManager.Instance.SpawnManager.SpawnFighter(room);
        }
    }
}
