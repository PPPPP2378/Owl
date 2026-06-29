using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class TitleManager_n : MonoBehaviour
{
    public TextMeshProUGUI startText;
    public TextMeshProUGUI howToText;
    public TextMeshProUGUI optionText;
    public TextMeshProUGUI exitText;

    public GameObject titlePanel;
    public GameObject howToPanel;
    public GameObject optionPanel;
    public TextMeshProUGUI voiceOnText;
    public TextMeshProUGUI voiceOffText;

    private int optionID = 0;
    private int selectID = 0;

    enum MenuState
    {
        Title,
        HowTo,
        Option
    }

    MenuState state = MenuState.Title;

    void Start()
    {
        titlePanel.SetActive(true);
        howToPanel.SetActive(false);
        optionPanel.SetActive(false);

        optionID =
            AudioManager_n.Instance.owlVoice ? 0 : 1;

        UpdateText();
        UpdateOptionText();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) && state == MenuState.Title)
        {
            selectID--;

            if (selectID < 0)
                selectID = 3;

            UpdateText();
        }

        if (Input.GetKeyDown(KeyCode.S) && state == MenuState.Title)
        {
            selectID++;

            if (selectID > 3)
                selectID = 0;

            UpdateText();
        }
        if (Input.GetKeyDown(KeyCode.E) &&
            state == MenuState.Title)
        {
            switch (selectID)
            {
                case 0:
                    SceneManager.LoadScene("Map1");
                    break;

                case 1:
                    state = MenuState.HowTo;

                    titlePanel.SetActive(false);
                    howToPanel.SetActive(true);
                    break;

                case 2:
                    state = MenuState.Option;

                    titlePanel.SetActive(false);
                    optionPanel.SetActive(true);
                    break;

                case 3:
#if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
                    break;
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape) &&
    state != MenuState.Title)
        {
            state = MenuState.Title;

            titlePanel.SetActive(true);
            howToPanel.SetActive(false);
            optionPanel.SetActive(false);
        }

        if (state == MenuState.Option)
        {
            if (Input.GetKeyDown(KeyCode.W) ||
                Input.GetKeyDown(KeyCode.S))
            {
                optionID = 1 - optionID;

                UpdateOptionText();
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                if (optionID == 0)
                {
                    AudioManager_n.Instance.SetOwlVoice(true);
                }
                else
                {
                    AudioManager_n.Instance.SetOwlVoice(false);
                }
            }
        }
    }
    void UpdateText()
    {
        startText.text = "  START";
        howToText.text = "  HOW TO";
        optionText.text = "  OPTION";
        exitText.text = "  EXIT";

        switch (selectID)
        {
            case 0:
                startText.text = "> START";
                break;

            case 1:
                howToText.text = "> HOW TO";
                break;

            case 2:
                optionText.text = "> OPTION";
                break;

            case 3:
                exitText.text = "> EXIT";
                break;
        }
    }

    void UpdateOptionText()
    {
        voiceOnText.text = "  ñ¬Ç´ê∫ ON";
        voiceOffText.text = "  ñ¬Ç´ê∫ OFF";

        if (optionID == 0)
            voiceOnText.text = "> ñ¬Ç´ê∫ ON";
        else
            voiceOffText.text = "> ñ¬Ç´ê∫ OFF";
    }
}