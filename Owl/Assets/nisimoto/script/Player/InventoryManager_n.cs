using System.Collections.Generic;
using UnityEngine;

public class InventoryManager_n : MonoBehaviour
{
    public static InventoryManager_n Instance;

    public List<ItemData_n> itemList = new List<ItemData_n>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddItem(
     string itemName,
     string description,
     ItemType type,
     Sprite icon = null,
     WeaponType_n weapon = WeaponType_n.None)
    {
        foreach (ItemData_n item in itemList)
        {
            if (item.itemName == itemName)
                return;
        }

        itemList.Add(
            new ItemData_n(
                itemName,
                description,
                type,
                icon,
                weapon
            )
        );

        Debug.Log(itemName + " ‚ð“üŽè");
    }

    public bool HasItem(string itemName)
    {
        foreach (ItemData_n item in itemList)
        {
            if (item.itemName == itemName)
                return true;
        }

        return false;
    }

    public void RemoveItem(string itemName)
    {
        itemList.RemoveAll(x => x.itemName == itemName);
    }
}
