using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public AudioClip buySound;
    public AudioClip errorSound;
    public AudioClip clickSound;
    public AudioClip reorollSound;
    public AudioSource audioSource;
    public GameObject deleteButton;
    public static Inventory Singleton;
    public static InventoryItem carriedItem;
    public float tooltipTimer = 0f;
    public bool isStore = false;
    public bool shouldUnspawn = false;
    public Inventory otherInventory;
    public Text moneyText;
    public Text rerollText;
    [SerializeField] InventorySlot[] inventorySlots;

    [SerializeField] Transform draggablesTransform;
    [SerializeField] InventoryItem itemPrefab;

    [Header("Item List")]
    [SerializeField] Item[] items;


    public int rerollCost = 5;


    void Start()
    {
        Singleton = this;
        if (!isStore)
        {
            if (otherInventory != null)
            {
                otherInventory.items = items;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            foreach (InventorySlot i in inventorySlots)
            {
                if (i.myItem != null)
                {
                    Destroy(i.myItem.gameObject);
                }
                i.SetItem(null);
            }
            foreach (Item i in GameData.cakes)
            {
                //Debug.Log("Spawn: " + i);
                SpawnInventoryItem(i);
            }
            if (otherInventory != null)
            {
                //Debug.Log("Reroll");
                otherInventory.Reroll(true);
            }
        }
        if (shouldUnspawn)
        {
            gameObject.SetActive(false);
        }
        UpdateText();
    }
    void Update()
    {
        tooltipTimer -= Time.unscaledDeltaTime;
        if (carriedItem == null) return;

        carriedItem.transform.position = Input.mousePosition;
    }

    public void setCarriedItem(InventoryItem item)
    {
        PlaySound(clickSound);
        carriedItem = item;
        if (item != null)
        {
            carriedItem.canvasGroup.blocksRaycasts = false;
            item.transform.SetParent(draggablesTransform);
            item.transform.SetParent(draggablesTransform, worldPositionStays: true);
            
            deleteButton.SetActive(true);
            Debug.Log(isStore);
        }
        else
        {
            deleteButton.SetActive(false);
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

        random = UnityEngine.Random.Range(0, possibleItems.Count - 1);
        // Debug.Log(string.Join(", ", possibleItems));
        // Debug.Log("Random Num: " + random + "  Item: " + items[random]);
        return possibleItems[random];
    }
    public void Reroll(bool isFree)
    {
        if (GameData.money >= rerollCost || isFree)
        {
            if (!isFree)
            {
                GameData.money -= rerollCost;
                rerollCost++;
            }
            UpdateText();
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
                SpawnInventoryItem();
            }
            PlaySound(reorollSound);
        }
        else
        {
            PlaySound(errorSound);
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
    public void deleteItem()
    {
        Destroy(carriedItem.gameObject);
        setCarriedItem(null);
    }
    public void UpdateSaveData()
    {
        if (!isStore)
        {
            GameData.cakes.Clear();
            foreach (InventorySlot invenSlot in inventorySlots)
            {
                if (invenSlot.myItem != null)
                {
                    GameData.cakes.Add(invenSlot.myItem.myItem); // Updates inventory save data (ex: cake slots)
                }
            }
        }
    }
    public void UpdateText()
    {
        if (moneyText != null)
        {
            moneyText.text = "$" + GameData.money;
        }
        if (rerollText != null)
        {
            rerollText.text = "Reroll\n$" + rerollCost;
        }
    }
    public void PlaySound(AudioClip aC)
    {
        if (audioSource != null && aC != null)
        {
            audioSource.PlayOneShot(aC); // Play without interrupting other sounds
        }
    }
}
