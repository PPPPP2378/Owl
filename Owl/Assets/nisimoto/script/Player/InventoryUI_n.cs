using UnityEngine;
using TMPro;

public class InventoryUI_n : MonoBehaviour
{
    public GameObject inventoryPanel;
    public TextMeshProUGUI[] itemTexts;

    private bool isOpen = false;
    private int selectID = 0;

    void Start()
    {
        inventoryPanel.SetActive(false);
        InventoryManager_n.Instance.AddItem("ŒÃ‚¢Œ®");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isOpen = !isOpen;

            inventoryPanel.SetActive(isOpen);

            if (isOpen)
            {
                selectID = 0;
                UpdateInventory();
            }
        }

        if (!isOpen)
            return;

        if (InventoryManager_n.Instance.itemList.Count == 0)
            return;

        if (Input.GetKeyDown(KeyCode.W))
        {
            selectID--;

            if (selectID < 0)
                selectID = InventoryManager_n.Instance.itemList.Count - 1;

            UpdateInventory();
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            selectID++;

            if (selectID >= InventoryManager_n.Instance.itemList.Count)
                selectID = 0;

            UpdateInventory();
        }
    }

    void UpdateInventory()
    {
        // ‘S•””ñ•\Ž¦
        for (int i = 0; i < itemTexts.Length; i++)
        {
            itemTexts[i].gameObject.SetActive(false);
        }

        // Ž‚¿•¨•\Ž¦
        for (int i = 0; i < InventoryManager_n.Instance.itemList.Count && i < itemTexts.Length; i++)
        {
            itemTexts[i].gameObject.SetActive(true);

            if (i == selectID)
                itemTexts[i].text = "> " + InventoryManager_n.Instance.itemList[i];
            else
                itemTexts[i].text = "  " + InventoryManager_n.Instance.itemList[i];
        }
    }
}
