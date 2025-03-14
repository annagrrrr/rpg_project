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
        public EnemyBehaviourType behaviourType;
    }

    public List<EnemySpawnInfo> enemySpawns = new List<EnemySpawnInfo>();
    public List<Transform> spawnPoints;

    private void Start()
    {
        SpawnEnemies();
    }
    private void SpawnEnemies()
    {
        Debug.Log("efregreg");
        if (spawnPoints.Count == 0 || enemySpawns.Count == 0) return;

        for (int i = 0; i < spawnPoints.Count; i++)
        {
            EnemySpawnInfo enemyInfo = enemySpawns[i % enemySpawns.Count];

            Transform spawnPoint = spawnPoints[i];

            GameObject enemyObject = Instantiate(enemyInfo.enemyPrefab, spawnPoint.position, Quaternion.identity);

            Enemy enemyComponent = enemyObject.GetComponent<Enemy>();
            if (enemyComponent != null )
            {
                IEnemyBehaviour behaviour = GetBehaviourByType(enemyInfo.behaviourType);

                enemyComponent.Initialize(enemyInfo.name, enemyInfo.maxHealth, enemyInfo.physicalResistance, enemyInfo.magicResistance, behaviour);
                Debug.Log($"{enemyInfo.name} заспавнился в точке {spawnPoint.position} с поведением {enemyInfo.behaviourType}");
            }
        }
    }

    private IEnemyBehaviour GetBehaviourByType(EnemyBehaviourType type)
    {
        switch (type)
        {
            case EnemyBehaviourType.Melee:
                return new MeleeBehavior();
            case EnemyBehaviourType.Ranged:
                return new RangedBehaviour();
            default:
                return new MeleeBehavior();
        }
    }
}
