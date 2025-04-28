using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunHandler : MonoBehaviour
{
    public GameObject projectilePrefab;  // Assign in Inspector
    public float projectileSpeed = 20f;  // Adjustable speed of projectile

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        Vector3 spawnPos = transform.position + transform.forward * 2f; // 1 unit in front
        GameObject projectile = Instantiate(projectilePrefab, spawnPos, transform.rotation);
        // Create projectile at current position and rotation

        // Get the Rigidbody component
        Rigidbody rb = projectile.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.velocity = transform.forward * projectileSpeed;
        }
    }
}
