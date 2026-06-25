using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class TitleManager_n : MonoBehaviour
{
    public TextMeshProUGUI startText;
    public TextMeshProUGUI menuText;
    public TextMeshProUGUI exitText;

    private int selectID = 0;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            selectID--;

            if (selectID < 0)
                selectID = 2;

            UpdateText();

            Debug.Log("‘I‘ð’† : " + selectID);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            selectID++;

            if (selectID > 2)
                selectID = 0;

            UpdateText();

            Debug.Log("‘I‘ð’† : " + selectID);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            switch (selectID)
            {
                case 0:
                    SceneManager.LoadScene("Map1");
                    break;

                case 1:
                    Debug.Log("MENU");
                    break;

                case 2:
#if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
                    break;
            }
        }
    }
    void Start()
    {
        Debug.Log("Start“®‚¢‚½");

        UpdateText();
    }

    void UpdateText()
    {
        Debug.Log("UpdateTextŽÀs");

        startText.text = "  START";
        menuText.text = "  MENU";
        exitText.text = "  EXIT";

        switch (selectID)
        {
            case 0:
                startText.text = "> START";
                break;

            case 1:
                menuText.text = "> MENU";
                break;

            case 2:
                exitText.text = "> EXIT";

                break;
        }
    }
}