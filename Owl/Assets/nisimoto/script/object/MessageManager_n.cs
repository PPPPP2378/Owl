//MessageManager_n.cs
using UnityEngine;
using TMPro;
using System.Collections;

public class MessageManager_n : MonoBehaviour
{
    public static MessageManager_n instance;

    public TextMeshProUGUI messageText;

    public GameObject choicePanel;
    public TextMeshProUGUI choiceText1;
    public TextMeshProUGUI choiceText2;

    public Painting_n currentPainting;

    int selectChoice = 0;

    string currentChoice1;
    string currentChoice2;

    public bool isChoiceActive = false;
    public bool isDoorChoice = false;

    bool canSelect = false;

    IEnumerator DelayChoiceInput()
    {
        canSelect = false;

        yield return null;

        canSelect = true;
    }

    void Awake()
    {
        instance = this;
    }

    public void ShowMessage(string message)
    {
        Debug.Log("ShowMessage開始");

        if (messageText == null)
        {
            Debug.LogError("messageText が null");
            return;
        }

        // ←これを追加
        messageText.gameObject.SetActive(true);

        messageText.text = message;

        Debug.Log("表示文字：" + message);
    }

    public void ShowChoice(string choice1, string choice2)
    {
        currentChoice1 = choice1;
        currentChoice2 = choice2;

        selectChoice = 0;  // 必ず調べる側

        choicePanel.SetActive(true);

        isChoiceActive = true;


        UpdateChoice();

        StartCoroutine(DelayChoiceInput());

        Debug.Log("選択肢開始 selectChoice=" + selectChoice);
    }

    void Update()
    {
        if (PaintingSelect_n.Instance != null &&
     PaintingSelect_n.Instance.panel.activeSelf)
        {
            return;
        }


        if (Input.GetKeyDown(KeyCode.Space))
        {
            HideMessage();
            HideChoice();
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            selectChoice = 0;
            UpdateChoice();
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            selectChoice = 1;
            UpdateChoice();
        }

        if (Input.GetKeyDown(KeyCode.E) &&
     choicePanel.activeSelf &&
     canSelect)
        {
            SelectChoice();
        }
    }

    public void HideMessage()
    {
        Debug.Log("HideMessage実行");

        messageText.text = "";
        messageText.gameObject.SetActive(false);

        choicePanel.SetActive(false);
    }

    public void HideChoice()
    {
        choicePanel.SetActive(false);
        isChoiceActive = false;
    }


    void UpdateChoice()
    {
        if (selectChoice == 0)
        {
            choiceText1.text = "> " + currentChoice1;
            choiceText2.text = "  " + currentChoice2;
        }
        else
        {
            choiceText1.text = "  " + currentChoice1;
            choiceText2.text = "> " + currentChoice2;
        }
    }

    void SelectChoice()
    {
        Debug.Log("決定処理開始");
        Debug.Log("selectChoice = " + selectChoice);

        if (selectChoice == 0)
        {
            Debug.Log("調べるが選ばれた");

            HideChoice();

            // ===== 絵画 =====
            if (currentPainting != null)
            {
                currentPainting.CheckPainting();
                return;
            }

            // ===== 絵画の扉 =====
            if (isDoorChoice)
            {
                PaintingSelect_n.Instance.Open();
                return;
            }
        }
        else
        {
            Debug.Log("やめるが選ばれた");

            HideChoice();
            HideMessage();
        }
    }
}
