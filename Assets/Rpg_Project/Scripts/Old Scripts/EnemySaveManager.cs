using System.Collections.Generic;
using UnityEngine;

public class EnemySaveManager : MonoBehaviour
{
    public static EnemySaveManager Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void SaveEnemies()
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        List<EnemySaveData> enemyDataList = new List<EnemySaveData>();

        foreach (Enemy enemy in enemies)
        {
            EnemySaveData data = new EnemySaveData
            {
                enemyID = enemy.EnemyID,
                posX = enemy.transform.position.x,
                posY = enemy.transform.position.y,
                posZ = enemy.transform.position.z,
                health = enemy.Health.currentHealth,
                isDead = enemy.Health.currentHealth <= 0
            };
            enemyDataList.Add(data);
        }

        string json = JsonUtility.ToJson(new EnemySaveListWrapper { enemies = enemyDataList });
        PlayerPrefs.SetString("EnemySaveData", json);
        PlayerPrefs.Save();
    }

    public void LoadEnemies()
    {
        string json = PlayerPrefs.GetString("EnemySaveData", "");
        if (string.IsNullOrEmpty(json)) return;

        EnemySaveListWrapper wrapper = JsonUtility.FromJson<EnemySaveListWrapper>(json);
        if (wrapper == null || wrapper.enemies == null) return;

        Enemy[] enemies = FindObjectsOfType<Enemy>();

        foreach (EnemySaveData data in wrapper.enemies)
        {
            Enemy match = System.Array.Find(enemies, e => e.EnemyID == data.enemyID);
            if (match != null)
            {
                match.transform.position = new Vector3(data.posX, data.posY, data.posZ);
                match.Health.SetHealth(data.health);

                if (data.isDead)
                    match.gameObject.SetActive(false); 
            }
        }
    }
}

[System.Serializable]
public class EnemySaveListWrapper
{
    public List<EnemySaveData> enemies;
}
