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
    public bool isStore = false;
    public Inventory otherInventory;
    [SerializeField] InventorySlot[] inventorySlots;

    [SerializeField] Transform draggablesTransform;
    [SerializeField] InventoryItem itemPrefab;

    [Header("Item List")]
    [SerializeField] Item[] items;

    [Header("Debug")]
    [SerializeField] Button giveItemButton;
    [SerializeField] Button rerollButton;


    public void Awake()
    {
        if (isStore)
        {
            items = otherInventory.items;
        }
        Singleton = this;
        giveItemButton.onClick.AddListener(delegate { SpawnInventoryItem(); });
        if (isStore)
        {
            rerollButton.onClick.AddListener(delegate { Reroll(); });
            Reroll();
        }

    }
    void Update()
    {
        tooltipTimer -= Time.deltaTime;
        if (carriedItem == null) return;

        carriedItem.transform.position = Input.mousePosition;
    }

    public void setCarriedItem(InventoryItem item)
    {

        carriedItem = item;
        if (item != null)
        {
            carriedItem.canvasGroup.blocksRaycasts = false;
            item.transform.SetParent(draggablesTransform);
        }
    }
    public void SpawnInventoryItem(Item item = null)
    {
        if (CheckIfFull())
        {
            return;
        }
        Item _item = item;
        if (_item == null)
        {
            _item = PickRandomItem();
        }

        for (int i = 0; i < inventorySlots.Length; i++)
        {
            //Check if empty
            if (inventorySlots[i].myItem == null)
            {
                Instantiate(itemPrefab, inventorySlots[i].transform).Initialize(_item, inventorySlots[i]);
                if (isStore)
                {
                    inventorySlots[i].UpdateText();
                }
                break;
            }
        }
    }

    Item PickRandomItem()
    {
        int random = 0;
        List<Item> itemsHad = new List<Item>();
        foreach (InventorySlot i in inventorySlots)
        {
            if (i.myItem != null)
            {
                itemsHad.Add(i.myItem.myItem);
            }
        }
        if (isStore)
        {
            foreach (InventorySlot i in otherInventory.inventorySlots)
            {
                if (i.myItem != null)
                {
                    itemsHad.Add(i.myItem.myItem);
                }
            }

        }
        HashSet<Item> itemsHadSet = new HashSet<Item>(itemsHad);
        List<Item> possibleItems = items.Where(item => !itemsHadSet.Contains(item)).ToList();
        if (possibleItems.Count == 0)
        {
            return items[0];
        }

        random = Random.Range(0, possibleItems.Count - 1);
        // Debug.Log(string.Join(", ", possibleItems));
        // Debug.Log("Random Num: " + random + "  Item: " + items[random]);
        return possibleItems[random];
    }
    public void Reroll()
    {
        foreach (InventorySlot i in inventorySlots)
        {
            if (i.myItem != null)
            {
                Destroy(i.myItem.gameObject);
            }
            i.SetItem(null);
        }
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            SpawnInventoryItem(PickRandomItem());
        }
    }
    public bool CheckIfFull()
    {
        foreach (InventorySlot invSlot in inventorySlots)
        {
            if (invSlot.myItem == null)
            {
                return false;
            }
        }
        return true;
    }
    public InventorySlot[] GetInventorySlots()
    {
        return inventorySlots;
    }
}
