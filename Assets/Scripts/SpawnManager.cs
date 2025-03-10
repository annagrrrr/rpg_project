using NUnit.Framework;
using System.Collections.Generic;
using Unity.Jobs;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [System.Serializable]
    public class EnemySpawnInfo
    {
        public GameObject enemyPrefab;
        public string name;
        public int maxHealth;
        public float physicalResistance;
        public float magicResistance;
    }

    public List<EnemySpawnInfo> enemySpawns = new List<EnemySpawnInfo>();
    public List<Transform> spawnPoints;

    private void Start()
    {
        SpawnEnemies();
        Debug.Log("вызвал");
    }
    private void SpawnEnemies()
    {
        if (spawnPoints.Count == 0 || enemySpawns.Count == 0) return;

        for (int i = 0; i < spawnPoints.Count; i++)
        {
            EnemySpawnInfo enemyInfo = enemySpawns[i % enemySpawns.Count];

            Transform spawnPoint = spawnPoints[i];

            GameObject enemyObject = Instantiate(enemyInfo.enemyPrefab, spawnPoint.position, Quaternion.identity);

            Enemy enemyComponent = enemyObject.GetComponent<Enemy>();
            if (enemyComponent != null )
            {
                enemyComponent.Initialize(enemyInfo.name, enemyInfo.maxHealth, enemyInfo.physicalResistance, enemyInfo.magicResistance);
                Debug.Log($"{enemyInfo.name} заспавнился в точке {spawnPoint.position}");
            }
        }
    }
}
