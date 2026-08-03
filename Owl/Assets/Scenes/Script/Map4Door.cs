using UnityEngine;
using UnityEngine.SceneManagement;

public class Map4Door_n : MonoBehaviour
{
    [Header("Map4のパズル管理")]
    public Map4Puzzle_n puzzle;

    [Header("移動先のエンディングシーン名")]
    public string endingSceneName = "Ending";

    public void Interact()
    {
        if (puzzle == null)
        {
            Debug.LogError(
                "Map4Door_nにMap4Puzzleが設定されていません。"
            );

            return;
        }

        // 扉がまだ閉じている場合は正解判定
        if (!puzzle.IsDoorOpen)
        {
            puzzle.CheckDoor();
            return;
        }

        // 扉が開いた後に、もう一度調べるとEndingへ移動
        if (string.IsNullOrEmpty(endingSceneName))
        {
            Debug.LogError(
                "Ending Scene Nameが設定されていません。"
            );

            return;
        }

        SceneManager.LoadScene(endingSceneName);
    }
}