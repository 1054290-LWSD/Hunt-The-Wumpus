using UnityEngine;

public class BulletHandler : MonoBehaviour
{
    private int damage = 10; // Set how much damage the bullet does
    private int maxBounces = 3;
    private int bounceCount = 0;

    void OnCollisionEnter(Collision collision)
    {
        // Try to get the EnemyHandler component
        EnemyHandler enemy = collision.gameObject.GetComponent<EnemyHandler>();

        if (enemy != null)
        {
            // If it has EnemyHandler, deal damage
            enemy.DealDamage(damage);
            Destroy(gameObject); // Bullet disappears after hitting enemy
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
