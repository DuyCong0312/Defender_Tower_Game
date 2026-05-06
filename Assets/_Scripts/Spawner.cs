using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Spawner : MonoBehaviour
{
    [System.Serializable]
    public class SpawnableObject
    {
        public GameObject enemyPrefab;
        public int amountOfEnemy;
    }

    [System.Serializable]
    public class Wave
    {
        public string waveName = "Wave";

        public SpawnableObject[] ambushEnemies;
        public SpawnableObject[] waveEnemies;

        public float waveBurstSpawnDelay = 0.3f;

        [HideInInspector] public bool hasTriggered = false;
        [HideInInspector] public bool isBurstDone = false;
        [HideInInspector] public List<GameObject> ambushPool = new List<GameObject>();
        [HideInInspector] public List<GameObject> wavePool = new List<GameObject>();
    }

    [SerializeField] private Wave[] waves;
    [SerializeField] private Transform[] spawnPos;
    [SerializeField] private Slider progressSlider;
    [SerializeField] private GameObject waveAnnoucement;
    [SerializeField] private TextMeshProUGUI waveAnnoucementText;

    [SerializeField] private float minSpawnDelay = 1f;
    [SerializeField] private float maxSpawnDelay = 5f;

    private List<GameObject> currentEnemies = new List<GameObject>();

    private float[] ambushTimers;
    private float[] ambushIntervals;

    private int totalEnemies = 0; 
    private int spawnedEnemies = 0;

    private int currentWaveIndex = 0;

    private bool allWavesTriggered = false;
    private bool gameEnded = false;

    private void Start()
    {
        BuildAllPools(); 
        InitAmbushTimers();
        UpdateProgressSlider();
    }

    private void Update()
    {
        if (gameEnded) return;

        CheckWaveTriggers();
        TickAmbushSpawners();
        CheckWinCondition();
    }

    private void BuildAllPools()
    {
        totalEnemies = 0;

        foreach (var wave in waves)
        {
            wave.ambushPool.Clear();
            wave.wavePool.Clear();

            foreach (var obj in wave.ambushEnemies)
                for (int i = 0; i < obj.amountOfEnemy; i++)
                {
                    wave.ambushPool.Add(obj.enemyPrefab);
                    totalEnemies++;
                }

            foreach (var obj in wave.waveEnemies)
                for (int i = 0; i < obj.amountOfEnemy; i++)
                {
                    wave.wavePool.Add(obj.enemyPrefab);
                    totalEnemies++;
                }
        }
    }

    private void InitAmbushTimers()
    {
        ambushTimers = new float[waves.Length];
        ambushIntervals = new float[waves.Length];

        for (int i = 0; i < waves.Length; i++)
            ambushIntervals[i] = Random.Range(minSpawnDelay, maxSpawnDelay);
    }

    private void CheckWaveTriggers()
    {
        if (currentWaveIndex >= waves.Length)
        {
            allWavesTriggered = true;
            return;
        }

        Wave wave = waves[currentWaveIndex];
        if (!wave.hasTriggered  && currentEnemies.Count == 0 && wave.ambushPool.Count == 0)
        {
            TriggerWave(currentWaveIndex);
            currentWaveIndex++;
        }
    }

    private void TickAmbushSpawners()
    {
        if (currentWaveIndex >= waves.Length) return;

        Wave wave = waves[currentWaveIndex];

        if (wave.hasTriggered) return;
        if (wave.ambushPool.Count == 0) return;

        ambushTimers[currentWaveIndex] += Time.deltaTime;

        if (ambushTimers[currentWaveIndex] >= ambushIntervals[currentWaveIndex])
        {
            SpawnOneFrom(wave.ambushPool);
            ambushTimers[currentWaveIndex] = 0f;
            ambushIntervals[currentWaveIndex] = Random.Range(minSpawnDelay, maxSpawnDelay);
        }
    }

    private void CheckWinCondition()
    {
        if (!allWavesTriggered) return;

        foreach (var wave in waves)
            if (!wave.isBurstDone) return;

        if (currentEnemies.Count == 0)
        {
            gameEnded = true;
            progressSlider.value = 1f;
            GameManager.Instance.Win();
        }
    }

    private void TriggerWave(int waveIndex)
    {
        Wave wave = waves[waveIndex];
        wave.hasTriggered = true;

        wave.wavePool.AddRange(wave.ambushPool);
        wave.ambushPool.Clear();

        StartCoroutine(BurstSpawn(wave));
    }

    private IEnumerator BurstSpawn(Wave wave)
    {
        waveAnnoucement.SetActive(true);
        waveAnnoucementText.text = wave.waveName + "Starts";
        yield return new WaitForSeconds(1f);
        waveAnnoucement.SetActive(false);
        while (wave.wavePool.Count > 0)
        {
            SpawnOneFrom(wave.wavePool);
            yield return new WaitForSeconds(wave.waveBurstSpawnDelay);
        }
        wave.isBurstDone = true;
    }

    private void SpawnOneFrom(List<GameObject> pool)
    {
        int index = Random.Range(0, pool.Count);
        GameObject prefab = pool[index];
        pool.RemoveAt(index);

        Transform spawnPoint = spawnPos[Random.Range(0, spawnPos.Length)];
        GameObject enemy = Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);

        currentEnemies.Add(enemy); 
        spawnedEnemies++;
        UpdateProgressSlider();

        var health = enemy.GetComponent<EnemyHealth>();
        if (health != null)
            health.OnDeath += RemoveEnemy;
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

    private void UpdateProgressSlider()
    {
        if (totalEnemies > 0)
        {
            progressSlider.value = (float)spawnedEnemies / totalEnemies;
        }
    }
}