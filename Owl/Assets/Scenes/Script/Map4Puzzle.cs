using UnityEngine;
using UnityEngine.Tilemaps;

public class Map4Puzzle_n : MonoBehaviour
{
    [Header("石像（左から順番に登録）")]
    public Statue_n[] statues;

    [Header("壁Ⅰの武器順")]
    public WeaponType_n[] wall1Answer = new WeaponType_n[5];

    [Header("壁Ⅱの武器順・本当の正解")]
    public WeaponType_n[] wall2Answer = new WeaponType_n[5];

    [Header("壁Ⅲの武器順")]
    public WeaponType_n[] wall3Answer = new WeaponType_n[5];

    [Header("扉のTilemap")]
    public Tilemap doorTilemap;

    [Header("扉のセル座標")]
    public Vector3Int doorTilePosition;

    [Header("開いた扉のTile")]
    public TileBase openDoorTile;

    [Header("解錠SE")]
    public AudioClip unlockSE;

    // 現在調べた壁
    // 0：未選択
    // 1：壁Ⅰ
    // 2：壁Ⅱ
    // 3：壁Ⅲ
    private int selectedWall;

    public bool IsDoorOpen { get; private set; }

    private void SetWeaponsAutomatically(
    WeaponType_n[] weaponOrder)
    {
        if (statues == null ||
            weaponOrder == null ||
            statues.Length != weaponOrder.Length)
        {
            Debug.LogError(
                "石像数と武器順の数が一致していません。"
            );
            return;
        }

        for (int i = 0; i < statues.Length; i++)
        {
            Statue_n statue = statues[i];

            if (statue == null)
            {
                continue;
            }

            // 現在のアイテムとの関連を解除
            if (statue.currentItem != null)
            {
                statue.currentItem.isPlaced = false;
                statue.currentItem = null;
            }

            // 対応する武器を自動表示
            statue.SetWeapon(weaponOrder[i]);
        }
    }

    // ========================================
    // 壁を調べる
    // ========================================

    public void SelectWall(int wallNumber)
    {
        if (IsDoorOpen)
        {
            MessageManager_n.instance.ShowMessage(
                "扉はすでに開いている。"
            );
            return;
        }

        if (wallNumber < 1 || wallNumber > 3)
        {
            Debug.LogWarning(
                "無効な壁番号です：" + wallNumber
            );
            return;
        }

        selectedWall = wallNumber;

        WeaponType_n[] selectedAnswer =
            GetSelectedWallAnswer();

        SetWeaponsAutomatically(selectedAnswer);

        string romanNumber = GetRomanNumber(wallNumber);

        MessageManager_n.instance.ShowMessage(
            "壁の「" + romanNumber + "」を調べた。\n" +
            "石像の武器が変化した。"
        );
    }

    // ========================================
    // 扉を調べて正解判定
    // ========================================

    public void CheckDoor()
    {
        if (IsDoorOpen)
        {
            return;
        }

        if (selectedWall == 0)
        {
            MessageManager_n.instance.ShowMessage(
                "先に壁の印を調べよう。"
            );

            return;
        }

        WeaponType_n[] selectedAnswer =
            GetSelectedWallAnswer();

        if (selectedAnswer == null ||
            selectedAnswer.Length != statues.Length)
        {
            Debug.LogError(
                "壁の武器順と石像の数が一致していません。"
            );

            return;
        }

        // 選択した壁に書かれた武器順との比較
        for (int i = 0; i < statues.Length; i++)
        {
            if (statues[i].currentWeapon != selectedAnswer[i])
            {
                MessageManager_n.instance.ShowMessage(
                    "武器の順番が違うようだ……"
                );

                return;
            }
        }

        // 武器順が合っていても、壁Ⅱ以外は不正解
        if (selectedWall != 2)
        {
            MessageManager_n.instance.ShowMessage(
                "武器の順番は合っている。\n" +
                "しかし、扉は反応しない……"
            );

            return;
        }

        OpenDoor();
    }

    // ========================================
    // 選択中の壁の武器順を取得
    // ========================================

    private WeaponType_n[] GetSelectedWallAnswer()
    {
        switch (selectedWall)
        {
            case 1:
                return wall1Answer;

            case 2:
                return wall2Answer;

            case 3:
                return wall3Answer;

            default:
                return null;
        }
    }

    // ========================================
    // 扉を開く
    // ========================================

    private void OpenDoor()
    {
        IsDoorOpen = true;

        if (unlockSE != null)
        {
            FindFirstObjectByType<AudioSource>().PlayOneShot(unlockSE);
        }

        if (doorTilemap != null)
        {
            doorTilemap.SetTile(
                doorTilePosition,
                openDoorTile
            );
        }
        else
        {
            Debug.LogWarning(
                "Door Tilemapが設定されていません。"
            );
        }

        MessageManager_n.instance.ShowMessage(
            "正しい壁と武器の順番だった。\n" +
            "扉が開いた。\n\n" +
            "もう一度扉を調べると移動できる。"
        );
    }

    private string GetRomanNumber(int wallNumber)
    {
        switch (wallNumber)
        {
            case 1:
                return "Ⅰ";

            case 2:
                return "Ⅱ";

            case 3:
                return "Ⅲ";

            default:
                return wallNumber.ToString();
        }
    }
}