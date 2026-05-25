using UnityEngine;

public class Owlwork_n : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float gridSize = 1f;

    private bool isMoving = false;
    private Vector3 targetPosition;

    void Start()
    {
        targetPosition = transform.position;
    }

    void Update()
    {
        // ˆÚ“®’†‚Í“ü—Í‚ðŽó‚¯•t‚¯‚È‚¢
        if (isMoving) return;

        Vector3 direction = Vector3.zero;

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            direction = Vector3.up;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            direction = Vector3.down;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            direction = Vector3.left;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            direction = Vector3.right;
        }

        if (direction != Vector3.zero)
        {
            targetPosition += direction * gridSize;
            StartCoroutine(Move());
        }
    }

    System.Collections.IEnumerator Move()
    {
        isMoving = true;

        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPosition,
                moveSpeed * Time.deltaTime
            );

            yield return null;
        }

        transform.position = targetPosition;
        isMoving = false;
    }
}
