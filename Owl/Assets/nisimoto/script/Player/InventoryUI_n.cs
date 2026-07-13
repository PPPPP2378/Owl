using UnityEngine;
using TMPro;

public class InventoryUI_n : MonoBehaviour
{
    public GameObject inventoryPanel;
    public TextMeshProUGUI[] itemTexts;

    public GameObject itemInfoPanel;
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI itemDescriptionText;
    private bool ignoreNextE = false;

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
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (ignoreNextE)
            {
                ignoreNextE = false;
                return;
            }

            UseSelectedItem();
        }
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
        if (itemInfoPanel == null) return;

        ItemData_n item = InventoryManager_n.Instance.itemList[selectID];

        itemNameText.text = item.itemName;
        itemDescriptionText.text = item.description;

        itemInfoPanel.SetActive(true);
        isViewingInfo = true;
    }

    void UseSelectedItem()
    {
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

        ItemData_n item = InventoryManager_n.Instance.itemList[selectID];

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

            currentStatue.SetWeapon(item.weaponType);

            Debug.Log("SetWeaponを呼んだ直後");

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
