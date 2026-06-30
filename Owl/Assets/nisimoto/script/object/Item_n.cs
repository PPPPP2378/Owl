using UnityEngine;

public class Item_n : MonoBehaviour
{
    public string itemName;

    public void GetItem()
    {
        InventoryManager_n.Instance.AddItem(itemName);

        Destroy(gameObject);
    }
}
