//Owlwork_n.cs
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class Owlwork_n : MonoBehaviour
{
    //====================
    // 移動設定
    //====================

    public float moveSpeed = 5f;
    public float gridSize = 1f;
    public float repeatDelay = 0.2f;

    private float nextMoveTime;
    private bool isMoving;

    // 向いている方向
    private Vector3 facingDirection = Vector3.down;


    //====================
    // UI
    //====================

    // 「調べる」のUI
    public GameObject interactText;

    // メモ画像などを表示するパネル
    public GameObject infoPanel;
    public Image infoImage;

    // 現在は未使用
    public Sprite mysteryImage;


    //====================
    // 調べる処理
    //====================

    // 今プレイヤーの前にあるMystery2
    private Collider2D currentMystery;

    public WallSequenceDoor_n wallSequenceDoor;

    // 調べるレイヤー
    // 現在のCheckFrontObjectでは未使用
    public LayerMask interactLayer;


    //====================
    // 壁判定
    //====================

    public LayerMask wallLayer;


    //====================
    // 暗闇システム
    //====================

    public bool darkVision = false;

    // 前方何マス照らすか
    public int visionLength = 5;

    public Tilemap darknessTilemap;
    public TileBase darkTile;

    // 前回照らした場所
    private readonly List<Vector3Int> lastVisionTiles =
        new List<Vector3Int>();


    //====================
    // プレイヤー画像
    //====================

    public SpriteRenderer spriteRenderer;

    public Sprite downSprite;
    public Sprite upSprite;
    public Sprite leftSprite;
    public Sprite rightSprite;


    //====================
    // 初期化
    //====================

    private void Start()
    {
        if (interactText != null)
        {
            interactText.SetActive(false);
        }

        if (infoPanel != null)
        {
            infoPanel.SetActive(false);
        }

        darkVision = true;

        UpdateVision();
    }


    //====================
    // 毎フレーム処理
    //====================

    private void Update()
    {
        if (Keyboard.current == null)
        {
            return;
        }

        // 最優先で詳細パネルの閉じる入力を処理する
        if (HandleInfoPanel())
        {
            return;
        }

        // 他のUIを開いている間は操作を止める
        if (IsPlayerInputBlocked())
        {
            return;
        }

        // 移動中は新しい移動を受け付けない
        if (isMoving)
        {
            return;
        }

        // 入力方向を取得する
        Vector3 direction = GetInputDirection();

        // プレイヤーの前にあるオブジェクトを確認
        CheckFrontObject();

        // Eキーで調べる
        HandleInteractionInput();

        // 移動
        if (direction != Vector3.zero)
        {
            TryMove(direction);
        }
    }


    //====================
    // 詳細パネル
    //====================

    /// <summary>
    /// メモ画像などの詳細パネルを閉じる。
    /// パネルが開いている場合はtrueを返す。
    /// </summary>
    private bool HandleInfoPanel()
    {
        if (infoPanel == null ||
            !infoPanel.activeInHierarchy)
        {
            return false;
        }

        bool closePressed =
            Keyboard.current.spaceKey.wasPressedThisFrame ||
            Keyboard.current.eKey.wasPressedThisFrame ||
            Keyboard.current.escapeKey.wasPressedThisFrame;

        if (closePressed)
        {
            infoPanel.SetActive(false);

            Debug.Log("詳細パネルを閉じました");
        }

        // パネル表示中は移動などをさせない
        return true;
    }


    //====================
    // 操作停止判定
    //====================

    private bool IsPlayerInputBlocked()
    {
        // インベントリが開いている
        if (InventoryUI_n.Instance != null &&
            InventoryUI_n.Instance.IsOpen)
        {
            return true;
        }

        // 選択肢が表示されている
        if (MessageManager_n.instance != null &&
            MessageManager_n.instance.isChoiceActive)
        {
            return true;
        }

        // アイテム詳細が開いている
        if (ItemInfoUI_n.Instance != null &&
            ItemInfoUI_n.Instance.IsOpen)
        {
            return true;
        }

        return false;
    }


    //====================
    // 移動入力
    //====================

    private Vector3 GetInputDirection()
    {
        if (Time.time < nextMoveTime)
        {
            return Vector3.zero;
        }

        Vector3 direction = Vector3.zero;

        if (Keyboard.current.wKey.isPressed)
        {
            direction = Vector3.up;
            SetFacingDirection(direction, upSprite);
        }
        else if (Keyboard.current.sKey.isPressed)
        {
            direction = Vector3.down;
            SetFacingDirection(direction, downSprite);
        }
        else if (Keyboard.current.aKey.isPressed)
        {
            direction = Vector3.left;
            SetFacingDirection(direction, leftSprite);
        }
        else if (Keyboard.current.dKey.isPressed)
        {
            direction = Vector3.right;
            SetFacingDirection(direction, rightSprite);
        }

        if (direction != Vector3.zero)
        {
            nextMoveTime = Time.time + repeatDelay;
        }

        return direction;
    }


    private void SetFacingDirection(
        Vector3 direction,
        Sprite directionSprite)
    {
        facingDirection = direction;

        if (spriteRenderer != null &&
            directionSprite != null)
        {
            spriteRenderer.sprite = directionSprite;
        }

        // 向きが変わった時点で視界を更新
        UpdateVision();
    }


    //====================
    // 移動処理
    //====================

    private void TryMove(Vector3 direction)
    {
        Vector3 nextPos =
            transform.position + direction * gridSize;

        Collider2D wallHit = Physics2D.OverlapCircle(
            nextPos,
            0.2f,
            wallLayer
        );

        // 壁がある場合は移動しない
        if (wallHit != null)
        {
            return;
        }

        StartCoroutine(Move(direction));
    }


    private IEnumerator Move(Vector3 direction)
    {
        isMoving = true;

        Vector3 startPos = transform.position;
        Vector3 endPos =
            startPos + direction * gridSize;

        float elapsedTime = 0f;

        // moveSpeedが0以下でもエラーにならないようにする
        float safeSpeed = Mathf.Max(moveSpeed, 0.01f);
        float moveTime = 1f / safeSpeed;

        while (elapsedTime < moveTime)
        {
            float rate = elapsedTime / moveTime;

            transform.position = Vector3.Lerp(
                startPos,
                endPos,
                rate
            );

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        transform.position = endPos;

        isMoving = false;

        // 移動完了後に前方と視界を更新
        CheckFrontObject();
        UpdateVision();
    }


    //====================
    // Eキー入力
    //====================

    private void HandleInteractionInput()
    {
        if (!Keyboard.current.eKey.wasPressedThisFrame)
        {
            return;
        }

        // 選択肢表示中は調べられない
        if (MessageManager_n.instance != null &&
            MessageManager_n.instance.isChoiceActive)
        {
            return;
        }

        // 通常メッセージ表示中は新しく調べない
        if (MessageManager_n.instance != null &&
            MessageManager_n.instance.messageText != null &&
            MessageManager_n.instance.messageText.gameObject.activeSelf)
        {
            return;
        }

        CheckInteraction();
    }


    //====================
    // 調べる対象の処理
    //====================

    private void CheckInteraction()
    {
        if (currentMystery == null)
        {
            Debug.Log("調べる対象がありません");
            return;
        }

        Debug.Log("調べる対象：" + currentMystery.name);


        //====================
        // 謎の壁
        //====================

        MysteryWall_n wall =
            currentMystery.GetComponent<MysteryWall_n>();

        if (wall != null)
        {
            wall.isChecked = true;

            if (wallSequenceDoor != null)
            {
                wallSequenceDoor.CheckWall(
                    wall.wallNumber
                );
            }

            currentMystery = null;

            SetInteractTextVisible(false);
            CheckFrontObject();

            return;
        }


        //====================
        // アイテム
        //====================

        Item_n item =
            currentMystery.GetComponent<Item_n>();

        if (item != null)
        {
            item.GetItem();

            currentMystery = null;

            SetInteractTextVisible(false);

            return;
        }


        //====================
        // 石像
        //====================

        Statue_n statue =
            currentMystery.GetComponent<Statue_n>();

        if (statue != null)
        {
            Debug.Log("石像を調べました");

            if (InventoryUI_n.Instance != null)
            {
                InventoryUI_n.Instance.OpenForStatue(
                    statue
                );
            }

            return;
        }


        //====================
        // 絵画
        //====================

        Painting_n painting =
            currentMystery.GetComponent<Painting_n>();

        if (painting != null)
        {
            Debug.Log("絵画の詳細を表示します");

            painting.ShowInfo();

            return;
        }


        //====================
        // 絵画の扉
        //====================

        PaintingDoor_n door =
            currentMystery.GetComponent<PaintingDoor_n>();

        if (door != null)
        {
            door.ShowQuestion();

            return;
        }


        //====================
        // メモ
        //====================

        Memo_n memo =
            currentMystery.GetComponent<Memo_n>();

        if (memo != null)
        {
            ShowMemo(memo);

            return;
        }

        Debug.LogWarning(
            currentMystery.name +
            "には対応する調べる処理がありません"
        );
    }


    //====================
    // メモ表示
    //====================

    private void ShowMemo(Memo_n memo)
    {
        if (memo.memoImage != null)
        {
            if (infoImage == null ||
                infoPanel == null)
            {
                Debug.LogWarning(
                    "infoImageまたはinfoPanelが設定されていません"
                );

                return;
            }

            infoImage.sprite = memo.memoImage;
            infoPanel.SetActive(true);

            Debug.Log("メモ画像を表示しました");

            return;
        }

        if (MessageManager_n.instance != null)
        {
            MessageManager_n.instance.ShowMessage(
                memo.memoText
            );
        }
    }


    //====================
    // 前方のオブジェクト確認
    //====================

    private void CheckFrontObject()
    {
        Vector2 checkPos =
            (Vector2)transform.position +
            (Vector2)facingDirection * gridSize;

        Collider2D[] hits =
            Physics2D.OverlapCircleAll(
                checkPos,
                0.7f
            );

        currentMystery = null;

        foreach (Collider2D hit in hits)
        {
            if (!hit.CompareTag("Mystery2"))
            {
                continue;
            }

            // 調査済みの謎の壁は除外
            MysteryWall_n wall =
                hit.GetComponent<MysteryWall_n>();

            if (wall != null && wall.isChecked)
            {
                continue;
            }

            currentMystery = hit;
            break;
        }

        SetInteractTextVisible(
            currentMystery != null
        );
    }


    private void SetInteractTextVisible(bool visible)
    {
        if (interactText != null)
        {
            interactText.SetActive(visible);
        }
    }


    //====================
    // 暗闇の視界処理
    //====================

    private void UpdateVision()
    {
        if (!darkVision ||
            darknessTilemap == null)
        {
            return;
        }

        // 前回照らした場所を暗闇に戻す
        foreach (Vector3Int pos in lastVisionTiles)
        {
            darknessTilemap.SetTile(
                pos,
                darkTile
            );
        }

        lastVisionTiles.Clear();

        Vector3 currentPos = transform.position;

        for (int i = 1; i <= visionLength; i++)
        {
            Vector3 worldPos =
                currentPos +
                facingDirection * i * gridSize;

            Collider2D wall = Physics2D.OverlapCircle(
                worldPos,
                0.2f,
                wallLayer
            );

            // 壁より先は照らさない
            if (wall != null)
            {
                break;
            }

            Vector3Int cell =
                darknessTilemap.WorldToCell(
                    worldPos
                );

            // 黒いタイルを消す
            darknessTilemap.SetTile(
                cell,
                null
            );

            // 後で元に戻すために保存
            lastVisionTiles.Add(cell);
        }
    }
}