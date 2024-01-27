using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSpawner : MonoBehaviour
{
    public GameObject PickupPrefab;
    public int MaxPickups;
    public float PickupSpawnInterval;
    public float SpawnMinX;
    public float SpawnMaxX;
    public float SpawnMinZ;
    public float SpawnMaxZ;

    private float _spawnTimer = 0f;
    private List<GameObject> _spawnedPickups;

    void Start()
    {
        _spawnedPickups = new List<GameObject>();
    }

    void Update()
    {
        if (_spawnTimer > PickupSpawnInterval
            && _spawnedPickups.Count < MaxPickups)
        {
            Spawn();
            _spawnTimer = 0f;
        }

        _spawnTimer += Time.deltaTime;
    }

    private void Spawn()
    {
        GameObject pickup = Instantiate(PickupPrefab, transform);
        pickup.transform.position =
            new Vector3(Random.Range(SpawnMinX, SpawnMaxX), 0f, Random.Range(SpawnMinZ, SpawnMaxZ));
        _spawnedPickups.Add(pickup);
    }

    public void NotifyDestroyedPickup(GameObject pickup)
    {
        _spawnedPickups.Remove(pickup);
    }
}
