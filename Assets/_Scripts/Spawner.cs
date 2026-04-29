using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spawner : MonoBehaviour
{
    [System.Serializable]
    public class SpawnableObject
    {
        public GameObject enemyPrefab;
        public int amoutOfEnemy;
    }

    [SerializeField] private SpawnableObject[] objects;
    [SerializeField] private Transform[] spawnPos;
    [SerializeField] private Slider progressSlider;
    [SerializeField] private float minSpawnDelay = 1f;
    [SerializeField] private float maxSpawnDelay = 5f;

    private List<GameObject> spawnPool = new List<GameObject>();
    private List<GameObject> currentEnemies = new List<GameObject>();

    private float enemySpawnTime;
    private float timeUntilEnemySpawn;

    private int totalEnemies;
    private int spawnedEnemies;

    private void Start()
    {
        if (DebugData.enemy0 > 0)
            objects[0].amoutOfEnemy = DebugData.enemy0;

        if (DebugData.enemy1 > 0)
            objects[1].amoutOfEnemy = DebugData.enemy1;

        if (DebugData.enemy2 > 0)
            objects[2].amoutOfEnemy = DebugData.enemy2;

        if (DebugData.enemy3 > 0)
            objects[3].amoutOfEnemy = DebugData.enemy3;

        BuildSpawnPool();
        UpdateProgressSlider();
        RandomSpawnTime();
    }

    private void Update()
    {
        SpawnLoop();

        if (spawnPool.Count == 0 && currentEnemies.Count == 0)
        {
            UpdateProgressSlider();
            GameManager.Instance.Win();
            enabled = false;
        }
    }

    private void BuildSpawnPool()
    {
        spawnPool.Clear();
        totalEnemies = 0;

        foreach (var obj in objects)
        {
            for (int i = 0; i < obj.amoutOfEnemy; i++)
            {
                spawnPool.Add(obj.enemyPrefab);
                totalEnemies++;
            }
        }
    }

    private void SpawnLoop()
    {
        if (spawnPool.Count == 0) return;

        timeUntilEnemySpawn += Time.deltaTime;

        if (timeUntilEnemySpawn >= enemySpawnTime)
        {
            Spawn();
            RandomSpawnTime();
            timeUntilEnemySpawn = 0f;
            UpdateProgressSlider();
        }
    }

    private void Spawn()
    {
        int index = Random.Range(0, spawnPool.Count);
        GameObject prefab = spawnPool[index];
        spawnPool.RemoveAt(index);

        Transform randomSpawnPos = spawnPos[Random.Range(0, spawnPos.Length)];
        GameObject enemy = Instantiate(prefab, randomSpawnPos.position, randomSpawnPos.rotation);

        currentEnemies.Add(enemy);
        spawnedEnemies++;

        var enemyHealth = enemy.GetComponent<EnemyHealth>();
        if (enemyHealth != null)
        {
            enemyHealth.OnDeath += RemoveEnemy;
        }
    }

    private void RemoveEnemy(GameObject enemy)
    {
        currentEnemies.Remove(enemy);

        var enemyHealth = enemy.GetComponent<EnemyHealth>();
        if (enemyHealth != null)
        {
            enemyHealth.OnDeath -= RemoveEnemy;
        }
    }

    private void RandomSpawnTime()
    {
        enemySpawnTime = Random.Range(minSpawnDelay, maxSpawnDelay);
    }

    private void UpdateProgressSlider()
    {
        if (totalEnemies > 0)
        {
            progressSlider.value = (float)spawnedEnemies / totalEnemies;
        }
    }

    // test 
    public void SetEnemyAmount(int index, int amount)
    {
        if (index < 0 || index >= objects.Length) return;

        objects[index].amoutOfEnemy = amount;
    }

    public void Rebuild()
    {
        BuildSpawnPool();
        spawnedEnemies = 0;
        UpdateProgressSlider();
    }
}