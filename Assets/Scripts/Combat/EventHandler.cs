using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class EventHandler : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform player;
    public Text enemyCountText;
    public GameObject levelCompletePanel;

    private double numberOfEnemies = 5f;
    private double enemyHealth = 50f;
    private float spawnRadius = 150f;
    private double mostDamage = -1;
    public CakeHandler cakeHandler;
    public List<GameObject> spawnedEnemies = new List<GameObject>();
    public LevelManger levelManger;
    public PauseMenu pauseMenu;



    void Start()
    {
        numberOfEnemies = numberOfEnemies * Math.Pow(1.25f, GameData.levelsCompleted);
        enemyHealth = enemyHealth * Math.Pow(2.5f, GameData.levelsCompleted);
        SpawnEnemies();

    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            int x = 0;
            while (spawnedEnemies.Count > 0)
            {
                x++;
                Debug.Log(x);
                spawnedEnemies[0].GetComponent<EnemyHandler>().DealDamage(100000);//2147483647

            }

            //SpawnEnemies();
        }
    }

    public void SpawnEnemies()
    {
        spawnedEnemies.Clear();

        for (int i = 0; i < numberOfEnemies; i++)
        {
            float angle = UnityEngine.Random.Range(0f, 360f);
            float radius = UnityEngine.Random.Range(spawnRadius * 0.5f, spawnRadius);
            Vector3 spawnPos = player.position + new Vector3(Mathf.Cos(angle) * radius, 10, Mathf.Sin(angle) * radius);

            GameObject enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
            spawnedEnemies.Add(enemy);

            EnemyHandler handler = enemy.GetComponent<EnemyHandler>();
            if (handler != null)
            {
                handler.SetSpawner(this);
                handler.SetHealth(enemyHealth);
            }
            cakeHandler.runCakes(enemy, CakeEventEnums.onEnemySpawn);
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
    public void UpdateMostDamage(double dmg)
    {
        if (dmg > mostDamage)
        {
            mostDamage = dmg;
        }
    }
    public void UpdateEnemyCount()
    {
        if (enemyCountText != null)
        {
            if (spawnedEnemies.Count > 0)
            {
                enemyCountText.text = "Enemies Left: " + spawnedEnemies.Count;
            }
            else
            {
                //Level End
                enemyCountText.text = "";
                cakeHandler.inventory.UpdateSaveData();
                EndLevel();

            }
        }
    }
    public void EndLevel()
    {
        pauseMenu.canPause = false;
        pauseMenu.Pause();
        levelCompletePanel.SetActive(true);

        GameObject levelCompleteText = levelCompletePanel.transform.Find("LevelComplete")?.gameObject;
        GameObject moneyEarnedText = levelCompletePanel.transform.Find("MoneyEarned")?.gameObject;
        GameObject mostDamageText = levelCompletePanel.transform.Find("MostDamage")?.gameObject;
        //GameObject nextButton = levelCompletePanel.transform.Find("Next")?.gameObject;

        levelCompleteText.GetComponent<Text>().text = "Level " + (GameData.levelsCompleted + 1) + " Completed";
        moneyEarnedText.GetComponent<Text>().text = "$ " + 4 + " Earned";
        mostDamageText.GetComponent<Text>().text = "Most Damage\n" + mostDamage;

    }
    public void NextLevel()
    {
        GameData.money += 4;
        GameData.levelsCompleted++;
        if (GameData.mostDamage < mostDamage)
        {
            GameData.mostDamage = mostDamage
        }
        levelManger.changesScene("Shop");
    }
    
}
