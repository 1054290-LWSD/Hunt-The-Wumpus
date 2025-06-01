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
    public CakeHandler cakeHandler;
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
            if (deleteButton != null)
                deleteButton.SetActive(true);
        }
        else
        {
            if (deleteButton != null)
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
        // Gather items already owned
        List<Item> itemsHad = new List<Item>();
        foreach (InventorySlot i in inventorySlots)
        {
            if (i.myItem != null)
            {
                itemsHad.Add(i.myItem.myItem);
            }
        }

        if (isStore && otherInventory != null)
        {
            foreach (InventorySlot i in otherInventory.inventorySlots)
            {
                if (i.myItem != null)
                {
                    itemsHad.Add(i.myItem.myItem);
                }
            }
        }

        // Get remaining items to choose from
        HashSet<Item> itemsHadSet = new HashSet<Item>(itemsHad);
        List<Item> possibleItems = items.Where(item => !itemsHadSet.Contains(item)).ToList();
        if (cakeHandler.HasBasque())
        {
            possibleItems = items.ToList();
        }
        
        if (possibleItems.Count == 0)
        {
            return items[0]; // fallback to item with index 0
        }

        // Separate by rarity
        var commons = possibleItems.Where(i => i.rarity == GameData.Rarity.common).ToList();
        var uncommons = possibleItems.Where(i => i.rarity == GameData.Rarity.uncommon).ToList();
        var rares = possibleItems.Where(i => i.rarity == GameData.Rarity.rare).ToList();

        float roll = UnityEngine.Random.Range(0f, 1f); // 0.0 to 1.0

        if (roll < 0.70f && commons.Count > 0)
        {
            Debug.Log("Common");
            return commons[UnityEngine.Random.Range(0, commons.Count)];
        }
        else if (roll < 0.95f && uncommons.Count > 0)
        {
            Debug.Log("Uncommon");
            return uncommons[UnityEngine.Random.Range(0, uncommons.Count)];
        }
        else if (rares.Count > 0)
        {
            Debug.Log("Rare");
            return rares[UnityEngine.Random.Range(0, rares.Count)];
        }

        // Fallbacks if chosen rarity group is empty
        if (commons.Count > 0) return commons[UnityEngine.Random.Range(0, commons.Count)];
        if (uncommons.Count > 0) return uncommons[UnityEngine.Random.Range(0, uncommons.Count)];
        if (rares.Count > 0) return rares[UnityEngine.Random.Range(0, rares.Count)];

        return items[0]; // fallback
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
