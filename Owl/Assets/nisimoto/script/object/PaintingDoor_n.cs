//PaintingDoor_n.cs
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

public class PaintingDoor_n : MonoBehaviour
{
    public static PaintingDoor_n Instance;

    public bool isSolved = false;

    [Header("移動先シーン名")]
    public string nextSceneName;

    bool canMove = false;

    [Header("扉のタイル変更")]
    public Tilemap doorTilemap;
    public Vector3Int doorTilePosition;
    public TileBase openDoorTile;

    [Header("開いた後に有効化するオブジェクト")]
    public GameObject doorTriggerObject;

    public AudioClip unlockSE;

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
        if (isSolved)
        {
            return;
        }

        isSolved = true;

        if (unlockSE != null)
        {
            FindFirstObjectByType<AudioSource>().PlayOneShot(unlockSE);
        }

        if (doorTilemap != null && openDoorTile != null)
        {
            doorTilemap.SetTile(
                doorTilePosition,
                openDoorTile
            );
        }
        else
        {
            Debug.LogWarning(
                "PaintingDoor_nの扉TilemapまたはOpenDoorTileが未設定です"
            );
        }

        if (doorTriggerObject != null)
        {
            doorTriggerObject.SetActive(true);
        }

        MessageManager_n.instance.ShowMessage(
            "鍵が外れる音がした。"
        );
    }
}
