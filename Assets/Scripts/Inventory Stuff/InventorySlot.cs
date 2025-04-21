using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class InventorySlot : MonoBehaviour//, IPointerClickHandler
{
    public InventoryItem myItem { get; set; }
    public void Click(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (Inventory.carriedItem == null) return;
            SetItem(Inventory.carriedItem);
        }
    }
    public void SetItem(InventoryItem item)
    {
        Inventory.carriedItem = null;
        item.activeSlot.myItem = null;
        myItem = item;
        myItem.activeSlot = this;

        myItem.transform.SetParent(transform); // resets local transform
        //myItem.transform.localPosition = Vector3.zero;
        myItem.transform.localRotation = Quaternion.identity;
        myItem.transform.localScale = Vector3.one; // force a clean scale

        myItem.canvasGroup.blocksRaycasts = true;
    }
}
