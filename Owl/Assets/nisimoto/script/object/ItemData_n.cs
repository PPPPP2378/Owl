using UnityEngine;

public enum ItemType
{
    Key,
    Weapon,
    StoryMemo,
    HintMemo,
    Tool
}

public class ItemData_n
{
    public string itemName;
    public string description;
    public Sprite icon;


    public ItemType itemType;
    public WeaponType_n weaponType;

    public ItemData_n(string name,
        string desc,
        ItemType type,
        Sprite sprite = null,
        WeaponType_n weapon = WeaponType_n.None)
    {
        itemName = name;
        description = desc;
        itemType = type;
        icon = sprite;
        weaponType = weapon;
    }
}
