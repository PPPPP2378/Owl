//CameraMove.cs
using UnityEngine;
using TMPro;

public class CameraMove : MonoBehaviour
{
    public float speed = 2f;
    public float stopY = 10f;

    public GameObject finText;   // Finのオブジェクト

    private AudioSource audioSource;
    private bool stopped = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (finText != null)
            finText.SetActive(false);
    }

    void Update()
    {
        if (!stopped)
        {
            Vector3 pos = transform.position;
            pos.y += speed * Time.deltaTime;

            if (pos.y >= stopY)
            {
                pos.y = stopY;
                stopped = true;

                audioSource.Play();

                if (finText != null)
                    finText.SetActive(true);
            }

            transform.position = pos;
        }
    }
}