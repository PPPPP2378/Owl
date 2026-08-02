//InventoryUI_n.cs
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class InventoryUI_n : MonoBehaviour
{
    public GameObject inventoryPanel;
    public TextMeshProUGUI[] itemTexts;

    public GameObject itemInfoPanel;
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI itemDescriptionText;
    private bool ignoreNextE = false;
    private List<ItemData_n> displayItems = new List<ItemData_n>();

    private bool isOpen = false;
    private bool isViewingInfo = false;
    private Statue_n currentStatue = null;
    public static InventoryUI_n Instance;
    public bool IsOpen => isOpen;

    void Awake()
    {
        Instance = this;
        Debug.Log("InventoryUI Awake : " + gameObject.scene.name + " / " + gameObject.name);

        Invoke(nameof(CheckState), 0.5f);
    }

    void CheckState()
    {
        Debug.Log(
            "activeSelf=" + gameObject.activeSelf +
            " activeInHierarchy=" + gameObject.activeInHierarchy +
            " enabled=" + enabled
        );
    }
    private int selectID = 0;




    void Start()
    {
        Debug.Log("Start実行");
        inventoryPanel.SetActive(false);
        if (itemInfoPanel != null)
        {
            itemInfoPanel.SetActive(false);
        }
        // 動作確認用（後で消す）
        /*InventoryManager_n.Instance.AddItem(
            "古い鍵",
            "錆びついた古い鍵。", ItemType.Key,
            null
        );*/
    }
    public void OpenForStatue(Statue_n statue)
    {
        if (statue.currentItem != null)
        {
            statue.currentItem.isPlaced = false;

            statue.currentItem = null;

            statue.SetWeapon(WeaponType_n.None);

            if (statue.puzzle != null)
            {
                statue.puzzle.CheckAnswer();
            }


            currentStatue = null;

            RefreshInventory();

            return;
        }

        // ===== 武器を置く =====
        currentStatue = statue;
        isOpen = true;
        inventoryPanel.SetActive(true);
        selectID = 0;

        UpdateInventory();

        ignoreNextE = true;
    }
    void Update()
    {
        Debug.Log("Update実行");
        Debug.Log("Update: isOpen = " + isOpen);
        // TABで開閉
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Debug.Log("Tab押された");

            if (isViewingInfo)
            {
                Debug.Log("詳細表示中");
                return;
            }

            isOpen = !isOpen;
            Debug.Log("isOpen = " + isOpen);

            inventoryPanel.SetActive(isOpen);
            Debug.Log("SetActive完了");

            if (isOpen)
            {
                selectID = 0;
                UpdateInventory();
                Debug.Log("UpdateInventory完了");
            }
        }

        if (!isOpen)
        {
            Debug.Log("isOpenがfalseなので終了");
            return;
        }

        // インベントリが開いている時だけEを受け付ける
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (ignoreNextE)
            {
                ignoreNextE = false;
                return;
            }

            UseSelectedItem();
        }

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
    }

    void UpdateInventory()
    {
        displayItems.Clear();

        for (int i = 0; i < itemTexts.Length; i++)
        {
            itemTexts[i].gameObject.SetActive(false);
        }

        foreach (ItemData_n item in InventoryManager_n.Instance.itemList)
        {
            if (!item.isPlaced)
            {
                displayItems.Add(item);
            }
        }

        if (selectID >= displayItems.Count)
        {
            selectID = Mathf.Max(0, displayItems.Count - 1);
        }

        for (int i = 0; i < displayItems.Count && i < itemTexts.Length; i++)
        {
            itemTexts[i].gameObject.SetActive(true);

            if (i == selectID)
                itemTexts[i].text = "> " + displayItems[i].itemName;
            else
                itemTexts[i].text = "  " + displayItems[i].itemName;
        }

        foreach (ItemData_n item in InventoryManager_n.Instance.itemList)
        {
            Debug.Log(item.itemName + " / isPlaced = " + item.isPlaced);
        }
    }

    public void RefreshInventory()
    {
        UpdateInventory();
    }

    void ShowItemInfo()
    {
        if (itemInfoPanel == null) return;

        List<ItemData_n> displayItems = new List<ItemData_n>();

        foreach (ItemData_n data in InventoryManager_n.Instance.itemList)
        {
            if (!data.isPlaced)
                displayItems.Add(data);
        }

        if (selectID < 0 || selectID >= displayItems.Count)
            return;

        ItemData_n item = displayItems[selectID];

        itemNameText.text = item.itemName;
        itemDescriptionText.text = item.description;

        itemInfoPanel.SetActive(true);
        isViewingInfo = true;
    }

    void UseSelectedItem()
    {
        Debug.Log("UseSelectedItem 呼び出し");
        Debug.Log("currentStatue = " + currentStatue);

        if (InventoryManager_n.Instance.itemList.Count == 0)
        {
            Debug.Log("アイテムがありません");
            return;
        }

        if (selectID < 0 ||
            selectID >= InventoryManager_n.Instance.itemList.Count)
        {
            Debug.Log("selectIDが範囲外");
            return;
        }

        if (selectID < 0 || selectID >= displayItems.Count)
        {
            Debug.Log("selectIDが範囲外");
            return;
        }

        ItemData_n item = displayItems[selectID];

        for (int i = 0; i < InventoryManager_n.Instance.itemList.Count; i++)
        {
            Debug.Log(i + " : "
                + InventoryManager_n.Instance.itemList[i].itemName
                + " / "
                + InventoryManager_n.Instance.itemList[i].weaponType);
        }

        Debug.Log("選択中：" + item.itemName);
        Debug.Log("武器：" + item.weaponType);

        Debug.Log("selectID = " + selectID);
        Debug.Log("item = " + item.itemName);
        Debug.Log("weaponType = " + item.weaponType);

        // ===== 像に武具を持たせる =====
        if (currentStatue != null)
        {
            Debug.Log("currentStatueあり");

            if (item.itemType != ItemType.Weapon)
            {
                Debug.Log("武器ではない");
                return;
            }

            Debug.Log("SetWeaponを呼ぶ直前");

            if (currentStatue.currentItem != null)
            {
                currentStatue.currentItem.isPlaced = false;
            }

            currentStatue.currentItem = item;
            item.isPlaced = true;
            currentStatue.SetWeapon(item.weaponType);

            if (currentStatue.puzzle != null &&
    currentStatue.puzzle.IsAllPlaced())
            {
                currentStatue.puzzle.CheckAnswer();
            }

            Debug.Log("SetWeaponを呼んだ直後");

            UpdateInventory();

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

        Debug.Log("itemList.Count = " + InventoryManager_n.Instance.itemList.Count);

        foreach (ItemData_n data in InventoryManager_n.Instance.itemList)
        {
            Debug.Log(data.itemName + " isPlaced=" + data.isPlaced);
        }
    }
}
