using UnityEngine;

public class ItemData_n
{
    public string itemName;
    public string description;
    public Sprite icon;

    public ItemData_n(string name, string desc, Sprite sprite = null)
    {
        itemName = name;
        description = desc;
        icon = sprite;
    }
}
