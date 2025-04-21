using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickHandler : MonoBehaviour, IPointerClickHandler
{
    public enum TrianglePart { TopLeft, BottomLeft }
    public TrianglePart trianglePart;

    [Header("This Scripts")]
    public InventorySlot thisSlot;

    [Header("Other Scripts")]
    public InventorySlot otherSlot;


    RectTransform rt;

    void Awake()
    {
        rt = GetComponent<RectTransform>();
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
