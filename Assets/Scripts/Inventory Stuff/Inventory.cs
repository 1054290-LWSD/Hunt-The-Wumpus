using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public static Inventory Singleton;
    public static InventoryItem carriedItem;

    [SerializeField] InventorySlot[] inventorySlots;

    [SerializeField] Transform draggablesTransform;
    [SerializeField] InventoryItem itemPrefab;

    [Header("Item List")]
    [SerializeField] Item[] items;

    [Header("Debug")]
    [SerializeField] Button giveItemButton;

    public void Awake()
    {
        Singleton = this;
        giveItemButton.onClick.AddListener(delegate { SpawnInventoryItem(); });
    }
    void Update()
    {
        if (carriedItem == null) return;

        carriedItem.transform.position = Input.mousePosition;
    }

    public void setCarriedItem(InventoryItem item)
    {
        // if (carriedItem != null)
        // {
        //     item.activeSlot.SetItem(carriedItem, true);
        // }

        carriedItem = item;
        if (item != null)
        {
            carriedItem.canvasGroup.blocksRaycasts = false;
            item.transform.SetParent(draggablesTransform);
        }
    }
    public void SpawnInventoryItem(Item item = null)
    {
        bool y = false;
        foreach (InventorySlot invSlot in inventorySlots) {
            if (invSlot.myItem == null) y = true;
        }
        if (!y) return;
        Item _item = item;
        if(_item == null)
        { _item = PickRandomItem(); }

        for (int i = 0; i < inventorySlots.Length; i++)
        {
            //Check if empty
            if (inventorySlots[i].myItem == null)
            {
                Instantiate(itemPrefab, inventorySlots[i].transform).Initialize(_item, inventorySlots[i]);
                break;
            }
        }
    }

    Item PickRandomItem()
    {
        int random = 0;
        bool x = false;
        while (!x)
        {
            random = Random.Range(0, items.Length);
            bool y = false;
            foreach (InventorySlot invSlot in inventorySlots) {
                if (invSlot.myItem != null)
                {
                    if (items[random] != invSlot.myItem.myItem)
                    {
                        Debug.Log(items[random] + "   " + invSlot.myItem.myItem);
                    }
                    else
                    {
                        if (items[0]!= invSlot.myItem.myItem )
                        y = true;
                    }
                }
            }
            if (!y) x = true;
        }
        return items[random];
    }
}
