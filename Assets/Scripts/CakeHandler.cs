using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CakeHandler : MonoBehaviour
{
    public Inventory inventory;
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
                        //  Debug.Log(invenSlots.myItem.myItem + " " + cakeTrigger);
                    }
                }
            }
        }
    }
    public void AppleCake(GameObject gameObject)
    {
        Movement playerMovement = gameObject.gameObject.GetComponent<Movement>();
        playerMovement.health *= 2;
        playerMovement.UpdateHealthText();
    }

    public void BirthdayCake(GameObject gameObject)
    {
        Transform bulletTrans = gameObject.gameObject.GetComponent<Transform>();
        if (gameObject.GetComponent<BulletHandler>().hasHit == false)
        {
            bulletTrans.localScale *= 10f;
            Debug.Log(bulletTrans.localScale.x);
        }
    }
    public void ButtCake(GameObject gameObject)
    {
        Transform bulletTrans = gameObject.gameObject.GetComponent<Transform>();
        bulletTrans.localScale *= 3f;
    }
    public void CherryCake(GameObject gameObject)
    {
        Transform bulletTrans = gameObject.gameObject.GetComponent<Transform>();
        bulletTrans.localScale += Vector3.one * 5f;
    }
    public void ChocolateCake(GameObject gameObject)
    {
        BulletHandler bulletHandler = gameObject.gameObject.GetComponent<BulletHandler>();
        bulletHandler.damage += 20;
    }
    public void CoffeeCake(GameObject gameObject)
    {
        Rigidbody bulletRB = gameObject.gameObject.GetComponent<Rigidbody>();
        bulletRB.velocity *= 3f;
    }
    public void MoonCake(GameObject gameObject)
    {
        EnemyHandler enemyHandler = gameObject.gameObject.GetComponent<EnemyHandler>();
        enemyHandler.moveSpeed *= 0.5f;
    }
    public void NullCake(GameObject gameObject)
    {
        BulletHandler bulletHandler = gameObject.gameObject.GetComponent<BulletHandler>();
        bulletHandler.damageMult *= UnityEngine.Random.Range(1.0f, 3.0f); ;
    }
    public void RedVelvet(GameObject gameObject)
    {
        BulletHandler bulletHandler = gameObject.gameObject.GetComponent<BulletHandler>();
        bulletHandler.damageMult += 5f;
    }
    public void TresLechesCake(GameObject gameObject)
    {
        BulletHandler bulletHandler = gameObject.gameObject.GetComponent<BulletHandler>();
        bulletHandler.damageMult *= 3f;
    }
    public void WeddingCake(GameObject gameObject)
    {
        //if (inventory.inventorySlots[])
    }
    
}
