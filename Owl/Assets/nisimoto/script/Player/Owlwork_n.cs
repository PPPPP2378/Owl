using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.UI;

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

    private bool isMoving;

    // 向いている方向
    private Vector3 facingDirection = Vector3.down;

    void Update()
    {

        if (isMoving) return;

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

        infoImage.sprite = mysteryImage;
        infoPanel.SetActive(true);
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
            if (!hit.CompareTag("Mystery2")) continue;

            MysteryWall_n wall =
                hit.GetComponent<MysteryWall_n>();

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
    }
}
