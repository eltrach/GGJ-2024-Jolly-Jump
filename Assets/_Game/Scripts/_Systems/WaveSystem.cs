using Enemy;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public class WaveSystem : Singleton<WaveSystem>
{
    public ThirdPersonEnemy enemyPrefab;
    public List<GameObject> spawnPoints;

    [SerializeField, MinValue(1)]
    private int initialEnemiesPerWave = 3;

    [SerializeField, MinValue(1)]
    private int enemiesPerWaveIncrease = 2;

    [SerializeField, MinValue(0)]
    private float timeBetweenWaves = 5f;

    private int currentWave = 0;
    private int enemiesSpawned = 0;

    [SerializeField] private List<ThirdPersonEnemy> spawnedAIs = new List<ThirdPersonEnemy>();

    private void Start()
    {
        Init();
    }
    private void Init()
    {
        StartNextWave();
    }

    [Button()]
    private void StartNextWave()
    {
        enemiesSpawned = 0;
        SpawnEnemies(initialEnemiesPerWave + (enemiesPerWaveIncrease * (currentWave - 1)));
        currentWave++;
    }

    private void SpawnEnemies(int numEnemies)
    {
        RefreshSpawnedList();
        for (int i = 0; i < numEnemies; i++)
        {
            GameObject spawnPoint = GetRandomSpawnPoint();
            if (spawnPoint != null)
            {
                spawnedAIs.Add( Instantiate(enemyPrefab, spawnPoint.transform.position, Quaternion.identity) );
                enemiesSpawned++;
            }
        }
    }
    private GameObject GetRandomSpawnPoint()
    {
        if (spawnPoints.Count > 0)
        {
            int randomIndex = Random.Range(0, spawnPoints.Count);
            return spawnPoints[randomIndex];
        }
        return null;
    }

    public void OnEnemyDeath()
    {
        enemiesSpawned--;
        RefreshSpawnedList();

        if (enemiesSpawned <= 0)
        {
            Invoke(nameof(StartNextWave), timeBetweenWaves);
        }
    }

    public void RefreshSpawnedList()
    {
        Debug.Log("Refresh");
        spawnedAIs.RemoveAll(item => item == null);
    }

    [Button]
    public void Reset()
    {
        RefreshSpawnedList();
        for (int i = 0; i < spawnedAIs.Count; i++)
        {
            Destroy(spawnedAIs[i].gameObject);
        }
        spawnedAIs.Clear();
        currentWave = 0;
        enemiesSpawned = 0;
        Init();
    }
}


