//ItemData_n.cs
using UnityEngine;

public enum ItemType
{
    Key,
    Weapon,
    StoryMemo,
    Weight,
    HintMemo,
    Tool
}

public class ItemData_n
{
    public string itemName;
    public string description;
    public Sprite icon;
    public bool isPlaced = false;
    public int weightValue = 0;

    public ItemType itemType;
    public WeaponType_n weaponType;

    public ItemData_n(
     string name,
     string desc,
     ItemType type,
     Sprite sprite = null,
     WeaponType_n weapon = WeaponType_n.None,
     int weight = 0)
    {
        itemName = name;
        description = desc;
        itemType = type;
        icon = sprite;
        weaponType = weapon;
        weightValue = weight;
    }
}
