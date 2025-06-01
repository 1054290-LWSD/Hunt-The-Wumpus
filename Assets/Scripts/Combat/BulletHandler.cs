using UnityEngine;
using System;

public class BulletHandler : MonoBehaviour
{
    public double damage = 10f; // Set how much damage the bullet does
    public double damageMult = 1f;
    private int maxBounces = 3;
    private int bounceCount = 0;
    public bool hasHit = false;
    public CakeHandler cakeHandler;
    public GunHandler gunHandler;
    public bool isExtra;
    void OnCollisionEnter(Collision collision)
    {
        // Try to get the EnemyHandler component
        EnemyHandler enemy = collision.gameObject.GetComponent<EnemyHandler>();

        if (enemy != null)
        {
            // If it has EnemyHandler, deal damage
            if (Double.IsInfinity(damage))
                damage = Double.MaxValue;
            if (Double.IsInfinity(damageMult))
                damageMult = Double.MaxValue;
            gunHandler.damageText.text = gunHandler.eventHandler.SmartFormat(damage);
            gunHandler.damageMultText.text = gunHandler.eventHandler.SmartFormat(damageMult);
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
