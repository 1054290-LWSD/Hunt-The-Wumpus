using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClickHandler : MonoBehaviour, IPointerClickHandler
{
    public enum TrianglePart { TopLeft, BottomLeft }
    public TrianglePart trianglePart;

    [Header("This Scripts")]
    public InventorySlot thisSlot;

    [Header("Other Scripts")]
    public InventorySlot otherSlot;

    [Header("Tooltip UI")]
    public GameObject tooltipObject;
    public Text tooltipText;
    [Header("Inventory from \"CakeSlots\"")]
    public Inventory inventory;

    
    private Vector2 offset = new Vector2(175, 30f);

    RectTransform rt;

    void Awake()
    {
        rt = GetComponent<RectTransform>();
        if (tooltipObject != null)
        {
            tooltipObject.SetActive(false); // ensure tooltip is hidden at start
        }
    }

    void Update()
    {
        if (tooltipObject != null && inventory.tooltipTimer <= 0f)
        {
            if (tooltipObject.activeSelf)
            {
                tooltipObject.SetActive(false);
            }
        }
        if (RectTransformUtility.RectangleContainsScreenPoint(rt, Input.mousePosition))
        {
            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rt, Input.mousePosition, null, out localPoint);
            Rect rect = rt.rect;
            localPoint += new Vector2(rect.width / 2, rect.height / 2);
            bool isTopLeft = localPoint.x + localPoint.y < rect.height;

            InventorySlot target = ((trianglePart == TrianglePart.TopLeft && isTopLeft)
                                    || (trianglePart == TrianglePart.BottomLeft && !isTopLeft))
                                    ? thisSlot
                                    : otherSlot;

            if (tooltipObject != null && tooltipText != null && target.myItem != null)
            {
                inventory.tooltipTimer = 0.1f;
                tooltipObject.SetActive(true);
                tooltipText.text = target.myItem.myItem.description;

                Canvas canvas = tooltipObject.GetComponentInParent<Canvas>();
                CanvasScaler scaler = canvas.GetComponent<CanvasScaler>();

                // Scale the offset based on current screen resolution
                float scaleFactor = canvas.scaleFactor; // Automatically accounts for resolution
                Vector2 scaledOffset = offset * scaleFactor;
                // Convert mouse position to local point in the canvas
                RectTransform canvasRect = tooltipObject.transform.root.GetComponent<RectTransform>();
                Vector2 localPointDeep;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    canvasRect,
                    Input.mousePosition + (Vector3)scaledOffset,
                    null,
                    out localPointDeep
                );

                tooltipObject.GetComponent<RectTransform>().anchoredPosition = localPointDeep;

            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rt, eventData.position, eventData.pressEventCamera, out localPoint);

        Rect rect = rt.rect;
        localPoint += new Vector2(rect.width / 2, rect.height / 2);

        bool isTopLeft = localPoint.x + localPoint.y < rect.height;

        if ((trianglePart == TrianglePart.TopLeft && isTopLeft) || (trianglePart == TrianglePart.BottomLeft && !isTopLeft))
        {
            Debug.Log("This");
            if (thisSlot.myItem != null)
            {
                thisSlot.myItem.Click(eventData);
            }
            else
            {
                thisSlot.Click(eventData);
            }
        }
        else
        {
            Debug.Log("Other");
            if (otherSlot.myItem != null)
            {
                otherSlot.myItem.Click(eventData);
            }
            else
            {
                otherSlot.Click(eventData);
            }
        }
    }
}
