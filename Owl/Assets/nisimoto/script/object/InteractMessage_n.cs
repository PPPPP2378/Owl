using UnityEngine;

public class InteractMessage_n : MonoBehaviour
{
    [TextArea(3, 5)]
    public string message;

    public void Interact()
    {
        MessageManager_n.instance.ShowMessage(message);
    }
}
