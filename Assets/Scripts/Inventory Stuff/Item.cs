using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Item")]
public class Item : ScriptableObject
{
    public Sprite sprite;
    public string methodName = "name";
    public string description = "Test";
    public GameData.Rarity rarity;
    public int price = -1;

    public List<CakeEventEnums> triggerEvents = new List<CakeEventEnums>();
}
