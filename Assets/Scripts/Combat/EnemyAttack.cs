using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    private float damage;
    public void SetDamage(float dmg)
    {
        damage = dmg;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Movement player = other.GetComponent<Movement>();
            if (player != null)
            {
                player.TakeDamage((int)damage);
            }
        }
    }
}
