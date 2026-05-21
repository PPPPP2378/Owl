using UnityEngine;

public class Owlwork_n : MonoBehaviour
{
    public float moveDistance = 1f;
    public float moveSpeed = 5f;

    private Vector3 targetPosition;
    private bool isMoving;

    void Start()
    {
        targetPosition = transform.position;
    }

    void Update()
    {
        // ˆÚ“®’†
        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPosition,
                moveSpeed * Time.deltaTime
            );

            // “ž’…‚µ‚½‚ç’âŽ~
            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                transform.position = targetPosition;
                isMoving = false;
            }

            return;
        }

        Vector3 direction = Vector3.zero;

        // WASD“ü—Í
        if (Input.GetKeyDown(KeyCode.W))
        {
            direction = Vector3.up;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            direction = Vector3.down;
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            direction = Vector3.left;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            direction = Vector3.right;
        }

        // ˆÚ“®ŠJŽn
        if (direction != Vector3.zero)
        {
            targetPosition += direction * moveDistance;
            isMoving = true;
        }
    }
};