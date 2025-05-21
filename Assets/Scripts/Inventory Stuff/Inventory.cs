using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public static Inventory Singleton;
    public static InventoryItem carriedItem;
    public float tooltipTimer = 0f;
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
        tooltipTimer -= Time.deltaTime;
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
        List<Item> itemsHad = new List<Item>();
        foreach (InventorySlot i in inventorySlots)
        {
            if (i.myItem != null)
            {
                itemsHad.Add(i.myItem.myItem);
            }
        }
        HashSet<Item> itemsHadSet = new HashSet<Item>(itemsHad);
        List<Item> possibleItems = items.Where(item => !itemsHadSet.Contains(item)).ToList();
        if (possibleItems.Count == 0)
        {
            return items[0];
        }
        
        random = Random.Range(0, possibleItems.Count - 1);
        Debug.Log(string.Join(", ", possibleItems));
        Debug.Log("Random Num: " + random + "  Item: " + items[random]);
        return possibleItems[random];
    }
}
