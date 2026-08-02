//PaintingDoor_n.cs
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PaintingDoor_n : MonoBehaviour
{
    public static PaintingDoor_n Instance;

    public bool isSolved = false;

    [Header("移動先シーン名")]
    public string nextSceneName;

    bool canMove = false;

    void Awake()
    {
        Instance = this;
    }

    public void ShowQuestion()
    {
        // 2回目以降はシーン移動
        if (canMove)
        {
            SceneManager.LoadScene(nextSceneName);
            return;
        }

        if (isSolved)
        {
            MessageManager_n.instance.ShowMessage("扉は開いている……");

            SceneManager.LoadScene(nextSceneName);
            return;
        }

        MessageManager_n.instance.currentPainting = null;
        MessageManager_n.instance.isDoorChoice = true;

        MessageManager_n.instance.ShowMessage(
            "この部屋にある真作は一枚だけ。\n\nどうする？"
        );

        MessageManager_n.instance.ShowChoice(
            "選ぶ",
            "もう一度調べる"
        );
    }

    public void OpenDoor()
    {
        isSolved = true;

        MessageManager_n.instance.ShowMessage(
            "鍵が外れる音がした。"
        );
    }
}
