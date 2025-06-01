using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class CakeHandler : MonoBehaviour
{
    public Inventory inventory;
    public bool HasBasque()
    {
        foreach (InventorySlot i in inventory.GetInventorySlots())
        {
            if (i.myItem != null && i.myItem.myItem.methodName == "BasqueCheeseCake")
                return true;
        }
        return false;
    }
    public void runCakes(GameObject obj, CakeEventEnums cakeEvent)
    {
        //foreach (InventorySlot invenSlots in inventory.GetInventorySlots())
        for (int i = 0; i < inventory.GetInventorySlots().Length; i++)
        {

            InventorySlot invenSlots = inventory.GetInventorySlots()[i];
            if (invenSlots.myItem != null)
            {
                if (invenSlots.myItem.myItem.methodName == "WeddingCake") //Does cake before this if wedding cake
                {


                    invenSlots = inventory.GetInventorySlots()[i > 0 ? i - 1 : 0];
                }
            }
            if (invenSlots.myItem != null)
            {
                foreach (CakeEventEnums cakeTrigger in invenSlots.myItem.myItem.triggerEvents)
                {
                    if (cakeTrigger == cakeEvent)
                    {
                        string methodName = invenSlots.myItem.myItem.methodName;

                        // Use reflection to find a method with that name
                        var method = GetType().GetMethod(methodName, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);

                        if (method != null)
                        {
                            method.Invoke(this, new object[] { obj });
                        }
                        else
                        {
                            Debug.LogWarning("Method not found: " + methodName);
                        }
                    }
                    else
                    {
                        Debug.Log(invenSlots.myItem.myItem + " is " + cakeTrigger + ". Should be: " + cakeEvent);
                    }
                }
            }
        }
    }
    private void AppleCake(GameObject gameObject)
    {
        Movement playerMovement = gameObject.GetComponent<Movement>();
        playerMovement.SetHealth(playerMovement.health * 2);
        playerMovement.UpdateHealthText();
        
    }
    private void BananaCake(GameObject gameObject)
    {
        BulletHandler bulletHandler = gameObject.gameObject.GetComponent<BulletHandler>();
        bulletHandler.gunHandler.coolDownTimer = bulletHandler.gunHandler.coolDownTimer / 2f;
        Debug.Log(bulletHandler.gunHandler.coolDownTimer);
    }
    private void BAPoundCake(GameObject gameObject)
    {
        EnemyHandler enemyHandler = gameObject.gameObject.GetComponent<EnemyHandler>();
        enemyHandler.moveSpeed *= 0.25f;
    }
    private void BASpongeCake(GameObject gameObject)
    {
        EventHandler eventHandler = gameObject.gameObject.GetComponent<EventHandler>();
        float health = eventHandler.player.gameObject.GetComponent<Movement>().health;
        eventHandler.moneyGained += (int)(health / 20);
    }
    private void BasqueCheeseCake(GameObject gameObject)
    {
        //Implemented in other places
    }
    private void BeefCake(GameObject gameObject)
    {
        if (gameObject.GetComponent<BulletHandler>() != null)
        {
            BulletHandler bulletHandler = gameObject.gameObject.GetComponent<BulletHandler>();
            bulletHandler.damage += 300;
        }
        else if (gameObject.GetComponent<Movement>() != null)
        {
            Movement playerMovement = gameObject.GetComponent<Movement>();
            playerMovement.SetHealth(playerMovement.health * 0.5f);
            playerMovement.UpdateHealthText();
        }
    }
    private void BirthdayCake(GameObject gameObject)
    {
        Transform bulletTrans = gameObject.gameObject.GetComponent<Transform>();
        if (gameObject.GetComponent<BulletHandler>().hasHit == false)
        {
            bulletTrans.localScale *= 5f;
        }
    }
    private void ButtCake(GameObject gameObject)
    {
        Transform bulletTrans = gameObject.gameObject.GetComponent<Transform>();
        bulletTrans.localScale *= 3f;
    }
    private void CarrotCake()
    {
        Rigidbody bulletRB = gameObject.gameObject.GetComponent<Rigidbody>();
        bulletRB.velocity *= 2f;
    }
    private void CheeseCake(GameObject gameObject)
    {
        EventHandler eventHandler = gameObject.gameObject.GetComponent<EventHandler>();
        foreach (InventorySlot i in inventory.GetInventorySlots())
        {
            if (i.myItem != null)
            {
                eventHandler.moneyGained += 1;
            }
        }
    }
    private void CherryCake(GameObject gameObject)
    {
        Transform bulletTrans = gameObject.gameObject.GetComponent<Transform>();
        bulletTrans.localScale += Vector3.one * 3f;
    }
    private void ChocolateCake(GameObject gameObject)
    {
        BulletHandler bulletHandler = gameObject.gameObject.GetComponent<BulletHandler>();
        bulletHandler.damage += 20;
    }
    private void ChocolateLavaCake(GameObject gameObject)
    {
        BulletHandler bulletHandler = gameObject.gameObject.GetComponent<BulletHandler>();
        float health = bulletHandler.gunHandler.eventHandler.player.gameObject.GetComponent<Movement>().health;
        float maxHealth = bulletHandler.gunHandler.eventHandler.player.gameObject.GetComponent<Movement>().GetMaxHealth();
        if (health == maxHealth)
        {
            bulletHandler.damage += 100f;
        }
    }
    private void CoconutCake(GameObject gameObject)
    {
        EnemyHandler enemyHandler = gameObject.gameObject.GetComponent<EnemyHandler>();
        enemyHandler.SetHealth(enemyHandler.GetHealth() * 0.70);
    }
    private void CoffeeCake(GameObject gameObject)
    {
        Rigidbody bulletRB = gameObject.gameObject.GetComponent<Rigidbody>();
        bulletRB.velocity *= 3f;
    }
    private void Cupcake(GameObject gameObject)
    {
        BulletHandler originalHandler = gameObject.GetComponent<BulletHandler>();
        if (!originalHandler.isExtra)
        {
            GunHandler gunHandler = originalHandler.gunHandler;
            List<GameObject> enemies = gunHandler.eventHandler.spawnedEnemies;
            if (enemies.Count == 0) return;
            List<GameObject> targets = new List<GameObject>();
            if (enemies.Count == 1) //shoots both at the last enemy
            {
                targets.Add(enemies[0]);
                targets.Add(enemies[0]);
            }
            else
            {
                while (targets.Count < 2)
                {
                    GameObject enemy = enemies[UnityEngine.Random.Range(0, enemies.Count)];
                    targets.Add(enemy);
                }
            }
            foreach (GameObject enemy in targets) //shoots twice, could be more
            {
                GameObject newBullet = gunHandler.Shoot(true);

                Rigidbody rb = newBullet.GetComponent<Rigidbody>();
                Vector3 direction = (enemy.transform.position - newBullet.transform.position).normalized;
                rb.velocity = direction * rb.velocity.magnitude;
            }
        }
    }
    private void GingerBreadCake(GameObject gameObject)
    {
        EventHandler eventHandler = gameObject.gameObject.GetComponent<EventHandler>();
        eventHandler.moneyGained += 3;

    }
    private void IcecreamCake(GameObject gameObject)
    {
        StartCoroutine(IcecreamCoroutine(gameObject));
    }
    private IEnumerator IcecreamCoroutine(GameObject gameObject)
    {
        EnemyHandler enemyHandler = gameObject.gameObject.GetComponent<EnemyHandler>();
        float ogValue = enemyHandler.moveSpeed;
        enemyHandler.moveSpeed = 0f;
        yield return new WaitForSeconds(2f);
        enemyHandler.moveSpeed = ogValue;
    }
    private void MoonCake(GameObject gameObject)
    {
        EnemyHandler enemyHandler = gameObject.gameObject.GetComponent<EnemyHandler>();
        enemyHandler.moveSpeed *= 0.5f;
    }
    private void NullCake(GameObject gameObject)
    {
        BulletHandler bulletHandler = gameObject.gameObject.GetComponent<BulletHandler>();
        bulletHandler.damageMult *= UnityEngine.Random.Range(1.0f, 3.0f); ;
    }
    private void PoundCake(GameObject gameObject)
    {

        EnemyHandler enemyHandler = gameObject.gameObject.GetComponent<EnemyHandler>();
        enemyHandler.moveSpeed *= 0.75f;
    }
    private void RaspberryCake(GameObject gameObject)
    {
        BulletHandler bulletHandler = gameObject.gameObject.GetComponent<BulletHandler>();
        float health = bulletHandler.gunHandler.eventHandler.player.gameObject.GetComponent<Movement>().health;
        bulletHandler.damageMult *= 1f + 0.1f * (int)(health / 10f);
    }
    private void RedVelvetCake(GameObject gameObject)
    {
        BulletHandler bulletHandler = gameObject.gameObject.GetComponent<BulletHandler>();
        bulletHandler.damageMult += 5f;
    }
    private void StrawberryCake(GameObject gameObject)
    {
        BulletHandler bulletHandler = gameObject.gameObject.GetComponent<BulletHandler>();
        bulletHandler.damageMult += 1f * ((int)(bulletHandler.gunHandler.eventHandler.maxEnemies) - bulletHandler.gunHandler.eventHandler.spawnedEnemies.Count);
    }   // ^ does calculation for how many enemies killed
    private void TresLechesCake(GameObject gameObject)
    {
        BulletHandler bulletHandler = gameObject.gameObject.GetComponent<BulletHandler>();
        bulletHandler.damageMult *= 3f;
    }
    private void UbeCake(GameObject gameObject)
    {
        BulletHandler bulletHandler = gameObject.gameObject.GetComponent<BulletHandler>();
        foreach (InventorySlot i in inventory.GetInventorySlots())
        {
            if (i.myItem != null)
            {
                bulletHandler.damageMult += 3f;
            }
        }
        
    }
    private void WeddingCake(GameObject gameObject)
    {
        //Implemented in other places
    }
    
}
