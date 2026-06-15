using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class SceneDoor_n : MonoBehaviour
{
    public string sceneName;

    public GameObject moveText;

    private bool playerInside;

    void Start()
    {
        moveText.SetActive(false);
    }

    void Update()
    {
        if (playerInside)
        {
            moveText.SetActive(true);

            if (Keyboard.current.eKey.wasPressedThisFrame)
            {
                SceneManager.LoadScene(sceneName);
            }
        }
        else
        {
            moveText.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
        }
    }
}