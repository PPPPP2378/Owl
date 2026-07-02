using UnityEngine;

public class Item_n : MonoBehaviour
{
    public string itemName;
    public bool showDescriptionOnPickup = true;
    public string description;
    public Sprite itemIcon;
    public ItemType itemType;
    public WeaponType_n weaponType;
    public Sprite icon;

    public void GetItem()
    {
        InventoryManager_n.Instance.AddItem(
           itemName,
           description,
           itemType,
           icon,
           weaponType
       );

        Destroy(gameObject);
    }
}
