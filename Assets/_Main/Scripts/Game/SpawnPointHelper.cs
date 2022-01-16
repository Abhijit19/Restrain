using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointHelper : MonoBehaviour
{
    public Transform[] spawnPoints;

    public Transform GetSpawnPoint()
    {
        int randomspawn = Random.Range(0, spawnPoints.Length);
        return spawnPoints[randomspawn];
    }

    public Transform GetSpawnPoint(int index)
    {
        index = index % spawnPoints.Length;
        return spawnPoints[index];
    }
}
