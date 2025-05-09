using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public AudioClip soundClip;

    private AudioSource audioSource;
    private int damage = 10;
    private int maxBounces = 3;
    private int bounceCount = 0;
    private void OnCollisionEnter(Collision other)
    {

        if (other.gameObject.CompareTag("Enemy"))
        {
            EnemyHandler enemy = other.gameObject.GetComponent<EnemyHandler>();
            audioSource = enemy.eventHandler.player.GetComponent<AudioSource>();
            audioSource.PlayOneShot(soundClip);
            Destroy(gameObject);
            enemy.DealDamage(damage);
        }
        else
        {
            bounceCount++;

            if (bounceCount >= maxBounces)
            {
                Destroy(gameObject);
            }
        }

    }
}
