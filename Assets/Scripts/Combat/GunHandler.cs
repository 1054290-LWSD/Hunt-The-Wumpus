using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunHandler : MonoBehaviour
{
    public Text damageText;
    public Text damageMultText;
    public GameObject projectilePrefab;  // Assign in Inspector
    private float projectileSpeed = 50f;  // Adjustable speed
    public float coolDownTimer = 0;
    public bool isPaused = false;
    public CakeHandler cakeHandler;
    public EventHandler eventHandler;

    void Update()
    {
        coolDownTimer -= Time.deltaTime;
        if (Input.GetMouseButton(0) && coolDownTimer <= 0 && !isPaused) // 0 = Left Click
        {
            coolDownTimer = 0.4f;
            Shoot(false);
            
        }
    }

    public GameObject Shoot(bool extra) //Is something spawned by cakes
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
        projectile.GetComponent<BulletHandler>().gunHandler = this;
        projectile.GetComponent<BulletHandler>().cakeHandler = cakeHandler;
        projectile.GetComponent<BulletHandler>().isExtra = extra;
        cakeHandler.runCakes(projectile, CakeEventEnums.onBulletSpawn);
        return projectile;
    }
}