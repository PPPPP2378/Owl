// InventoryUI_n.cs
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryUI_n : MonoBehaviour
{
    public static InventoryUI_n Instance;

    [Header("インベントリ")]
    public GameObject inventoryPanel;
    public TextMeshProUGUI[] itemTexts;

    [Header("アイテム詳細")]
    public GameObject itemInfoPanel;
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI itemDescriptionText;

    public int LastInventoryActionFrame { get; private set; } = -1;

    private readonly List<ItemData_n> displayItems =
        new List<ItemData_n>();

    private bool isOpen;
    private bool isViewingInfo;

    private int selectID;
    private int openedFrame = -1;

    private Statue_n currentStatue;

    public bool IsOpen => isOpen;

    private void Awake()
    {
        Instance = this;
        Debug.Log("新しいInventoryUIコードが動いています");
    }

    private void Start()
    {
        isOpen = false;
        isViewingInfo = false;

        if (inventoryPanel != null)
        {
            inventoryPanel.SetActive(false);
        }

        if (itemInfoPanel != null)
        {
            itemInfoPanel.SetActive(false);
        }
    }

    private void Update()
    {
        if (Keyboard.current == null)
        {
            return;
        }

        HandleTabInput();

        if (!isOpen)
        {
            return;
        }

        // 詳細画面を開いている間
        if (isViewingInfo)
        {
            HandleItemInfoInput();
            return;
        }

        UpdateDisplayItems();

        if (displayItems.Count == 0)
        {
            return;
        }

        HandleCursorInput();
        HandleDecisionInput();
        HandleInfoInput();
    }

    // ========================================
    // 通常のインベントリ開閉
    // ========================================

    private void HandleTabInput()
    {
        if (!Keyboard.current.tabKey.wasPressedThisFrame)
        {
            return;
        }

        if (isViewingInfo)
        {
            return;
        }

        if (isOpen)
        {
            CloseInventory();
        }
        else
        {
            OpenNormalInventory();
        }
    }

    private void OpenNormalInventory()
    {
        // 通常のTab表示なので、石像選択状態を解除
        currentStatue = null;

        isOpen = true;
        selectID = 0;

        inventoryPanel.SetActive(true);
        UpdateInventory();
    }

    private void CloseInventory()
    {
        isOpen = false;
        isViewingInfo = false;
        currentStatue = null;

        inventoryPanel.SetActive(false);

        if (itemInfoPanel != null)
        {
            itemInfoPanel.SetActive(false);
        }
    }

    // ========================================
    // 石像からインベントリを開く
    // ========================================

    public void OpenForStatue(Statue_n statue)
    {
        if (statue == null)
        {
            Debug.LogError("OpenForStatueにnullが渡されました");
            return;
        }

        // すでに武器を置いている場合は取り外す
        if (statue.currentItem != null)
        {
            statue.currentItem.isPlaced = false;
            statue.currentItem = null;

            statue.SetWeapon(WeaponType_n.None);

            if (statue.puzzle != null)
            {
                statue.puzzle.CheckAnswer();
            }
        }

        currentStatue = statue;
        isOpen = true;
        isViewingInfo = false;
        selectID = 0;

        inventoryPanel.SetActive(true);

        if (itemInfoPanel != null)
        {
            itemInfoPanel.SetActive(false);
        }

        UpdateInventory();

        // 石像を調べたE入力が、
        // アイテム決定にも使われるのを防ぐ
        openedFrame = Time.frameCount;
    }

    // ========================================
    // 入力
    // ========================================

    private void HandleCursorInput()
    {
        if (Keyboard.current.wKey.wasPressedThisFrame)
        {
            selectID--;

            if (selectID < 0)
            {
                selectID = displayItems.Count - 1;
            }

            UpdateInventory();
        }
        else if (Keyboard.current.sKey.wasPressedThisFrame)
        {
            selectID++;

            if (selectID >= displayItems.Count)
            {
                selectID = 0;
            }

            UpdateInventory();
        }
    }

    private void HandleDecisionInput()
    {
        if (!Keyboard.current.eKey.wasPressedThisFrame)
        {
            return;
        }

        // インベントリを開いたフレームのEは無視
        if (Time.frameCount == openedFrame)
        {
            return;
        }

        UseSelectedItem();
    }

    private void HandleInfoInput()
    {
        if (Keyboard.current.qKey.wasPressedThisFrame)
        {
            ShowItemInfo();
        }
    }

    private void HandleItemInfoInput()
    {
        bool closePressed =
            Keyboard.current.escapeKey.wasPressedThisFrame ||
            Keyboard.current.qKey.wasPressedThisFrame ||
            Keyboard.current.eKey.wasPressedThisFrame;

        if (!closePressed)
        {
            return;
        }

        itemInfoPanel.SetActive(false);
        isViewingInfo = false;
    }

    // ========================================
    // インベントリ表示
    // ========================================

    private void UpdateDisplayItems()
    {
        displayItems.Clear();

        if (InventoryManager_n.Instance == null)
        {
            return;
        }

        foreach (ItemData_n item in InventoryManager_n.Instance.itemList)
        {
            // 設置済みアイテムは表示しない
            if (item.isPlaced)
            {
                continue;
            }

            // 石像を調べているときは武器だけ表示
            if (currentStatue != null &&
                item.itemType != ItemType.Weapon)
            {
                continue;
            }

            displayItems.Add(item);
        }

        if (displayItems.Count == 0)
        {
            selectID = 0;
        }
        else
        {
            selectID = Mathf.Clamp(
                selectID,
                0,
                displayItems.Count - 1
            );
        }
    }

    private void UpdateInventory()
    {
        UpdateDisplayItems();

        foreach (TextMeshProUGUI itemText in itemTexts)
        {
            itemText.text = "";
            itemText.gameObject.SetActive(false);
        }

        int displayCount =
            Mathf.Min(displayItems.Count, itemTexts.Length);

        for (int i = 0; i < displayCount; i++)
        {
            itemTexts[i].gameObject.SetActive(true);

            string cursor = i == selectID ? "> " : "  ";

            itemTexts[i].text =
                cursor + displayItems[i].itemName;
        }
    }

    public void RefreshInventory()
    {
        if (isOpen)
        {
            UpdateInventory();
        }
    }

    // ========================================
    // 詳細表示
    // ========================================

    private void ShowItemInfo()
    {
        if (itemInfoPanel == null ||
            displayItems.Count == 0)
        {
            return;
        }

        if (selectID < 0 ||
            selectID >= displayItems.Count)
        {
            return;
        }

        ItemData_n item = displayItems[selectID];

        itemNameText.text = item.itemName;
        itemDescriptionText.text = item.description;

        itemInfoPanel.SetActive(true);
        isViewingInfo = true;
    }

    // ========================================
    // アイテム決定
    // ========================================

    private void UseSelectedItem()
    {
        UpdateDisplayItems();

        if (displayItems.Count == 0)
        {
            return;
        }

        if (selectID < 0 ||
            selectID >= displayItems.Count)
        {
            return;
        }

        ItemData_n item = displayItems[selectID];

        // 石像へ武器を置く
        if (currentStatue != null)
        {
            if (item.itemType != ItemType.Weapon)
            {
                Debug.Log("武器アイテムを選んでください");
                return;
            }

            currentStatue.currentItem = item;
            item.isPlaced = true;

            currentStatue.SetWeapon(item.weaponType);

            if (currentStatue.puzzle != null &&
                currentStatue.puzzle.IsAllPlaced())
            {
                currentStatue.puzzle.CheckAnswer();
            }

            currentStatue = null;
            isOpen = false;
            inventoryPanel.SetActive(false);

            // 武器設置に使ったEを、プレイヤーの調べる処理に再利用させない
            LastInventoryActionFrame = Time.frameCount;

            return;
        }

        // 通常のアイテム使用処理
        switch (item.itemName)
        {
            case "古い鍵":
                break;

            case "盾":
                break;

            case "使用人のメモ①":
                break;
        }

        currentStatue = null;
        isOpen = false;
        inventoryPanel.SetActive(false);

        // このフレームのE入力をプレイヤー側で再利用させない
        LastInventoryActionFrame = Time.frameCount;

        return;
    }
}
