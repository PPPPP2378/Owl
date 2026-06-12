using UnityEngine;
using TMPro;

public class MessageManager_n : MonoBehaviour
{
    public static MessageManager_n instance;

    public TextMeshProUGUI messageText;

    void Awake()
    {
        instance = this;
        if (messageText != null)
        {
            messageText.text = "";
        }
    }

    public void ShowMessage(string message)
    {
        if (messageText != null)
        {
            messageText.text = message;
        }
    }

    public void HideMessage()
    {
        messageText.text = "";
    }
}
