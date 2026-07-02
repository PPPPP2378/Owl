using TMPro;
using UnityEngine;

public class ItemInfoUI_n : MonoBehaviour
{
    public static ItemInfoUI_n Instance;

    public GameObject panel;
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI itemDescriptionText;

    private void Awake()
    {
        Instance = this;

        panel.SetActive(false);
    }

    void Update()
    {
        if (panel.activeSelf && Input.GetKeyDown(KeyCode.E))
        {
            panel.SetActive(false);
        }
    }

    public void Show(string itemName, string description)
    {
        itemNameText.text = itemName;
        itemDescriptionText.text = description;

        panel.SetActive(true);
    }
}
