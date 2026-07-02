using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class Owlwork_n : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float gridSize = 1f;

    public GameObject interactText;//”調べる”のUI

    private Collider2D currentMystery;//今見ているTag「Mystery2」オブジェクト

    public WallSequenceDoor_n wallSequenceDoor;

    public GameObject infoPanel;
    public Image infoImage;
    public Sprite mysteryImage;

    // 壁レイヤー
    public LayerMask wallLayer;

    // 調べるレイヤー
    public LayerMask interactLayer;

    //====================
    // 暗闇システム
    //====================

    // 暗闇モード
    public bool darkVision = false;

    // 前方何マス照らすか
    public int visionLength = 5;

    // 暗闇タイル
    public Tilemap darknessTilemap;

    // 黒いタイル
    public TileBase darkTile;

    // 前回照らした場所
    private List<Vector3Int> lastVisionTiles = new List<Vector3Int>();

    private bool isMoving;

    // 向いている方向
    private Vector3 facingDirection = Vector3.down;

    void Update()
    {

        if (isMoving) return;

        if (InventoryUI_n.Instance != null)
        {
            Debug.Log("Inventory Open = " + InventoryUI_n.Instance.IsOpen);
            if (InventoryUI_n.Instance.IsOpen)
            {
                Debug.Log("プレイヤー操作停止");
                return;
            }
        }

    

        Vector3 direction = Vector3.zero;

        // WASD入力
        if (Keyboard.current.wKey.wasPressedThisFrame)
        {
            direction = Vector3.up;
            facingDirection = direction;
        }

        else if (Keyboard.current.sKey.wasPressedThisFrame)
        {
            direction = Vector3.down;
            facingDirection = direction;
        }

        else if (Keyboard.current.aKey.wasPressedThisFrame)
        {
            direction = Vector3.left;
            facingDirection = direction;
        }

        else if (Keyboard.current.dKey.wasPressedThisFrame)
        {
            direction = Vector3.right;
            facingDirection = direction;
        }

        CheckFrontObject();

        // Eキーで調べる
        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            Debug.Log("Owlwork UpdateでE検知");

            if (infoPanel.activeSelf)
            {
                infoPanel.SetActive(false);
            }
            else
            {
                CheckInteraction();
            }
        }

        // 移動処理
        if (direction != Vector3.zero)
        {
            Vector3 nextPos =
                transform.position + direction * gridSize;

            Collider2D hit =
                Physics2D.OverlapCircle(
                    nextPos,
                    0.2f,
                    wallLayer
                );

            // 壁がなければ移動
            if (hit == null)
            {
                StartCoroutine(Move(direction));
            }
            UpdateVision();
        }
        if (ItemInfoUI_n.Instance != null &&
     ItemInfoUI_n.Instance.IsOpen)
        {
            return;
        }
    }

    IEnumerator Move(Vector3 direction)
    {
        isMoving = true;

        Vector3 startPos = transform.position;
        Vector3 endPos = startPos + direction * gridSize;

        float elapsedTime = 0f;
        float moveTime = 1f / moveSpeed;

        while (elapsedTime < moveTime)
        {
            transform.position = Vector3.Lerp(
                startPos,
                endPos,
                elapsedTime / moveTime
            );

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        transform.position = endPos;

        isMoving = false;
    }

    // 前を調べる
    void CheckInteraction()
    {
        Debug.Log("CheckInteraction開始");

        if (currentMystery == null) return;

        MysteryWall_n wall =
            currentMystery.GetComponent<MysteryWall_n>();

        if (wall != null)
        {
            wall.isChecked = true;

            wallSequenceDoor.CheckWall(wall.wallNumber);

            currentMystery = null;
            interactText.SetActive(false);

            CheckFrontObject();

            return;
        }

        // アイテム
        Item_n item = currentMystery.GetComponent<Item_n>();

        if (item != null)
        {
            item.GetItem();

            currentMystery = null;
            interactText.SetActive(false);

            return;
        }

        // 像
        Statue_n statue = currentMystery.GetComponent<Statue_n>();

        Debug.Log("statue = " + statue);

        if (statue != null)
        {
            Debug.Log("石像を調べた");
            InventoryUI_n.Instance.OpenForStatue(statue);
            return;
        }

        Memo_n memo =currentMystery.GetComponent<Memo_n>();

        if (memo != null)
        {
            infoImage.sprite = memo.memoImage;
            infoPanel.SetActive(true);
        }
    }

    void CheckFrontObject()
    {
        Vector2 checkPos =
            (Vector2)transform.position +
            (Vector2)facingDirection * gridSize;

        Collider2D[] hits =
            Physics2D.OverlapCircleAll(checkPos, 0.7f);

        currentMystery = null;

        foreach (Collider2D hit in hits)
        {
            Debug.Log(hit.name + " / " + hit.tag);

            if (!hit.CompareTag("Mystery2")) continue;

            MysteryWall_n wall = hit.GetComponent<MysteryWall_n>();
            if (wall != null && wall.isChecked)
            {
                continue;
            }

            currentMystery = hit;
            break;
        }

        interactText.SetActive(currentMystery != null);
    }

    void Start()
    {
        interactText.SetActive(false);
        infoPanel.SetActive(false);

        darkVision = true;

        UpdateVision();
    }

    void UpdateVision()
    {
        // 暗闇部屋じゃないなら何もしない
        if (!darkVision || darknessTilemap == null)
            return;

        // 前回照らした場所を元に戻す
        foreach (Vector3Int pos in lastVisionTiles)
        {
            darknessTilemap.SetTile(pos, darkTile);
        }

        lastVisionTiles.Clear();

        Vector3 currentPos = transform.position;

        for (int i = 1; i <= visionLength; i++)
        {
            Vector3 worldPos = currentPos + facingDirection * i * gridSize;

            // 壁ならそこで終了
            Collider2D wall =
                Physics2D.OverlapCircle(
                    worldPos,
                    0.2f,
                    wallLayer);

            if (wall != null)
                break;

            Vector3Int cell = darknessTilemap.WorldToCell(worldPos);

            // 黒タイルを消す
            darknessTilemap.SetTile(cell, null);

            // 戻すため保存
            lastVisionTiles.Add(cell);
        }
    }
}


