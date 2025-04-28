using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHandler : MonoBehaviour
{
    private int health = 50;
    private EventHandler eventHandler; // deals with all Enemies

    private float moveSpeed = 12f; // Speed Enemy moves
    private float stopDistance = 3f; // How far away the Enemy will be before it stops moving
    public void SetSpawner(EventHandler eHandler)
    {
        eventHandler = eHandler; // Called when the enemy is spawned
    }
    void Update()
    {
        Vector3 direction = eventHandler.player.position - transform.position;

        // Ignore vertical difference for rotation
        direction.y = 0;

        float distanceToPlayer = direction.magnitude;

        // Only rotate if there's some distance
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }

        // Check if too close

        if (distanceToPlayer > stopDistance)
        {
            // Move only if far enough
            direction = direction.normalized;
            transform.position += direction * moveSpeed * Time.deltaTime;
        }
    }
    public void DealDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Destroy(gameObject);
            if (eventHandler != null)
            {
                eventHandler.RemoveEnemy(gameObject);
            }

        }
    }
}
