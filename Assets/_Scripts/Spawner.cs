using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject obstaclePrefabs; 
    [SerializeField] private float minSpawnDelay = 1f;
    [SerializeField] private float maxSpawnDelay = 5f;

    private float enemySpawnTime = 2f;
    private float timeUntilEnemySpawn;

    private void Update()
    {
        SpawnLoop();
    }

    private void SpawnLoop()
    {
        timeUntilEnemySpawn += Time.deltaTime;
        if (timeUntilEnemySpawn >= enemySpawnTime)
        {
            Spawn();
            RandomSpawnTime();
            timeUntilEnemySpawn = 0f;
        }
    }

    private void Spawn()
    {

        GameObject spawnedObstacle = Instantiate(obstaclePrefabs, transform.position, transform.rotation);
    }

    private void RandomSpawnTime() 
    { 
        enemySpawnTime = Random.Range(minSpawnDelay, maxSpawnDelay);
    }
}