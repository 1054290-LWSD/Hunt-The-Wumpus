using UnityEngine;

public class BulletHandler : MonoBehaviour
{
    public int damage = 25; // Set how much damage the bullet does
    public int maxBounces = 3;
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
            // If it wasn't an enemy, just bounce
            bounceCount++;

            if (bounceCount >= maxBounces)
            {
                Destroy(gameObject);
            }
        }
    }
}
