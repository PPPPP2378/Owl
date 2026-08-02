//PaintingSelect_n.cs
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PaintingSelect_n : MonoBehaviour
{
    public static PaintingSelect_n Instance;

    public GameObject panel;

    public TextMeshProUGUI text1;
    public TextMeshProUGUI text2;
    public TextMeshProUGUI text3;
    public TextMeshProUGUI text4;

    int select = 0;

    string[] paintingNames =
    {
        "耳飾りの少女",
        "作者の自画像",
        "サン＝ベルナール峠を越える皇帝",
        "モザリナ"
    };

    // ← モザリナが正解
    int answer = 3;

    bool isOpen = false;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        panel.SetActive(false);
    }

    public void Open()
    {
        isOpen = true;
        select = 0;

        panel.SetActive(true);

        MessageManager_n.instance.isChoiceActive = true;

        UpdateText();
    }

    public void Close()
    {
        isOpen = false;

        panel.SetActive(false);

        MessageManager_n.instance.isChoiceActive = false;
    }

    void Update()
    {
        if (!isOpen)
            return;

        if (Keyboard.current.wKey.wasPressedThisFrame)
        {
            select--;

            if (select < 0)
                select = 3;

            UpdateText();
        }

        if (Keyboard.current.sKey.wasPressedThisFrame)
        {
            select++;

            if (select > 3)
                select = 0;

            UpdateText();
        }

        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            Decide();
        }
    }

    void UpdateText()
    {
        text1.text = (select == 0 ? "> " : "  ") + paintingNames[0];
        text2.text = (select == 1 ? "> " : "  ") + paintingNames[1];
        text3.text = (select == 2 ? "> " : "  ") + paintingNames[2];
        text4.text = (select == 3 ? "> " : "  ") + paintingNames[3];
    }

    void Decide()
    {
        if (select == answer)
        {
            Close();

            MessageManager_n.instance.ShowMessage(
                "正解だ。扉の鍵が外れた。"
            );

            PaintingDoor_n.Instance.OpenDoor();
        }
        else
        {
            Close();

            MessageManager_n.instance.ShowMessage(
                "違うようだ……"
            );
        }
    }
}
