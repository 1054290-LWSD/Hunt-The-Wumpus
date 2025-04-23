using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class InventoryItem : MonoBehaviour//, IPointerClickHandler
{
    Image itemIcon;
    public CanvasGroup canvasGroup { get; private set; }

    public Item myItem { get; set;}
    public InventorySlot activeSlot { get; set; }
    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        itemIcon = GetComponent<Image>();
    }
    public void Click(PointerEventData eventData)
    {

        if (eventData.button == PointerEventData.InputButton.Left)
        {
            //InventoryItem temp = Inventory.carriedItem;
            if (Inventory.carriedItem == null)
            {
                Inventory.Singleton.setCarriedItem(this);
                activeSlot.SetItem(null, true);
            }
            else
            {
                InventoryItem temp = activeSlot.myItem;
                activeSlot.SetItem(Inventory.carriedItem, true);
                Inventory.Singleton.setCarriedItem(temp);
            }
            //if (temp != null)
            //    activeSlot.SetItem(temp, false);
            //else Debug.Log("No Item In Cursor");
        }
    }
    public void Initialize(Item item, InventorySlot parent)
    {
        activeSlot = parent;
        activeSlot.myItem = this;
        myItem = item;
        itemIcon.sprite = item.sprite;
    }
}
