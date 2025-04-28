using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventHandler : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform player;  // Drag the player Transform here
    private int numberOfEnemies = 100;
    private float minSpawnDistance = 50f;
    private float maxSpawnDistance = 100f;
    public List<GameObject> spawnedEnemies = new List<GameObject>();
    void Start()
    {
        SpawnEnemies(numberOfEnemies, minSpawnDistance, maxSpawnDistance);
    }
    public void SpawnEnemies(int numEnemies, float minDistance, float maxDistance)
    {
        for (int i = 0; i < numEnemies; i++)
        {
            // Random angle in radians
            float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;

            // Random distance
            float distance = Random.Range(minDistance, maxDistance);

            // Random Y height offset

            // Calculate spawn position
            Vector3 spawnPos = player.position;
            spawnPos += new Vector3(Mathf.Cos(angle) * distance, 0, Mathf.Sin(angle) * distance);

            // Spawn enemy
            GameObject enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
            spawnedEnemies.Add(enemy);

            // Gives refrence for EnemyHandler
            EnemyHandler enemyHandler = enemy.GetComponent<EnemyHandler>();
            if (enemyHandler != null)
            {
                enemyHandler.SetSpawner(this);
            }
        }
    }
    public void RemoveEnemy(GameObject enemy)
    {
        spawnedEnemies.Remove(enemy);
    }
}
