using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] asteroidPrefabs;
    [SerializeField] private float secondsBetweenAsteroids = 1.5f;
    [SerializeField] private Vector2 forceRange;

    private Camera mainCamera;
    private float timer;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            SpawnAsteroid();
            SpawnAsteroid();
            timer += secondsBetweenAsteroids;
        }
    }

    private void SpawnAsteroid()
    {
        GameObject selectedAsteroid = asteroidPrefabs[Random.Range(0, asteroidPrefabs.Length)];
        var result = Spawn();
        Vector3 worldSpawnPoint = result[0];
        GameObject asteroidInstance = Instantiate(
            selectedAsteroid,
            worldSpawnPoint,
            Quaternion.Euler(0f, 0f, Random.Range(0f, 360f)));
        Rigidbody rb = asteroidInstance.GetComponent<Rigidbody>();
        Vector3 direction = result[1];
        rb.velocity = direction.normalized * Random.Range(forceRange.x, forceRange.y);
    }

    private Vector3[] Spawn()
    {
        int side = Random.Range(0, 4);
        Vector3 spawnPoint = Vector3.zero;
        Vector3 direction = Vector3.zero;
        
        switch (side)
        {
            case 0:
                // Left
                spawnPoint.x = 0;
                spawnPoint.y = Random.value;
                direction = new Vector3(1f, Random.Range(-1f, 1f),0f);
                break;
            case 1:
                // Right
                spawnPoint.x = 1;
                spawnPoint.y = Random.value;
                direction = new Vector3(-1f, Random.Range(-1f, 1f),0f);
                break;
            case 2:
                // Bottom
                spawnPoint.x = Random.value;
                spawnPoint.y = 0;
                direction = new Vector3(Random.Range(-1f, 1f), 1f,0f);
                break;
            case 3:
                // Top
                spawnPoint.x = Random.value;
                spawnPoint.y = 1;
                direction = new Vector3(Random.Range(-1f, 1f), -1f,0f);
                break;
        }
        spawnPoint.z = 10f;       
        Vector3 worldSpawnPoint = mainCamera.ViewportToWorldPoint(spawnPoint);

        Vector3[] result = new Vector3[2];
        result[0] = worldSpawnPoint;
        result[1] = direction;
        return result;
    }
}
