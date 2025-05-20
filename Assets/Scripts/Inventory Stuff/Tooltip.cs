using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Tooltip : MonoBehaviour
{
    public static Tooltip Singleton; // <-- singleton access
    [SerializeField] private GameObject tooltipObject;
    [SerializeField] private Text tooltipText;

    void Awake()
    {
        Singleton = this;
        HideTooltip();
    }

    public void ShowTooltip(string content, Vector3 position)
    {
        tooltipObject.SetActive(true);
        tooltipObject.transform.position = position;
        tooltipText.text = content;
    }

    public void HideTooltip()
    {
        tooltipObject.SetActive(false);
    }
}
