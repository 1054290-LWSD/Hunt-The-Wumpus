using System;
using UnityEngine;
using TMPro;
public class DevInput : MonoBehaviour
{
    public Inventory inventory;
    public TMP_InputField inputField;
    void Start()
    {
        if (inputField != null)
        {
            inputField.onEndEdit.AddListener(ParseCommand);
        }
        if (!GameData.isDev)
        {
            gameObject.SetActive(false);
        }
    }
    public Item FindCake(string str)
    {
        foreach (Item i in inventory.GetItems())
        {
            if (str.ToLower() == i.methodName.ToLower())
            {
                return i;
            }
        }

        //Add word "Cake"
        foreach (Item i in inventory.GetItems())
        {
            if ((str + "cake").ToLower() == i.methodName.ToLower())
            {
                return i;
            }
        }

        //Add word "Pie"
        foreach (Item i in inventory.GetItems())
        {
            if ((str + "pie").ToLower() == i.methodName.ToLower())
            {
                return i;
            }
        }
        return null;
    }
    public void GiveCake(string str)
    {
        if (FindCake(str) != null)
            inventory.SpawnInventoryItem(FindCake(str));
    }
    public void GiveMoney(string str)
    {
        if (int.TryParse(str, out int result))
        {
            GameData.money += (result);
            inventory.UpdateText();
        }
        else
        {
            Debug.Log("Invalid number input.");
        }
    }
    public void ParseCommand(string str)
    {
        if (str.IndexOf(" ") != -1)
            str.Replace(" ", "");
        if (str.Length >= 5)
        {
            if (str.Substring(0, 4).ToLower() == "cake" || str.Substring(0, 4).ToLower() == "give")
            {
                GiveCake(str.Substring(4));
                return;
            }
            if (str.Substring(0, 5).ToLower() == "money")
            {
                GiveMoney(str.Substring(5));

                return;
            }
        }
        if (str.Substring(0, 1) == "$")
        {
            GiveMoney(str.Substring(1));
            return;
        }
        GiveMoney(str);
        GiveCake(str);
    }
}
