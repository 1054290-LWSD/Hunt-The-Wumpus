using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventHandler : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject player;  // Drag the player Transform here
    private int numberOfEnemies = 10;
    private float minSpawnDistance = 50f;
    private float maxSpawnDistance = 100f;
    public Text enemyCountText;
    public List<GameObject> allEnemies = new List<GameObject>();
    
    void Start()
    {
        SpawnEnemies(numberOfEnemies, minSpawnDistance, maxSpawnDistance);
    }
    public void runUpdateCycle()
    {
        if (enemyCountText != null)
        {
            enemyCountText.text = "Enemies Left: " + allEnemies.Count;
        }
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
            Vector3 spawnPos = player.transform.position;
            spawnPos += new Vector3(Mathf.Cos(angle) * distance, 0, Mathf.Sin(angle) * distance);

            // Spawn enemy
            GameObject enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
            allEnemies.Add(enemy);

            // Gives refrence for EnemyHandler
            EnemyHandler enemyHandler = enemy.GetComponent<EnemyHandler>();
            if (enemyHandler != null)
            {
                enemyHandler.SetSpawner(this);
            }
            
        }
        runUpdateCycle();
    }
    public void RemoveEnemy(GameObject enemy)
    {
        allEnemies.Remove(enemy);
    }
}
