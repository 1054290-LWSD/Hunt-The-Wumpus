using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Item")]
public class Item : ScriptableObject
{
    public Sprite sprite;
    public string description = "Test";
    public int price = -1;

    public List<CakeEventEnums> triggerEvents = new List<CakeEventEnums>();
}
