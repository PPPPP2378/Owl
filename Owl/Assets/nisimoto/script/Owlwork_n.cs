using UnityEngine;

public class Owlwork_n : MonoBehaviour
{
    public float moveDistance = 1f;
    public float moveSpeed = 5f;

    private Vector2 targetPosition;
    private bool isMoving;

    void Start()
    {
        targetPosition = transform.position;
    }

    void Update()
    {
        HandleInput();
        Move();
    }

    void HandleInput()
    {
        if (isMoving) return;

        Vector2 direction = Vector2.zero;

        if (Input.GetKey(KeyCode.W))
            direction = Vector2.up;
        else if (Input.GetKey(KeyCode.S))
            direction = Vector2.down;
        else if (Input.GetKey(KeyCode.A))
            direction = Vector2.left;
        else if (Input.GetKey(KeyCode.D))
            direction = Vector2.right;

        if (direction != Vector2.zero)
        {
            targetPosition += direction * moveDistance;
            isMoving = true;
        }
    }

    void Move()
    {
        if (!isMoving) return;

        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPosition,
            moveSpeed * Time.deltaTime
        );

        Vector2 currentPos = transform.position;

        if (Vector2.Distance(currentPos, targetPosition) < 0.01f)
        {
            transform.position = new Vector3(targetPosition.x, targetPosition.y, transform.position.z);
            isMoving = false;
        }
    }
}