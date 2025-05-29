using UnityEngine;
using System;

public class BulletHandler : MonoBehaviour
{
    public float damage = 10f; // Set how much damage the bullet does
    public float damageMult = 1f; 
    private int maxBounces = 3;
    private int bounceCount = 0;
    public bool hasHit = false;
    public CakeHandler cakeHandler;

    void OnCollisionEnter(Collision collision)
    {
        // Try to get the EnemyHandler component
        EnemyHandler enemy = collision.gameObject.GetComponent<EnemyHandler>();

        if (enemy != null)
        {
            // If it has EnemyHandler, deal damage
            enemy.DealDamage(damage * damageMult);
            cakeHandler.runCakes(gameObject, CakeEventEnums.onBulletHit);
            hasHit = true;
            Destroy(gameObject, 0.1f); // Bullet disappears after hitting enemy
        }
        else
        {
            // Only bounce if the object is NOT tagged as "Bullet"
            if (!collision.gameObject.CompareTag("Bullet"))
            {
                bounceCount++;

                if (bounceCount >= maxBounces)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
