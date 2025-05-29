using UnityEngine;
using UnityEngine.UI;

public class EnemyHandler : MonoBehaviour
{
    private static int maxHealth = 50;
    private int health = maxHealth;
    
    private EventHandler eventHandler;
    private Transform player;
    private Camera mainCamera;
    public float moveSpeed = 10f;
    private float damage = 10f;
    public Text healthText;
    public GameObject attackPrefab;
    private float attackTimer = 0f;
    private float attackInterval = 2.5f;
    private float initialOffset;

    public void SetSpawner(EventHandler eHandler)
    {
        eventHandler = eHandler;
        player = eHandler.player;
    }

    void Start()
    {
        mainCamera = Camera.main;

        if (healthText == null)
        {
            Transform textTransform = transform.Find("Canvas/HealthText");
            if (textTransform != null)
            {
                healthText = textTransform.GetComponent<Text>();
            }
        }
        initialOffset = Random.Range(0f, 5f);
        attackTimer = -initialOffset;

        UpdateHealthText();
    }

    void Update()
    {
        // Billboard effect
        if (healthText != null && mainCamera != null)
        {
            healthText.transform.rotation = Quaternion.LookRotation(
                healthText.transform.position - mainCamera.transform.position
            );
        }

        // Move toward player with swarm offset
        if (player != null)
        {
            Vector3 offset = new Vector3(
                Mathf.Sin(Time.time * 1.5f + GetInstanceID()) * 1.5f,
                0,
                Mathf.Cos(Time.time * 1.5f + GetInstanceID()) * 1.5f
            );

            Vector3 targetPos = player.position + offset;
            Vector3 direction = targetPos - transform.position;
            direction.y = 0;

            //sets enemy position to be at correct Y level if bug occurs
            if (transform.position.y < -100)
            {
                transform.position = new Vector3(transform.position.x, 10, transform.position.z);
            }
            float distance = direction.magnitude;
            float stopDistance = 2f;

            if (direction != Vector3.zero)
            {
                Quaternion lookRot = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, Time.deltaTime * 5f);
            }

            if (distance > stopDistance)
            {
                transform.position += direction.normalized * moveSpeed * Time.deltaTime;
            }
            // Timed Attack
            attackTimer += Time.deltaTime;
            if (attackTimer >= attackInterval)
            {
                attackTimer -= attackInterval;
                TriggerAttack();
            }
        }
    }
    void TriggerAttack()
    {
        if (attackPrefab != null)
        {
            GameObject attack = Instantiate(attackPrefab, transform.position, Quaternion.identity);
            attack.GetComponent<EnemyAttack>().SetDamage(damage);
            Destroy(attack, 0.25f); // Auto-destroy after 0.1s
        }
    }
    public void DealDamage(float damage)
    {
        Debug.Log("Damage: " + damage);
        health -= (int)damage;
        UpdateHealthText();

        if (health <= 0)
        {
            if (eventHandler != null)
                eventHandler.RemoveEnemy(gameObject);

            Destroy(gameObject);
        }
    }

    void UpdateHealthText()
    {
        if (healthText != null)
        {
            healthText.text = health.ToString() + "/" + maxHealth;
        }
    }
}
