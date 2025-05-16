using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClickHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler
{
    public enum TrianglePart { TopLeft, BottomLeft }
    public TrianglePart trianglePart;

    [Header("This Scripts")]
    public InventorySlot thisSlot;

    [Header("Other Scripts")]
    public InventorySlot otherSlot;

    [Header("Tooltip UI")]
    public RectTransform tooltipPanel;
    public Text tooltipText;

    private RectTransform rt;

    void Awake()
    {
        rt = GetComponent<RectTransform>();
        HideTooltip();
    }

    void Update()
    {
        if (tooltipPanel.gameObject.activeSelf)
        {
            Vector2 localPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                tooltipPanel.parent as RectTransform,
                Input.mousePosition,
                null,
                out localPos
            );
            tooltipPanel.anchoredPosition = localPos + new Vector2(64f, 8f);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Clicked");
        HideTooltip();

        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rt, eventData.position, eventData.pressEventCamera, out localPoint);
        Rect rect = rt.rect;
        localPoint += new Vector2(rect.width / 2, rect.height / 2);

        bool isTopLeft = localPoint.x + localPoint.y < rect.height;

        if ((trianglePart == TrianglePart.TopLeft && isTopLeft) || (trianglePart == TrianglePart.BottomLeft && !isTopLeft))
        {
            Debug.Log("This");
            if (thisSlot.myItem != null)
                thisSlot.myItem.Click(eventData);
            else
                thisSlot.Click(eventData);
        }
        else
        {
            Debug.Log("Other");
            if (otherSlot.myItem != null)
                otherSlot.myItem.Click(eventData);
            else
                otherSlot.Click(eventData);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        TryShowTooltip(eventData);
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        TryShowTooltip(eventData);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        HideTooltip();
    }

    void TryShowTooltip(PointerEventData eventData)
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rt, eventData.position, eventData.enterEventCamera, out localPoint);
        Rect rect = rt.rect;
        localPoint += new Vector2(rect.width / 2, rect.height / 2);

        bool isTopLeft = localPoint.x + localPoint.y < rect.height;
        bool shouldUseThis = (trianglePart == TrianglePart.TopLeft && isTopLeft) ||
                             (trianglePart == TrianglePart.BottomLeft && !isTopLeft);

        InventorySlot targetSlot = shouldUseThis ? thisSlot : otherSlot;

        if (targetSlot != null && targetSlot.myItem != null && targetSlot.myItem.myItem != null)
        {
            ShowTooltip(targetSlot.myItem.myItem.description);
        }
        else
        {
            HideTooltip();
        }
    }

    void ShowTooltip(string text)
    {
        tooltipText.text = text;
        tooltipPanel.gameObject.SetActive(true);
    }

    void HideTooltip()
    {
        tooltipPanel.gameObject.SetActive(false);
    }
}
