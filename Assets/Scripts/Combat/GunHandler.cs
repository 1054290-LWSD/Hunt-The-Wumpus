using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunHandler : MonoBehaviour
{
    public GameObject projectilePrefab;  // Assign in Inspector
    public float projectileSpeed = 20f;  // Adjustable speed

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 0 = Left Click
        {
            Shoot();
        }
    }

    void Shoot()
    {
        // Create projectile at current position and rotation
        GameObject projectile = Instantiate(projectilePrefab, transform.position, transform.rotation);

        // Get the Rigidbody component
        Rigidbody rb = projectile.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.velocity = transform.forward * projectileSpeed;
        }
    }
}