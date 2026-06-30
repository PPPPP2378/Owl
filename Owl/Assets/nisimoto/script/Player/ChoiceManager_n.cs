using UnityEngine;
using TMPro;

public class ChoiceManager_n : MonoBehaviour
{
    public TextMeshProUGUI[] choiceTexts;

    private int selectID = 0;

    public int GetChoice()
    {
        return selectID;
    }

    void Start()
    {
        UpdateChoice();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            selectID--;

            if (selectID < 0)
                selectID = choiceTexts.Length - 1;

            UpdateChoice();
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            selectID++;

            if (selectID >= choiceTexts.Length)
                selectID = 0;

            UpdateChoice();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("‘I‘ð : " + selectID);
        }
    }

    void UpdateChoice()
    {
        for (int i = 0; i < choiceTexts.Length; i++)
        {
            choiceTexts[i].text =
                choiceTexts[i].text.Replace("> ", "  ");

            if (i == selectID)
                choiceTexts[i].text =
                    "> " + choiceTexts[i].text.Trim();
        }
    }
}
