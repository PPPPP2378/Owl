using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class Owlwork_n : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float gridSize = 1f;

    // WallレイヤーをInspectorで設定
    public LayerMask wallLayer;

    private bool isMoving;
    private Vector3 targetPosition;

    void Start()
    {
        targetPosition = transform.position;
    }

    void Update()
    {
        if (isMoving) return;

        Vector3 direction = Vector3.zero;

        if (Keyboard.current.wKey.wasPressedThisFrame)
            direction = Vector3.up;

        else if (Keyboard.current.sKey.wasPressedThisFrame)
            direction = Vector3.down;

        else if (Keyboard.current.aKey.wasPressedThisFrame)
            direction = Vector3.left;

        else if (Keyboard.current.dKey.wasPressedThisFrame)
            direction = Vector3.right;

        if (direction != Vector3.zero)
        {
            Vector3 nextPos = transform.position + direction * gridSize;

            // 壁チェック
            Collider2D hit = Physics2D.OverlapCircle(
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
        targetPosition = endPos;

        isMoving = false;
    }
}
