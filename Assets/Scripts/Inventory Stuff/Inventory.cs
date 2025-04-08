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

    public void Awake()
    {
        Singleton = this;
    }
    void Update()
    {
        if (carriedItem == null) return;

        carriedItem.transform.position = Input.mousePosition;
    }

    public void setCarriedItem(InventoryItem item)
    {
        if (carriedItem != null)
        {
            item.activeSlot.SetItem(carriedItem);
        }

        carriedItem = item;
        carriedItem.canvasGroup.blocksRaycasts = false;
        item.transform.SetParent(draggablesTransform);
    }
}
