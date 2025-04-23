using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class InventorySlot : MonoBehaviour//, IPointerClickHandler
{
    public InventoryItem myItem { get; set; }
    public void Click(PointerEventData eventData)
    {
        //if (myItem == null) Debug.Log("InventorySlot: Doesn't Have Item");
        //else Debug.Log("InventorySlot: Does Have Item?");
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (Inventory.carriedItem == null)
            {
                Debug.Log("Return");
                return;
            }

            SetItem(Inventory.carriedItem, true);
            Inventory.Singleton.setCarriedItem(null);
        }
    }
    public void SetItem(InventoryItem item, bool shouldDelete)
    {
        //Inventory.carriedItem = myItem;
        //if (shouldDelete)
        //{
            // if (item == null)
            // {
            //     myItem = null;
            // }
        //}
        if (item == null) myItem = null;
        else
        {
            //item.activeSlot.myItem = item;
            myItem = item;
            myItem.activeSlot = this;


            myItem.transform.SetParent(transform); // resets local transform
                                                   //myItem.transform.localPosition = Vector3.zero;
            myItem.transform.localRotation = Quaternion.identity;
            myItem.transform.localScale = Vector3.one; // force a clean scale

            myItem.canvasGroup.blocksRaycasts = true;
        }
    }
}
