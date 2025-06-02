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
    public GameObject deathPanel;
    public AudioSource audioSource;
    public AudioClip hitSound;

    public int moneyGained = 4;

    private double numberOfEnemies = 5f;
    public double maxEnemies;
    private double enemyHealth = 50f;
    private float spawnRadius = 150f;
    private double mostDamage = -1;
    public CakeHandler cakeHandler;
    public List<GameObject> spawnedEnemies = new List<GameObject>();
    public LevelManger levelManger;
    public PauseMenu pauseMenu;



    void Start()
    {
        maxEnemies = (int) (numberOfEnemies * Math.Pow(1.1f, GameData.levelsCompleted));
        enemyHealth = enemyHealth * Math.Pow(1.4f, GameData.levelsCompleted);
        if (GameData.levelsCompleted >= 8)
        {
            enemyHealth *= Math.Pow(1.1f, GameData.levelsCompleted - 8);
        }

        SpawnEnemies();

    }
    void Update()
    {
        if (GameData.isDev && Input.GetKeyDown(KeyCode.R))
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

        for (int i = 0; i < maxEnemies; i++)
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
    public void TriggerDie()
    {
        if (GameData.mostDamage < mostDamage)
        {
            GameData.mostDamage = mostDamage;
        }
        pauseMenu.canPause = false;
        pauseMenu.Pause();
        deathPanel.SetActive(true);

        GameObject levelsCompletedText = deathPanel.transform.Find("LevelsCompleted")?.gameObject;
        GameObject mostDamageText = deathPanel.transform.Find("MostDamage")?.gameObject;

        levelsCompletedText.GetComponent<Text>().text = "Levels Completed:\n " + GameData.levelsCompleted;
        if (Double.IsInfinity(GameData.mostDamage))
            GameData.mostDamage = Double.MaxValue;
        mostDamageText.GetComponent<Text>().text = "Most Damage\n" + SmartFormat(GameData.mostDamage);
        GameData.levelsCompleted = 0;
        GameData.mostDamage = -1;
        GameData.money = 4;
        GameData.cakes.Clear();
    }
    public void EndLevel()
    {
        cakeHandler.runCakes(gameObject, CakeEventEnums.onLevelComplete);
        pauseMenu.canPause = false;
        pauseMenu.Pause();
        pauseMenu.CloseCakeMenu();
        pauseMenu.ClosePauseMenu();
        levelCompletePanel.SetActive(true);

        GameObject levelCompleteText = levelCompletePanel.transform.Find("LevelComplete")?.gameObject;
        GameObject moneyEarnedText = levelCompletePanel.transform.Find("MoneyEarned")?.gameObject;
        GameObject mostDamageText = levelCompletePanel.transform.Find("MostDamage")?.gameObject;
        //GameObject nextButton = levelCompletePanel.transform.Find("Next")?.gameObject;

        levelCompleteText.GetComponent<Text>().text = "Level " + (GameData.levelsCompleted + 1) + " Completed";
        moneyEarnedText.GetComponent<Text>().text = "$ " + moneyGained + " Earned\n$ " + (GameData.money >= 25 ? 5 : (int)(GameData.money / 5)) + " Interest\n";
        if (Double.IsInfinity(mostDamage))
            mostDamage = Double.MaxValue;
        mostDamageText.GetComponent<Text>().text = "Most Damage\n" + SmartFormat(mostDamage);

    }
    public void NextLevel()
    {
        GameData.money += moneyGained + (GameData.money >= 25 ? 5 : (int)(GameData.money / 5));
        GameData.levelsCompleted++;
        if (GameData.mostDamage < mostDamage)
        {
            GameData.mostDamage = mostDamage;
        }
        levelManger.changesScene("Shop");
    }
    public void PlaySound(AudioClip aC)
    {
        if (audioSource != null && aC != null)
        {
            audioSource.PlayOneShot(aC); // Play without interrupting other sounds
        }
    }
    public string SmartFormat(double value = -1)
    {
        if (value == -1)
        {
            return "Infinity";
        }
        if (value >= 1e9 || value <= -1e9 || (value != 0 && Math.Abs(value) < 1e-4))
        {
            // Use scientific notation and clean exponent
            string raw = value.ToString("E3").Replace("+", "");
            int eIndex = raw.IndexOf('E');
            string basePart = raw.Substring(0, eIndex);
            string expPart = raw.Substring(eIndex + 1).TrimStart('0');

            if (expPart.StartsWith("-"))
            {
                expPart = "-" + expPart.Substring(1).TrimStart('0');
            }

            // Handle "E" with no exponent digits (e.g., E0)
            if (expPart == "" || expPart == "-")
                expPart = "0";

            return basePart + "E" + expPart;
        }
        else if (value == Math.Floor(value))
        {
            return value.ToString("0");
        }
        else
        {
            return value.ToString("0.###");
        }
    }
    public double GetBaseNumEnemies()
    {
        return numberOfEnemies;
    }
}
