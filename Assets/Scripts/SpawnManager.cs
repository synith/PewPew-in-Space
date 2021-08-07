using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPositionsRoom0;
    [SerializeField] private Transform[] spawnPositionsRoom1;
    [SerializeField] private Transform[] spawnPositionsRoom2;
    [SerializeField] private Transform[] spawnPositionsRoom3;
    [SerializeField] private Transform[] spawnPositionsRoom4;
    [SerializeField] private GameObject fighterPrefab;

    private void Start()
    {
        SpawnFighter(0);
    }

    public void SpawnFighter(int room)
    {
        if (room == 0)
        {
            foreach (Transform pos in spawnPositionsRoom0)
            {
                Instantiate(fighterPrefab, pos);
            }
        }
        if (room == 1)
        {
            foreach (Transform pos in spawnPositionsRoom1)
            {
                Instantiate(fighterPrefab, pos);
            }
        }
        if (room == 2)
        {
            foreach (Transform pos in spawnPositionsRoom2)
            {
                Instantiate(fighterPrefab, pos);
            }
        }
        if (room == 3)
        {
            foreach (Transform pos in spawnPositionsRoom3)
            {
                Instantiate(fighterPrefab, pos);
            }
        }
        if (room == 4)
        {
            foreach (Transform pos in spawnPositionsRoom4)
            {
                Instantiate(fighterPrefab, pos);
            }
        }
    }

}
