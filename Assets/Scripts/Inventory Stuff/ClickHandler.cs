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

    [Header("Tooltip UI")]                           // ← NEW
    public GameObject tooltipObject;
    public Text tooltipText;

    public float tooltipTimer = 0f;
    private Vector2 offset = new Vector2(160f, 15f);

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
        tooltipTimer -= Time.deltaTime;
        if (tooltipObject != null && tooltipTimer >= 0f)
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
                tooltipTimer = 2f;
                tooltipObject.SetActive(true);
                tooltipText.text = target.myItem.myItem.description;
    
                // Convert mouse position to local point in the canvas
                RectTransform canvasRect = tooltipObject.transform.root.GetComponent<RectTransform>();
                Vector2 localPointDeep;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    canvasRect,
                    Input.mousePosition + (Vector3)offset,
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
