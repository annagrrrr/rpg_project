using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [System.Serializable]
    public class EnemySpawnInfo
    {
        public GameObject enemyPrefab;
        public string name;
        public int maxHealth;
        public int damage;
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
        if (spawnPoints.Count == 0 || enemySpawns.Count == 0) return;

        for (int i = 0; i < spawnPoints.Count; i++)
        {
            EnemySpawnInfo enemyInfo = enemySpawns[i % enemySpawns.Count];
            Transform spawnPoint = spawnPoints[i];

            GameObject enemyObject = Instantiate(enemyInfo.enemyPrefab, spawnPoint.position, Quaternion.identity);

            Enemy enemyComponent = enemyObject.GetComponent<Enemy>();
            HealthBar healthBar = enemyObject.GetComponentInChildren<HealthBar>();

            IAttack attackType = (enemyInfo.behaviourType == EnemyBehaviourType.Melee)
                ? new MeleeAttack()
                : new MagicAttack();

            if (enemyComponent != null)
            {
                IEnemyBehaviour behaviour = GetBehaviourByType(enemyInfo.behaviourType);

                enemyComponent.Initialize(enemyInfo.name, enemyInfo.maxHealth, enemyInfo.damage, enemyInfo.physicalResistance, enemyInfo.magicResistance, behaviour, attackType);

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
