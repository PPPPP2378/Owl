using UnityEngine;

public class Painting_n : MonoBehaviour
{
    [Header("絵画名")]
    public string paintingName;

    [TextArea]
    public string description;

    [Header("この絵画の正解番号")]
    public int correctAnswer;

    [HideInInspector]
    public bool isSolved = false;

    public PaintingPuzzle_n puzzle;

    public void ShowInfo()
    {
        MessageManager_n.instance.ShowMessage(description);
    }

    // 選択肢で選ばれた時に呼ぶ
    public void SelectAnswer(int answer)
    {
        if (answer == correctAnswer)
        {
            isSolved = true;
            Debug.Log(gameObject.name + " 正解");

            if (puzzle != null)
                puzzle.CheckAnswer();
        }
        else
        {
            isSolved = false;
            Debug.Log(gameObject.name + " 不正解");
            // 後でここに「違うようだ」などのメッセージを追加
        }
    }
}
