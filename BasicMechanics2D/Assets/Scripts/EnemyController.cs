using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public GameObject EnemyPrefab;
    public float minSpawnTime = 2f;
    public float maxSpawnTime = 6f;
    public float minSpawnLocationX = 0f;
    public float maxSpawnLocationXR = 15f;
    public float minSpawnLocationXL = 19f;
    public float maxSpawnLocationX = 30f;
    private float nexSpawnTime;

    void Start()
    {
        nexSpawnTime = Time.time + Random.Range(minSpawnTime, maxSpawnTime);
    }


    void Update()
    {
        if (Time.time >= nexSpawnTime)
        {
            SpawnObject();
            nexSpawnTime = Time.time + Random.Range(minSpawnTime, maxSpawnTime);
        }
    }


    private void SpawnObject()
    {
        Vector2 spawnPoint;
        float random = Random.Range(0f, 2f);

        if ((int)random == 0)
        {
            spawnPoint = new Vector2(Random.Range(minSpawnLocationX, maxSpawnLocationXR), 0f);
        }
        else
        {
            spawnPoint = new Vector2(Random.Range(minSpawnLocationXL, maxSpawnLocationX), 0f);
        }

        Instantiate(EnemyPrefab, spawnPoint, Quaternion.identity);
    }
}

