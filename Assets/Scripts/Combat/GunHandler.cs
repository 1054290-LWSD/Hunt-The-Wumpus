using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunHandler : MonoBehaviour
{
    public GameObject projectilePrefab;  // Assign in Inspector
    private float projectileSpeed = 50f;  // Adjustable speed
    private float coolDownTimer = 0;

    void Update()
    {
        coolDownTimer -= Time.deltaTime;
        if (Input.GetMouseButton(0) && coolDownTimer <= 0) // 0 = Left Click
        {
            Shoot();
            coolDownTimer = 0.4f;
        }
    }

    void Shoot()
    {
        // Create projectile at current position + a little and rotation
        Vector3 spawnPos = transform.position + transform.forward * 1f; // 1 unit in front
        GameObject projectile = Instantiate(projectilePrefab, spawnPos, transform.rotation);

        // Get the Rigidbody component
        Rigidbody rb = projectile.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.velocity = transform.forward * projectileSpeed;
        }
    }
}