using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject EnemyPrefab;
    public float SpawnInterval = 5f;
    public bool RandomSpawnDelay = true;

    void Start()
    {
        float delay = 0f;
        if (RandomSpawnDelay)
            delay = Random.Range(0f, SpawnInterval);

        InvokeRepeating("SpawnEnemy", delay, SpawnInterval);
    }

    public void SpawnEnemy()
    {
        GameObject enemy = Instantiate(EnemyPrefab, transform.position, Quaternion.identity);
    }
}
