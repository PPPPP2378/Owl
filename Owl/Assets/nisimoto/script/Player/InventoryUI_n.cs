using UnityEngine;
using TMPro;

public class InventoryUI_n : MonoBehaviour
{
    public GameObject inventoryPanel;
    public TextMeshProUGUI[] itemTexts;

    public GameObject itemInfoPanel;
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI itemDescriptionText;

    private bool isOpen = false;
    private bool isViewingInfo = false;
    private Statue_n currentStatue = null;

    private int selectID = 0;




    void Start()
    {
        inventoryPanel.SetActive(false);
        itemInfoPanel.SetActive(false);

        // 動作確認用（後で消す）
        InventoryManager_n.Instance.AddItem(
            "古い鍵",
            "錆びついた古い鍵。", ItemType.Key,
            null
        );
    }
    public void OpenForStatue(Statue_n statue)
    {
        currentStatue = statue;

        isOpen = true;
        inventoryPanel.SetActive(true);

        selectID = 0;

        UpdateInventory();
    }
    void Update()
    {
        // TABで開閉
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (isViewingInfo) return;

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

        // 詳細表示中
        if (isViewingInfo)
        {
            if (Input.GetKeyDown(KeyCode.Escape) ||
                Input.GetKeyDown(KeyCode.Q))
            {
                itemInfoPanel.SetActive(false);
                isViewingInfo = false;
            }

            return;
        }

        // カーソル移動
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

        // Q 詳細
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ShowItemInfo();
        }

        // E 使用
        if (Input.GetKeyDown(KeyCode.E))
        {
            UseSelectedItem();
        }
    }

    void UpdateInventory()
    {
        for (int i = 0; i < itemTexts.Length; i++)
        {
            itemTexts[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < InventoryManager_n.Instance.itemList.Count && i < itemTexts.Length; i++)
        {
            itemTexts[i].gameObject.SetActive(true);

            if (i == selectID)
                itemTexts[i].text = "> " + InventoryManager_n.Instance.itemList[i].itemName;
            else
                itemTexts[i].text = "  " + InventoryManager_n.Instance.itemList[i].itemName;
        }
    }

    void ShowItemInfo()
    {
        ItemData_n item = InventoryManager_n.Instance.itemList[selectID];

        itemNameText.text = item.itemName;
        itemDescriptionText.text = item.description;

        itemInfoPanel.SetActive(true);
        isViewingInfo = true;
    }

    void UseSelectedItem()
    {
        ItemData_n item = InventoryManager_n.Instance.itemList[selectID];

        // ===== 像に武具を持たせる =====
        if (currentStatue != null)
        {
            // 武器以外は持たせられない
            if (item.itemType != ItemType.Weapon)
            {
                Debug.Log("これは武具ではありません。");
                return;
            }

            // 武器を像に渡す
            currentStatue.SetWeapon(item.weaponType);

            Debug.Log(item.itemName + "を像に持たせた");

            // インベントリを閉じる
            currentStatue = null;
            isOpen = false;
            inventoryPanel.SetActive(false);

            return;
        }

        // ===== 通常使用 =====
        switch (item.itemName)
        {
            case "古い鍵":
                break;

            case "盾":
                break;

            case "使用人のメモ①":
                break;
        }
    }
}
