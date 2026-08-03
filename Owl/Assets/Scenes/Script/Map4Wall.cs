using UnityEngine;

public class Map4Wall_n : MonoBehaviour
{
    [Header("壁番号：Ⅰ=1、Ⅱ=2、Ⅲ=3")]
    public int wallNumber;

    [Header("Map4のパズル管理")]
    public Map4Puzzle_n puzzle;

    public void Interact()
    {
        if (puzzle == null)
        {
            Debug.LogError(
                gameObject.name +
                " にMap4Puzzleが設定されていません。"
            );

            return;
        }

        puzzle.SelectWall(wallNumber);
    }
}