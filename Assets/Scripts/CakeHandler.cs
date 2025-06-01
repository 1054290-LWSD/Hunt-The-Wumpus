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
            int index = i;
            InventorySlot invenSlots = inventory.GetInventorySlots()[i];
            if (invenSlots.myItem != null)
            {
                while (invenSlots.myItem.myItem.methodName == "WeddingCake" && index != 0) //Does cake before this if wedding cake
                {
                    if (index > 0)
                    {
                        index--;
                        invenSlots = inventory.GetInventorySlots()[index];
                    }
                    else
                    {
                        invenSlots = inventory.GetInventorySlots()[0];
                    }


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
        double health = eventHandler.player.gameObject.GetComponent<Movement>().health;
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
            bulletHandler.damage += 300.0;
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

    private void BlueberryCake(GameObject gameObject)
    {
        BulletHandler bulletHandler = gameObject.gameObject.GetComponent<BulletHandler>();
        Debug.Log("Enemies Killed " + ((int)((bulletHandler.gunHandler.eventHandler.maxEnemies) - bulletHandler.gunHandler.eventHandler.spawnedEnemies.Count)));
        bulletHandler.damage += 10.0 * ((int)((bulletHandler.gunHandler.eventHandler.maxEnemies) - bulletHandler.gunHandler.eventHandler.spawnedEnemies.Count));
        // ^ does calculation for how many enemies killed
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
        bulletHandler.damage += 20.0;
    }
    private void ChocolateLavaCake(GameObject gameObject)
    {
        BulletHandler bulletHandler = gameObject.gameObject.GetComponent<BulletHandler>();
        double health = bulletHandler.gunHandler.eventHandler.player.gameObject.GetComponent<Movement>().health;
        double maxHealth = bulletHandler.gunHandler.eventHandler.player.gameObject.GetComponent<Movement>().GetMaxHealth();
        if (health == maxHealth)
        {
            bulletHandler.damage += 100.0;
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
            if (enemies.Count == 1)
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

            StartCoroutine(CupcakeCourtine(targets, gunHandler));
        }

    }
    private IEnumerator CupcakeCourtine(List<GameObject> targets, GunHandler gunHandler)
    {
        for (int i = 0; i < targets.Count; i++)
        {
            yield return new WaitForSeconds(0.1f * (i + 1));

            GameObject newBullet = gunHandler.Shoot(true);
            Rigidbody rb = newBullet.GetComponent<Rigidbody>();
            Vector3 direction = (targets[i].transform.position - newBullet.transform.position).normalized;
            rb.velocity = direction * rb.velocity.magnitude;
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
    private void LemonCake(GameObject gameObject)
    {
        BulletHandler bulletHandler = gameObject.gameObject.GetComponent<BulletHandler>();
        Transform bulletTrans = gameObject.gameObject.GetComponent<Transform>();
        bulletTrans.localScale += Vector3.one * ((int)bulletHandler.gunHandler.eventHandler.spawnedEnemies.Count);
        // ^ how many emenies left
    }
    private void MangoCake(GameObject gameObject)
    {
        BulletHandler bulletHandler = gameObject.gameObject.GetComponent<BulletHandler>();
        int numOtherCakes = 0;
        foreach (InventorySlot i in inventory.GetInventorySlots())
        {
            if (i.myItem != null && i.myItem.myItem.methodName != "MangoCake")
                numOtherCakes++;
        }
        Debug.Log(numOtherCakes);
        bulletHandler.damageMult *= 5.0 - (0.5 * (double)numOtherCakes);
    }
    private void MatchaCake(GameObject gameObject)
    {
        BulletHandler bulletHandler = gameObject.gameObject.GetComponent<BulletHandler>();
        bulletHandler.damageMult *= 1.0 + 0.1 * ((int)(bulletHandler.gunHandler.eventHandler.maxEnemies) - bulletHandler.gunHandler.eventHandler.spawnedEnemies.Count);
        // ^ does calculation for how many enemies killed
    }
    private void MoonCake(GameObject gameObject)
    {
        EnemyHandler enemyHandler = gameObject.gameObject.GetComponent<EnemyHandler>();
        enemyHandler.moveSpeed *= 0.5f;
    }
    private void NullCake(GameObject gameObject)
    {
        BulletHandler bulletHandler = gameObject.gameObject.GetComponent<BulletHandler>();
        bulletHandler.damageMult *= (double)UnityEngine.Random.Range(1.0f, 3.0f);
    }
    private void Pancake(GameObject gameObject)
    {
        BulletHandler bulletHandler = gameObject.gameObject.GetComponent<BulletHandler>();
        bulletHandler.damageMult *= 1.0 + 0.1 * (bulletHandler.gunHandler.eventHandler.spawnedEnemies.Count);
        // ^ does calculation for how many enemies left
    }
    private void PoundCake(GameObject gameObject)
    {

        EnemyHandler enemyHandler = gameObject.gameObject.GetComponent<EnemyHandler>();
        enemyHandler.moveSpeed *= 0.75f;
    }
    private void RaspberryCake(GameObject gameObject)
    {
        BulletHandler bulletHandler = gameObject.gameObject.GetComponent<BulletHandler>();
        double health = bulletHandler.gunHandler.eventHandler.player.gameObject.GetComponent<Movement>().health;
        bulletHandler.damageMult += 1.0 + 0.1 * ((health / 10.0) - (health / 10.0) % 1);
    }
    private void RedVelvetCake(GameObject gameObject)
    {
        BulletHandler bulletHandler = gameObject.gameObject.GetComponent<BulletHandler>();
        bulletHandler.damageMult += 5.0;
    }
    private void StrawberryCake(GameObject gameObject)
    {
        BulletHandler bulletHandler = gameObject.gameObject.GetComponent<BulletHandler>();
        bulletHandler.damageMult += 1.0 * ((int)(bulletHandler.gunHandler.eventHandler.maxEnemies) - bulletHandler.gunHandler.eventHandler.spawnedEnemies.Count);
    }   // ^ does calculation for how many enemies killed
    private void TCIAL(GameObject gameObject)
    {
        EnemyHandler enemyHandler = gameObject.gameObject.GetComponent<EnemyHandler>();
        EventHandler eventHandler = enemyHandler.GetEventHandler();
        if (eventHandler.spawnedEnemies.Count == 0) return;
        List<GameObject> candidates = new List<GameObject>(eventHandler.spawnedEnemies);
        candidates.Remove(gameObject);

        GameObject randomEnemy = candidates[UnityEngine.Random.Range(0, candidates.Count)];
        randomEnemy.transform.position = gameObject.transform.position;
    }
    private void TresLechesCake(GameObject gameObject)
    {
        BulletHandler bulletHandler = gameObject.gameObject.GetComponent<BulletHandler>();
        bulletHandler.damageMult *= 3.0;
    }
    private void UbeCake(GameObject gameObject)
    {
        BulletHandler bulletHandler = gameObject.gameObject.GetComponent<BulletHandler>();
        foreach (InventorySlot i in inventory.GetInventorySlots())
        {
            if (i.myItem != null)
            {
                bulletHandler.damageMult += 3.0;
            }
        }

    }
    private void WeddingCake(GameObject gameObject)
    {
        //Implemented in other places
    }
    private void ApplePie(GameObject gameObject)
    {
        Movement playerMovement = gameObject.GetComponent<Movement>();
        for (int i = 0; i < (playerMovement.eventHandler.GetBaseNumEnemies() * Math.Pow(1.2f, GameData.levelsCompleted)); i++)
        {
            playerMovement.SetHealth(playerMovement.health * 1.5);
        }
        playerMovement.UpdateHealthText();
    }
    private void KeyLimePie(GameObject gameObject)
    {
        BulletHandler bulletHandler = gameObject.gameObject.GetComponent<BulletHandler>();
        if (GameData.money > 0)
            bulletHandler.damageMult *= 1 + 1.0 * GameData.money;
    }
}
