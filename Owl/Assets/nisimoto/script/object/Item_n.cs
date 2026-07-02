using UnityEngine;

public class Item_n : MonoBehaviour
{
    public string itemName;
    public bool showDescriptionOnPickup = true;
    public string description;
    public Sprite itemIcon;

    public void GetItem()
    {
        InventoryManager_n.Instance.AddItem(
     itemName,
     description,
     itemIcon
        );

        if (showDescriptionOnPickup)
        {
           ItemInfoUI_n.Instance.Show(itemName, description);
        }

        Destroy(gameObject);
    }
}
