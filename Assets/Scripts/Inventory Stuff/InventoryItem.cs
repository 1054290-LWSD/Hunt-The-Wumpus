using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class InventoryItem : MonoBehaviour, IPointerClickHandler
{
    Image itemIcon;
    public CanvasGroup canvasGroup { get; private set; }

    public Item myItem {get;}
    public InventorySlot activeSlot { get; set; }
    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        itemIcon = GetComponent<Image>();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
    if (eventData.button == PointerEventData.InputButton.Left)
        {
            Inventory.Singleton.setCarriedItem(this);
        }
    }
}
