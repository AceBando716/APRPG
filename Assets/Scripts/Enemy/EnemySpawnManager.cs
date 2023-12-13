using UnityEngine;
using System.Collections.Generic;

public class EnemySpawnManager : MonoBehaviour
{
    public GameObject enemyPrefab;  
    public Transform spawnPoint;    
    public int initialNumberOfEnemies = 4;

    private int currentWaveEnemyCount;
    private List<GameObject> enemies = new List<GameObject>();

    void Start()
    {
        currentWaveEnemyCount = initialNumberOfEnemies;
        SpawnEnemies(initialNumberOfEnemies);
    }

    void Update()
    {
        if (AllEnemiesDead())
        {
            currentWaveEnemyCount *= 2;  
            SpawnEnemies(currentWaveEnemyCount);
        }
    }

    void SpawnEnemies(int numberOfEnemies)
    {
         
        enemies.RemoveAll(item => item == null);

        for (int i = 0; i < numberOfEnemies; i++)
        {
            GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
            enemy.SetActive(true);
            enemies.Add(enemy);
        }
    }

    public void ResetWave()
    {
        foreach (GameObject enemy in enemies)
        {
            if (enemy != null)
            {
                Destroy(enemy);
            }
        }

        enemies.Clear();
        currentWaveEnemyCount = initialNumberOfEnemies;
        SpawnEnemies(currentWaveEnemyCount);

    }
    bool AllEnemiesDead()
    {
        foreach (GameObject enemy in enemies)
        {
            if (enemy != null)
            {
                return false;
            }
        }
        return true;
    }
}


