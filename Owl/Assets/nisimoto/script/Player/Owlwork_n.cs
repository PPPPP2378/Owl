using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class Owlwork_n : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float gridSize = 1f;

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

        // Eキーで調べる
        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            CheckInteraction();
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
        Vector2 checkPos =
            (Vector2)transform.position +
            (Vector2)facingDirection * gridSize;

        Collider2D hit =
            Physics2D.OverlapCircle(
                checkPos,
                0.2f,
                interactLayer
            );

        if (hit != null)
        {
            InteractMessage_n message =
                hit.GetComponent<InteractMessage_n>();

            if (message != null)
            {
                message.Interact();
            }
        }
    }
}
