//ChoiceUI_n.cs
using UnityEngine;
using TMPro;


public class ChoiceUI_n : MonoBehaviour
{
    public static ChoiceUI_n Instance;

    public GameObject panel;
    public TextMeshProUGUI[] choices;

    private Painting_n currentPainting;

    private void Awake()
    {
        Instance = this;
        panel.SetActive(false);
    }

    public void Open(Painting_n painting, string[] texts)
    {
        currentPainting = painting;

        for (int i = 0; i < choices.Length; i++)
        {
            if (i < texts.Length)
            {
                choices[i].text = texts[i];
                choices[i].gameObject.SetActive(true);
            }
            else
            {
                choices[i].gameObject.SetActive(false);
            }
        }

        panel.SetActive(true);
    }

    public void Select(int index)
    {
        if (currentPainting != null)
        {
            currentPainting.SelectAnswer(index);
        }

        Close();
    }

    public void Close()
    {
        currentPainting = null;
        panel.SetActive(false);
    }
}
