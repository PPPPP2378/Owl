using System.Collections.Generic;
using UnityEngine;

public class InventoryManager_n : MonoBehaviour
{
    public static InventoryManager_n Instance;

    public List<string> itemList = new List<string>();

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

    public void AddItem(string itemName)
    {
        if (!itemList.Contains(itemName))
        {
            itemList.Add(itemName);
            Debug.Log(itemName + " ‚ð“üŽè");
        }
    }

    public bool HasItem(string itemName)
    {
        return itemList.Contains(itemName);
    }

    public void RemoveItem(string itemName)
    {
        itemList.Remove(itemName);
    }
}
