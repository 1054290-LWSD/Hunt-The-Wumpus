using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class EventHandler : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform player;
    public Text enemyCountText;

    private int numberOfEnemies = 5;
    private float spawnRadius = 150f;

    public List<GameObject> spawnedEnemies = new List<GameObject>();

    void Start()
    {
        SpawnEnemies();
    }

    public void SpawnEnemies()
    {
        spawnedEnemies.Clear();

        for (int i = 0; i < numberOfEnemies; i++)
        {
            float angle = Random.Range(0f, 360f);
            float radius = Random.Range(spawnRadius * 0.5f, spawnRadius);
            Vector3 spawnPos = player.position + new Vector3(Mathf.Cos(angle) * radius, 10, Mathf.Sin(angle) * radius);

            GameObject enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
            spawnedEnemies.Add(enemy);

            EnemyHandler handler = enemy.GetComponent<EnemyHandler>();
            if (handler != null)
            {
                handler.SetSpawner(this);
            }
        }

        UpdateEnemyCount();
    }

    public void RemoveEnemy(GameObject enemy)
    {
        if (spawnedEnemies.Contains(enemy))
        {
            spawnedEnemies.Remove(enemy);
            UpdateEnemyCount();
        }
    }

    public void UpdateEnemyCount()
    {
        if (enemyCountText != null)
        {
            if (spawnedEnemies.Count > 0)
                enemyCountText.text = "Enemies Left: " + spawnedEnemies.Count;
            else
                enemyCountText.text = "All Enemies Defeated!";
        }
    }
}
